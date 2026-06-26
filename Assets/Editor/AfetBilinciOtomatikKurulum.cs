using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AfetBilinciOtomatikKurulum
{
    private const string GirisSahnesi = "Assets/Scenes/GirisEkrani.unity";
    private const string CantaSahnesi = "Assets/Scenes/DepremCantasiSahnesi.unity";
    private const string EvSahnesi = "Assets/Scenes/SampleScene.unity";
    private const string YolSahnesi = "Assets/Scenes/DisariYolSecimiSahnesi.unity";

    [MenuItem("Afet Bilinci/Otomatik Kurulumu Calistir")]
    public static void OtomatikKurulumuCalistir()
    {
        KurGirisSahnesi();
        KurCantaSahnesi();
        KurEvSahnesi();
        KurYolSahnesi();

        AssetDatabase.SaveAssets();
        Debug.Log("Afet Bilinci otomatik kurulumu tamamlandi.");
    }

    public static void BatchKurulum()
    {
        OtomatikKurulumuCalistir();
        EditorApplication.Exit(0);
    }

    private static void KurGirisSahnesi()
    {
        OpenScene(GirisSahnesi);

        GirisKontrol[] kontroller = Object.FindObjectsByType<GirisKontrol>(FindObjectsSortMode.None);
        foreach (GirisKontrol kontrol in kontroller)
        {
            SerializedObject serialized = new SerializedObject(kontrol);
            bool eksikInput = serialized.FindProperty("isimGiris").objectReferenceValue == null;
            if (eksikInput)
            {
                Object.DestroyImmediate(kontrol);
            }
        }

        SaveScene();
    }

    private static void KurCantaSahnesi()
    {
        OpenScene(CantaSahnesi);

        CantaKontrol cantaKontrol = Object.FindFirstObjectByType<CantaKontrol>();
        TextMeshProUGUI durumYazisi = FindTmp("CantaDurumYazisi") ?? FindAnyTmp();

        if (cantaKontrol != null)
        {
            SerializedObject serialized = new SerializedObject(cantaKontrol);
            serialized.FindProperty("durumYazisi").objectReferenceValue = durumYazisi;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(cantaKontrol);
        }

        KurCantaEsyasi("Kup_Su", CantaEsyasiEtkilesimi.EsyaTuru.Dogru, "E tuşuna bas: Suyu al", "Su çanta için iyi bir seçim.", cantaKontrol, durumYazisi);
        KurCantaEsyasi("Kup_Fener", CantaEsyasiEtkilesimi.EsyaTuru.Dogru, "E tuşuna bas: Feneri al", "Fener çanta için iyi bir seçim.", cantaKontrol, durumYazisi);
        KurCantaEsyasi("Kup_Laptop", CantaEsyasiEtkilesimi.EsyaTuru.Yanlis, "E tuşuna bas: Kontrol et", "Laptop şimdi gerekli değil.", cantaKontrol, durumYazisi);
        KurCantaEsyasi("Kup_Oyuncak", CantaEsyasiEtkilesimi.EsyaTuru.Yanlis, "E tuşuna bas: Kontrol et", "Oyuncak bekleyebilir. Acil eşyaları seç.", cantaKontrol, durumYazisi);

        SaveScene();
    }

    private static void KurEvSahnesi()
    {
        OpenScene(EvSahnesi);

        TextMeshProUGUI talimatYazisi = FindTmp("TalimatYazisi") ?? FindTmp("GeriBildirimMetni") ?? FindAnyTmp();
        TextMeshProUGUI geriBildirimYazisi = FindTmp("GeriBildirimMetni") ?? talimatYazisi;
        GameObject devamButonu = GameObject.Find("DevamButonu");

        GameObject gorevAlani = GameObject.Find("CokKapanTutunAlani");
        if (gorevAlani == null)
        {
            gorevAlani = new GameObject("CokKapanTutunAlani");
            gorevAlani.transform.position = new Vector3(0f, 1f, 2f);
            gorevAlani.transform.localScale = new Vector3(3f, 2f, 3f);
        }

        BoxCollider collider = gorevAlani.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = gorevAlani.AddComponent<BoxCollider>();
        }
        collider.isTrigger = true;

        CokKapanTutunGorevi gorev = GetOrAdd<CokKapanTutunGorevi>(gorevAlani);
        gorev.yonergeYazisi = talimatYazisi;
        gorev.bilgiYazisi = geriBildirimYazisi;
        gorev.gerekliSure = 3f;
        gorev.tamamlanincaAktifOlacak = devamButonu;
        EditorUtility.SetDirty(gorev);

        Camera mainCamera = Camera.main ?? Object.FindFirstObjectByType<Camera>();
        if (mainCamera != null)
        {
            YumusakKameraSarsintisi sarsinti = GetOrAdd<YumusakKameraSarsintisi>(mainCamera.gameObject);
            sarsinti.sarsintiAktif = false;
            sarsinti.siddet = 0.015f;
            EditorUtility.SetDirty(sarsinti);
        }

        KurTahliyeKontrol("Asansor", talimatYazisi, null);
        KurTahliyeKontrol("Merdiven", talimatYazisi, devamButonu);

        SaveScene();
    }

    private static void KurYolSahnesi()
    {
        OpenScene(YolSahnesi);

        GameObject oyuncu = GameObject.Find("Oyuncu");
        if (oyuncu != null)
        {
            foreach (TahliyeKontrol kontrol in oyuncu.GetComponents<TahliyeKontrol>())
            {
                Object.DestroyImmediate(kontrol);
            }
        }

        TextMeshProUGUI yolYazisi = FindTmp("YolDurumYazisi") ?? FindAnyTmp();
        KurSokakSecimi("Yol_Guvenli", yolYazisi);
        KurSokakSecimi("Yol_Tehlikeli", yolYazisi);

        SaveScene();
    }

    private static void KurCantaEsyasi(string objeAdi, CantaEsyasiEtkilesimi.EsyaTuru tur, string yonerge, string yanlisMesaj, CantaKontrol cantaKontrol, TextMeshProUGUI yazi)
    {
        GameObject obje = GameObject.Find(objeAdi);
        if (obje == null)
        {
            Debug.LogWarning(objeAdi + " bulunamadi.");
            return;
        }

        Collider collider = obje.GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
            EditorUtility.SetDirty(collider);
        }

        CantaEsyasiEtkilesimi etkilesim = GetOrAdd<CantaEsyasiEtkilesimi>(obje);
        etkilesim.esyaTuru = tur;
        etkilesim.yonergeMetni = yonerge;
        etkilesim.yanlisMesaj = yanlisMesaj;
        etkilesim.cantaKontrol = cantaKontrol;
        etkilesim.yonergeYazisi = yazi;
        EditorUtility.SetDirty(etkilesim);
    }

    private static void KurTahliyeKontrol(string objeAdi, TextMeshProUGUI yazi, GameObject devamButonu)
    {
        GameObject obje = GameObject.Find(objeAdi);
        if (obje == null)
        {
            return;
        }

        Collider collider = obje.GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
            EditorUtility.SetDirty(collider);
        }

        TahliyeKontrol kontrol = GetOrAdd<TahliyeKontrol>(obje);
        kontrol.uiMetni = yazi;
        kontrol.devamButonu = devamButonu;
        EditorUtility.SetDirty(kontrol);
    }

    private static void KurSokakSecimi(string objeAdi, TextMeshProUGUI yazi)
    {
        GameObject obje = GameObject.Find(objeAdi);
        if (obje == null)
        {
            return;
        }

        Collider collider = obje.GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
            EditorUtility.SetDirty(collider);
        }

        SokakSecimi secim = GetOrAdd<SokakSecimi>(obje);
        secim.durumYazisi = yazi;
        EditorUtility.SetDirty(secim);
    }

    private static T GetOrAdd<T>(GameObject target) where T : Component
    {
        T component = target.GetComponent<T>();
        if (component == null)
        {
            component = target.AddComponent<T>();
        }
        return component;
    }

    private static TextMeshProUGUI FindTmp(string objectName)
    {
        GameObject obj = GameObject.Find(objectName);
        return obj != null ? obj.GetComponent<TextMeshProUGUI>() : null;
    }

    private static TextMeshProUGUI FindAnyTmp()
    {
        return Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None).FirstOrDefault();
    }

    private static void OpenScene(string path)
    {
        EditorSceneManager.OpenScene(path);
    }

    private static void SaveScene()
    {
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
    }
}
