using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class AfetBilinciSahneDekorasyonu
{
    private const string DisSahneDuzeltmeAnahtari = "AfetBilinci.DisSahneFinal.v1";
    private const string CantaSahnesi = "Assets/Scenes/DepremCantasiSahnesi.unity";
    private const string EvSahnesi = "Assets/Scenes/SampleScene.unity";
    private const string YolSahnesi = "Assets/Scenes/DisariYolSecimiSahnesi.unity";
    private const string KenneyMobilya = "Assets/ThirdParty/KenneyFurnitureKit/Models/FBX format/";

    [MenuItem("Afet Bilinci/Sahneleri Cocuk Dostu Dekore Et")]
    public static void DekoreEt()
    {
        FinalCocukDostuCila.Uygula();
    }

    public static void BatchDekoreEt()
    {
        DekoreEt();
        EditorApplication.Exit(0);
    }

    [MenuItem("Afet Bilinci/Canta Sahnesini Duzenle")]
    public static void CantaSahnesiniDuzenle()
    {
        OpenScene(CantaSahnesi);
        DekoreEtCantaSahnesi();
        CantaGorevNesneleriniDuzenle();
        CantaKamerasiniDuzenle();
        CantaArayuzunuDuzenle();
        SaveScene();
        AssetDatabase.SaveAssets();
        Debug.Log("Canta sahnesi cocuk odasi ve E etkilesimi icin duzenlendi.");
    }

    [MenuItem("Afet Bilinci/Dis Sahneyi Sadelestir ve Duzelt")]
    public static void DisSahneyiDuzelt()
    {
        OpenScene(YolSahnesi);

        GameObject eklenenDekor = GameObject.Find("Dekor_ToplanmaAlani");
        if (eklenenDekor != null)
        {
            Object.DestroyImmediate(eklenenDekor);
        }

        GameObject oyuncu = GameObject.Find("Oyuncu");
        if (oyuncu != null)
        {
            oyuncu.tag = "Player";

            MeshRenderer kapsulGorseli = oyuncu.GetComponent<MeshRenderer>();
            if (kapsulGorseli != null)
            {
                kapsulGorseli.enabled = false;
                EditorUtility.SetDirty(kapsulGorseli);
            }

            Camera kamera = oyuncu.GetComponentInChildren<Camera>(true);
            if (kamera != null)
            {
                kamera.transform.localPosition = new Vector3(0f, 3.2f, -5.6f);
                kamera.transform.localEulerAngles = new Vector3(10f, 0f, 0f);
                EditorUtility.SetDirty(kamera.transform);
            }

            foreach (Transform cocuk in oyuncu.transform)
            {
                if (kamera != null && cocuk == kamera.transform)
                {
                    continue;
                }

                cocuk.localPosition = new Vector3(0f, -1f, 0f);
                cocuk.localScale = Vector3.one * 0.5f;
                EditorUtility.SetDirty(cocuk);
            }

            EditorUtility.SetDirty(oyuncu);
        }

        GameObject guvenliYol = GameObject.Find("Yol_Guvenli");
        if (guvenliYol != null)
        {
            guvenliYol.tag = "Untagged";
            EditorUtility.SetDirty(guvenliYol);
        }

        GameObject riskliYol = GameObject.Find("Yol_Tehlikeli");
        if (riskliYol != null)
        {
            riskliYol.tag = "Untagged";
            EditorUtility.SetDirty(riskliYol);
        }

        TextMeshProUGUI yolYazisi = FindTmp("YolDurumYazisi");
        if (yolYazisi != null)
        {
            yolYazisi.text = "Güvenli yolu seç.";
            yolYazisi.fontSize = 28f;
            yolYazisi.enableAutoSizing = true;
            yolYazisi.fontSizeMin = 20f;
            yolYazisi.fontSizeMax = 30f;
            yolYazisi.raycastTarget = false;

            RectTransform rect = yolYazisi.rectTransform;
            rect.sizeDelta = new Vector2(900f, 90f);
            EditorUtility.SetDirty(rect);
            EditorUtility.SetDirty(yolYazisi);
        }

        SaveScene();
        Debug.Log("Dis sahne sadelestirildi; oyuncu ve kamera olcegi duzeltildi.");
    }

    [InitializeOnLoadMethod]
    private static void DisSahneFinaliniOtomatikUygula()
    {
        EditorApplication.delayCall += () =>
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode ||
                EditorPrefs.GetBool(DisSahneDuzeltmeAnahtari, false))
            {
                return;
            }

            DisSahneyiDuzelt();
            EditorPrefs.SetBool(DisSahneDuzeltmeAnahtari, true);
        };
    }

    private static void DekoreEtCantaSahnesi()
    {
        SetSceneObjectActive("Zemin", false);
        SetSceneObjectActive("Duvar_Arka", false);
        SetSceneObjectActive("duvarr", false);
        SetSceneObjectActive("little_glass_table", false);

        GameObject root = GetOrCreateRoot("Dekor_CocukOdasi");
        ClearChildren(root);

        Material zemin = GetOrCreateMaterial("CocukOdasi_Zemin", new Color(0.54f, 0.72f, 0.67f));
        Material duvar = GetOrCreateMaterial("CocukOdasi_Duvar", new Color(0.94f, 0.86f, 0.68f));
        Material altDuvar = GetOrCreateMaterial("CocukOdasi_AltDuvar", new Color(0.39f, 0.66f, 0.70f));

        CreateBox(root, "OdaZemini", new Vector3(0f, -0.12f, 3.6f), new Vector3(10f, 0.24f, 9f), zemin);
        CreateBox(root, "ArkaDuvar", new Vector3(0f, 1.75f, 8.05f), new Vector3(10f, 3.5f, 0.18f), duvar);
        CreateBox(root, "SolDuvar", new Vector3(-5.05f, 1.75f, 4.9f), new Vector3(0.18f, 3.5f, 6.3f), duvar);
        CreateBox(root, "SagDuvar", new Vector3(5.05f, 1.75f, 4.9f), new Vector3(0.18f, 3.5f, 6.3f), duvar);
        CreateBox(root, "ArkaDuvarSeridi", new Vector3(0f, 0.65f, 7.93f), new Vector3(9.8f, 1.3f, 0.08f), altDuvar);

        SpawnFitted(KenneyMobilya + "rugRounded.fbx", root, "OyunHalisi", new Vector3(0f, 0.01f, 3.1f), Vector3.zero, new Vector3(4.4f, 0.08f, 3.2f));
        SpawnFitted(KenneyMobilya + "bedSingle.fbx", root, "CocukYatagi", new Vector3(3.25f, 0f, 4.65f), new Vector3(0f, -90f, 0f), new Vector3(2.25f, 1.25f, 3.8f));
        SpawnFitted(KenneyMobilya + "cabinetBedDrawerTable.fbx", root, "Komodin", new Vector3(3.6f, 0f, 6.85f), Vector3.zero, new Vector3(1.15f, 1.1f, 1.05f));
        SpawnFitted(KenneyMobilya + "lampRoundTable.fbx", root, "GeceLambasi", new Vector3(3.6f, 1.02f, 6.85f), Vector3.zero, new Vector3(0.55f, 0.75f, 0.55f));

        SpawnFitted(KenneyMobilya + "desk.fbx", root, "CalismaMasasi", new Vector3(-1.45f, 0f, 6.85f), new Vector3(0f, 180f, 0f), new Vector3(2.8f, 1.35f, 1.25f));
        SpawnFitted(KenneyMobilya + "chairDesk.fbx", root, "MasaSandalyesi", new Vector3(-1.45f, 0f, 5.55f), Vector3.zero, new Vector3(0.9f, 1.15f, 0.9f));
        SpawnFitted(KenneyMobilya + "bookcaseOpen.fbx", root, "Kitaplik", new Vector3(-4.15f, 0f, 6.8f), Vector3.zero, new Vector3(1.45f, 2.6f, 0.75f));
        SpawnFitted(KenneyMobilya + "books.fbx", root, "Kitaplar", new Vector3(-4.15f, 1.05f, 6.45f), Vector3.zero, new Vector3(0.9f, 0.5f, 0.45f));
        SpawnFitted(KenneyMobilya + "bookcaseOpenLow.fbx", root, "AlcakRaf", new Vector3(-3.65f, 0f, 3.65f), new Vector3(0f, 90f, 0f), new Vector3(1.4f, 1.15f, 0.75f));

        SpawnFitted(KenneyMobilya + "pottedPlant.fbx", root, "OdaBitkisi", new Vector3(4.15f, 0f, 7.25f), Vector3.zero, new Vector3(0.8f, 1.35f, 0.8f));
        SpawnFitted(KenneyMobilya + "radio.fbx", root, "Radyo", new Vector3(-4.1f, 1.45f, 6.6f), Vector3.zero, new Vector3(0.65f, 0.5f, 0.4f));
        SpawnFitted(KenneyMobilya + "wallWindow.fbx", root, "PencereDuvari", new Vector3(1.4f, 0f, 7.94f), Vector3.zero, new Vector3(2.4f, 3.2f, 0.3f));
        SpawnFitted(KenneyMobilya + "wallDoorway.fbx", root, "KapiDuvari", new Vector3(-3.25f, 0f, 7.94f), Vector3.zero, new Vector3(2.1f, 3.2f, 0.3f));
    }

    private static void CantaGorevNesneleriniDuzenle()
    {
        SetTaskObject(
            "Kup_Su",
            new Vector3(3.6f, 1.02f, 6.45f),
            "E tuşuna bas: Suyu al",
            "Assets/Low Poly Cartoon Food and Groceries Pack/Prefabs/water_bottle.prefab",
            new Vector3(0.32f, 0.7f, 0.32f),
            Vector3.zero);
        SetTaskObject(
            "Kup_Fener",
            new Vector3(-3.65f, 1.02f, 3.65f),
            "E tuşuna bas: Feneri al",
            "Assets/Akduman/ToonTastic - Electronic Devices/Prefabs/Prop_FlashLight.prefab",
            new Vector3(0.65f, 0.35f, 0.35f),
            new Vector3(0f, 25f, 0f));
        SetTaskObject(
            "Kup_Laptop",
            new Vector3(-1.45f, 1.15f, 6.55f),
            "E tuşuna bas: Kontrol et",
            KenneyMobilya + "laptop.fbx",
            new Vector3(0.85f, 0.45f, 0.65f),
            new Vector3(0f, 180f, 0f));
        SetTaskObject(
            "Kup_Oyuncak",
            new Vector3(2.7f, 0f, 2.35f),
            "E tuşuna bas: Kontrol et",
            KenneyMobilya + "bear.fbx",
            new Vector3(0.7f, 0.9f, 0.65f),
            new Vector3(0f, -20f, 0f));

        GameObject oyuncu = GameObject.Find("Oyuncu");
        if (oyuncu != null)
        {
            oyuncu.tag = "Player";
            oyuncu.transform.position = new Vector3(0f, 1f, 0.3f);
            oyuncu.transform.eulerAngles = Vector3.zero;
            EditorUtility.SetDirty(oyuncu);
        }
    }

    private static void SetTaskObject(
        string name,
        Vector3 position,
        string prompt,
        string visualPath,
        Vector3 visualSize,
        Vector3 visualRotation)
    {
        GameObject obj = GameObject.Find(name);
        if (obj == null)
        {
            return;
        }

        obj.transform.position = position;
        obj.transform.localScale = Vector3.one;
        ClearChildren(obj);
        SpawnFitted(visualPath, obj, name + "_Gorsel", position, visualRotation, visualSize);

        BoxCollider trigger = obj.GetComponent<BoxCollider>();
        if (trigger != null)
        {
            trigger.isTrigger = true;
            trigger.center = new Vector3(0f, 0.55f, 0f);
            trigger.size = new Vector3(1.8f, 1.8f, 1.8f);
            EditorUtility.SetDirty(trigger);
        }

        CantaEsyasiEtkilesimi interaction = obj.GetComponent<CantaEsyasiEtkilesimi>();
        if (interaction != null)
        {
            interaction.yonergeMetni = prompt;
            EditorUtility.SetDirty(interaction);
        }

        EditorUtility.SetDirty(obj.transform);
    }

    private static void CantaKamerasiniDuzenle()
    {
        Camera camera = Camera.main;
        if (camera == null)
        {
            return;
        }

        camera.transform.localPosition = new Vector3(0f, 2.25f, -3.8f);
        camera.transform.localEulerAngles = new Vector3(7f, 0f, 0f);
        camera.fieldOfView = 66f;
        camera.backgroundColor = new Color(0.55f, 0.76f, 0.86f);
        EditorUtility.SetDirty(camera);
        EditorUtility.SetDirty(camera.transform);
    }

    private static void CantaArayuzunuDuzenle()
    {
        TextMeshProUGUI durum = FindTmp("CantaDurumYazisi");
        if (durum != null)
        {
            durum.text = "Su ve feneri bul. Yaklaş ve E tuşuna bas.";
            durum.fontSize = 34f;
            durum.enableAutoSizing = true;
            durum.fontSizeMin = 24f;
            durum.fontSizeMax = 36f;
            durum.fontStyle = FontStyles.Bold;
            durum.color = Color.white;
            durum.alignment = TextAlignmentOptions.Center;
            durum.raycastTarget = false;

            RectTransform rect = durum.rectTransform;
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(0f, -72f);
            rect.sizeDelta = new Vector2(940f, 90f);

            Image panel = GetOrCreatePanel(durum.transform.parent, "GorevArkaPlani");
            RectTransform panelRect = panel.rectTransform;
            panelRect.anchorMin = new Vector2(0.5f, 1f);
            panelRect.anchorMax = new Vector2(0.5f, 1f);
            panelRect.anchoredPosition = new Vector2(0f, -72f);
            panelRect.sizeDelta = new Vector2(1020f, 108f);
            panel.color = new Color(0.06f, 0.18f, 0.24f, 0.86f);
            panel.transform.SetAsFirstSibling();

            EditorUtility.SetDirty(panel);
            EditorUtility.SetDirty(panelRect);
            EditorUtility.SetDirty(rect);
            EditorUtility.SetDirty(durum);
        }

        GameObject buttonObject = GameObject.Find("TahliyeyeBaslaButonu");
        if (buttonObject == null)
        {
            return;
        }

        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
        Image buttonImage = buttonObject.GetComponent<Image>();
        TextMeshProUGUI buttonText = buttonObject.GetComponentInChildren<TextMeshProUGUI>(true);

        if (buttonRect != null)
        {
            buttonRect.anchorMin = new Vector2(0.5f, 0f);
            buttonRect.anchorMax = new Vector2(0.5f, 0f);
            buttonRect.anchoredPosition = new Vector2(0f, 75f);
            buttonRect.sizeDelta = new Vector2(380f, 76f);
            EditorUtility.SetDirty(buttonRect);
        }

        if (buttonImage != null)
        {
            buttonImage.color = new Color(0.18f, 0.62f, 0.35f, 1f);
            EditorUtility.SetDirty(buttonImage);
        }

        if (buttonText != null)
        {
            buttonText.text = "Hazırım, devam et";
            buttonText.fontSize = 30f;
            buttonText.fontStyle = FontStyles.Bold;
            buttonText.color = Color.white;
            EditorUtility.SetDirty(buttonText);
        }
    }

    private static void DekoreEtEvSahnesi()
    {
        OpenScene(EvSahnesi);
        GameObject root = GetOrCreateRoot("Dekor_GuvenliEv");
        ClearChildren(root);

        Spawn("Assets/KayKit/Packs/KayKit - Free Sample (for Unity)/Prefabs/Primitive_Floor.prefab", root, "SicakHali", new Vector3(0f, 0.04f, 0.8f), Vector3.zero, new Vector3(5.5f, 0.08f, 4f));
        Spawn("Assets/KayKit/Packs/KayKit - Free Sample (for Unity)/Prefabs/Primitive_Wall.prefab", root, "SolRaf", new Vector3(-2.7f, 1.1f, 1.5f), new Vector3(0f, 90f, 0f), new Vector3(2.4f, 1.8f, 0.18f));
        Spawn("Assets/KayKit/Packs/KayKit - Free Sample (for Unity)/Prefabs/Primitive_Cube.prefab", root, "GuvenliMasa", new Vector3(0f, 0.55f, 1.55f), Vector3.zero, new Vector3(1.8f, 0.25f, 1.1f));
        Spawn("Assets/KayKit/Packs/KayKit - Free Sample (for Unity)/Prefabs/Primitive_Cube_Small.prefab", root, "MasaAyak1", new Vector3(-0.7f, 0.25f, 1.15f), Vector3.zero, new Vector3(0.18f, 0.5f, 0.18f));
        Spawn("Assets/KayKit/Packs/KayKit - Free Sample (for Unity)/Prefabs/Primitive_Cube_Small.prefab", root, "MasaAyak2", new Vector3(0.7f, 0.25f, 1.15f), Vector3.zero, new Vector3(0.18f, 0.5f, 0.18f));
        Spawn("Assets/KayKit/Packs/KayKit - Free Sample (for Unity)/Prefabs/Primitive_Cube_Small.prefab", root, "MasaAyak3", new Vector3(-0.7f, 0.25f, 1.95f), Vector3.zero, new Vector3(0.18f, 0.5f, 0.18f));
        Spawn("Assets/KayKit/Packs/KayKit - Free Sample (for Unity)/Prefabs/Primitive_Cube_Small.prefab", root, "MasaAyak4", new Vector3(0.7f, 0.25f, 1.95f), Vector3.zero, new Vector3(0.18f, 0.5f, 0.18f));
        Spawn("Assets/LowpolyStreetPack/Prefabs/StreetProps/PlantPots/QuickBuild/PlantPot_B_V01.prefab", root, "EvBitkisi", new Vector3(2.15f, 0f, 2.2f), new Vector3(0f, -20f, 0f), Vector3.one);
        Spawn("Assets/Doors/Prefabs/door_frame.prefab", root, "CikisKapiCercevesi", new Vector3(2.4f, 0.8f, -1.25f), new Vector3(0f, 90f, 0f), new Vector3(1.2f, 1.2f, 1.2f));
        Spawn("Assets/Doors/Prefabs/door.prefab", root, "CikisKapisi", new Vector3(2.42f, 0.8f, -1.25f), new Vector3(0f, 90f, 0f), new Vector3(1.2f, 1.2f, 1.2f));

        SaveScene();
    }

    private static void DekoreEtYolSahnesi()
    {
        OpenScene(YolSahnesi);
        GameObject root = GetOrCreateRoot("Dekor_ToplanmaAlani");
        ClearChildren(root);

        Spawn("Assets/LowpolyStreetPack/Prefabs/Roads/Streets/Road_Streight.prefab", root, "AnaYol", new Vector3(0f, 0f, 0f), Vector3.zero, new Vector3(1.4f, 1f, 1.4f));
        Spawn("Assets/LowpolyStreetPack/Prefabs/StreetProps/Bench/Bench_A.prefab", root, "ToplanmaBanki", new Vector3(-2f, 0f, 2.2f), new Vector3(0f, 25f, 0f), Vector3.one);
        Spawn("Assets/LowpolyStreetPack/Prefabs/StreetProps/ParkLamp/ParkLamp.prefab", root, "Aydinlatma", new Vector3(2.2f, 0f, 1.8f), Vector3.zero, Vector3.one);
        Spawn("Assets/LowpolyStreetPack/Prefabs/StreetProps/Signs/StreetSigns/StreetSign_A.prefab", root, "GuvenliAlanTabela", new Vector3(0.6f, 0f, 2.4f), new Vector3(0f, -20f, 0f), Vector3.one);
        Spawn("Assets/LowpolyStreetPack/Prefabs/Foliage/Trees/Tree_A_V01.prefab", root, "Agac1", new Vector3(-3f, 0f, 3f), Vector3.zero, Vector3.one);
        Spawn("Assets/LowpolyStreetPack/Prefabs/Foliage/Bushes/Bush_A.prefab", root, "Calilik1", new Vector3(2.9f, 0f, 2.7f), Vector3.zero, Vector3.one);
        Spawn("Assets/LowpolyStreetPack/Prefabs/StreetProps/RoadBlocks/RoadCone_A.prefab", root, "RiskliYolUyarisi1", new Vector3(-1.2f, 0f, -2.2f), Vector3.zero, Vector3.one);
        Spawn("Assets/LowpolyStreetPack/Prefabs/StreetProps/RoadBlocks/RoadCone_A.prefab", root, "RiskliYolUyarisi2", new Vector3(-0.7f, 0f, -2.2f), Vector3.zero, Vector3.one);

        SaveScene();
    }

    private static GameObject CreateBox(GameObject parent, string name, Vector3 position, Vector3 scale, Material material)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = name;
        box.transform.SetParent(parent.transform);
        box.transform.position = position;
        box.transform.localScale = scale;

        Renderer renderer = box.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial = material;
        }

        EditorUtility.SetDirty(box);
        return box;
    }

    private static void SetSceneObjectActive(string name, bool active)
    {
        GameObject obj = GameObject.Find(name);
        if (obj == null)
        {
            return;
        }

        obj.SetActive(active);
        EditorUtility.SetDirty(obj);
    }

    private static Material GetOrCreateMaterial(string name, Color color)
    {
        const string generatedFolder = "Assets/Generated";
        const string materialFolder = "Assets/Generated/AfetBilinciMaterials";

        if (!AssetDatabase.IsValidFolder(generatedFolder))
        {
            AssetDatabase.CreateFolder("Assets", "Generated");
        }

        if (!AssetDatabase.IsValidFolder(materialFolder))
        {
            AssetDatabase.CreateFolder(generatedFolder, "AfetBilinciMaterials");
        }

        string path = materialFolder + "/" + name + ".mat";
        Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (material == null)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            material = new Material(shader);
            AssetDatabase.CreateAsset(material, path);
        }

        material.color = color;
        EditorUtility.SetDirty(material);
        return material;
    }

    private static Image GetOrCreatePanel(Transform parent, string name)
    {
        Transform existing = parent.Find(name);
        GameObject panelObject;

        if (existing != null)
        {
            panelObject = existing.gameObject;
        }
        else
        {
            panelObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            panelObject.transform.SetParent(parent, false);
        }

        return panelObject.GetComponent<Image>();
    }

    private static GameObject SpawnFitted(
        string path,
        GameObject parent,
        string name,
        Vector3 floorPosition,
        Vector3 rotation,
        Vector3 targetSize)
    {
        GameObject instance = Spawn(path, parent, name, Vector3.zero, rotation, Vector3.one);
        if (instance == null)
        {
            return null;
        }

        Renderer[] renderers = instance.GetComponentsInChildren<Renderer>(true);
        if (renderers.Length == 0)
        {
            instance.transform.position = floorPosition;
            return instance;
        }

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        Vector3 size = bounds.size;
        float scale = Mathf.Min(
            targetSize.x / Mathf.Max(size.x, 0.001f),
            targetSize.y / Mathf.Max(size.y, 0.001f),
            targetSize.z / Mathf.Max(size.z, 0.001f));

        instance.transform.localScale = Vector3.one * scale;

        renderers = instance.GetComponentsInChildren<Renderer>(true);
        bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        Vector3 offset = floorPosition - new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
        instance.transform.position += offset;
        EditorUtility.SetDirty(instance.transform);
        return instance;
    }

    private static GameObject Spawn(string path, GameObject parent, string name, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (prefab == null)
        {
            Debug.LogWarning("Prefab bulunamadi: " + path);
            return null;
        }

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instance.name = name;
        instance.transform.SetParent(parent.transform);
        instance.transform.position = position;
        instance.transform.eulerAngles = rotation;
        instance.transform.localScale = scale;
        EditorUtility.SetDirty(instance);
        return instance;
    }

    private static GameObject GetOrCreateRoot(string name)
    {
        GameObject root = GameObject.Find(name);
        if (root == null)
        {
            root = new GameObject(name);
        }
        return root;
    }

    private static TextMeshProUGUI FindTmp(string objectName)
    {
        GameObject obj = GameObject.Find(objectName);
        return obj != null ? obj.GetComponent<TextMeshProUGUI>() : null;
    }

    private static void ClearChildren(GameObject root)
    {
        for (int i = root.transform.childCount - 1; i >= 0; i--)
        {
            Object.DestroyImmediate(root.transform.GetChild(i).gameObject);
        }
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
