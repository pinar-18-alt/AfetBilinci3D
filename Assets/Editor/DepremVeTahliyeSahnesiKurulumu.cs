using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class DepremVeTahliyeSahnesiKurulumu
{
    private const string KaynakSahne = "Assets/Scenes/DepremCantasiSahnesi.unity";
    private const string HedefSahne = "Assets/Scenes/SampleScene.unity";
    private const string YedekSahne = "Assets/Scenes/Backups/SampleScene_Eski.unity";
    private const string AutoKey = "AfetBilinci.DepremTahliyeKurulum.v1";

    [InitializeOnLoadMethod]
    private static void OtomatikKur()
    {
        EditorApplication.delayCall += () =>
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode ||
                EditorPrefs.GetBool(AutoKey, false))
            {
                return;
            }

            Kur();
            EditorPrefs.SetBool(AutoKey, true);
        };
    }

    [MenuItem("Afet Bilinci/Deprem ve Tahliye Sahnesini Kur")]
    public static void Kur()
    {
        YedekAl();

        Scene scene = EditorSceneManager.OpenScene(KaynakSahne, OpenSceneMode.Single);
        Temizle("CantaYonetici");
        Temizle("Kup_Su");
        Temizle("Kup_Fener");
        Temizle("Kup_Laptop");
        Temizle("Kup_Oyuncak");
        Temizle("TahliyeyeBaslaButonu");

        GameObject player = GameObject.Find("Oyuncu");
        Camera camera = player != null ? player.GetComponentInChildren<Camera>(true) : null;
        if (player != null)
        {
            player.transform.position = new Vector3(2.1f, 0.22f, -3.4f);
            player.transform.eulerAngles = new Vector3(0f, -90f, 0f);
        }

        YumusakKameraSarsintisi shake = null;
        if (camera != null)
        {
            shake = camera.GetComponent<YumusakKameraSarsintisi>();
            if (shake == null)
            {
                shake = camera.gameObject.AddComponent<YumusakKameraSarsintisi>();
            }
            shake.sarsintiAktif = true;
            shake.siddet = 0.045f;
            shake.hiz = 13f;
        }

        TextMeshProUGUI status = FindTmp("CantaDurumYazisi");
        if (status != null)
        {
            status.gameObject.name = "TalimatYazisi";
            status.text = "Sarsıntı başladı. Güvenli masaya yaklaş.";
        }

        GameObject evacuationRoot = new GameObject("TahliyeSecenekleri");
        GameObject continueButton = CreateContinueButton(status != null ? status.transform.parent : null);
        CreateEvacuationZone(
            evacuationRoot.transform,
            "Asansor",
            new Vector3(2.35f, 1f, 7.25f),
            new Color(0.82f, 0.32f, 0.27f),
            "ASANSÖR",
            status,
            continueButton);
        CreateEvacuationZone(
            evacuationRoot.transform,
            "Merdiven",
            new Vector3(4.25f, 1f, 3.45f),
            new Color(0.2f, 0.65f, 0.38f),
            "MERDİVEN",
            status,
            continueButton);

        GameObject taskArea = new GameObject("CokKapanTutunAlani");
        taskArea.transform.position = new Vector3(-6.73f, 0.65f, -0.18f);
        BoxCollider trigger = taskArea.AddComponent<BoxCollider>();
        trigger.isTrigger = true;
        trigger.size = new Vector3(2.7f, 1.4f, 3.1f);

        CokKapanTutunGorevi task = taskArea.AddComponent<CokKapanTutunGorevi>();
        task.yonergeYazisi = status;
        task.bilgiYazisi = status;
        task.gerekliSure = 3f;
        task.tamamlanincaAktifOlacak = evacuationRoot;
        task.kameraSarsintisi = shake;

        AddStoryPanel(status != null ? status.transform.parent : null);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, HedefSahne, true);
        AssetDatabase.SaveAssets();
        Debug.Log("Deprem ve tahliye sahnesi stylized ev icinde kuruldu.");
    }

    private static void YedekAl()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Scenes/Backups"))
        {
            AssetDatabase.CreateFolder("Assets/Scenes", "Backups");
        }

        if (AssetDatabase.LoadAssetAtPath<SceneAsset>(YedekSahne) == null)
        {
            AssetDatabase.CopyAsset(HedefSahne, YedekSahne);
        }
    }

    private static void Temizle(string name)
    {
        GameObject obj = GameObject.Find(name);
        if (obj != null)
        {
            Object.DestroyImmediate(obj);
        }
    }

    private static void CreateEvacuationZone(
        Transform parent,
        string name,
        Vector3 position,
        Color color,
        string label,
        TextMeshProUGUI status,
        GameObject continueButton)
    {
        GameObject zone = GameObject.CreatePrimitive(PrimitiveType.Cube);
        zone.name = name;
        zone.transform.SetParent(parent);
        zone.transform.position = position;
        zone.transform.localScale = new Vector3(1.25f, 2f, 0.18f);

        Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        material.color = color;
        zone.GetComponent<Renderer>().sharedMaterial = material;

        BoxCollider collider = zone.GetComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(2.1f, 1.2f, 5f);

        TahliyeKontrol control = zone.AddComponent<TahliyeKontrol>();
        control.uiMetni = status;
        control.devamButonu = continueButton;

        GameObject textObject = new GameObject("Etiket", typeof(TextMeshPro));
        textObject.transform.SetParent(zone.transform, false);
        textObject.transform.localPosition = new Vector3(0f, 0f, -0.65f);
        textObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        textObject.transform.localScale = new Vector3(0.15f, 0.1f, 0.1f);
        TextMeshPro text = textObject.GetComponent<TextMeshPro>();
        text.text = label;
        text.fontSize = 5f;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
    }

    private static GameObject CreateContinueButton(Transform canvas)
    {
        if (canvas == null)
        {
            return null;
        }

        GameObject buttonObject = new GameObject(
            "DisariyaDevamButonu",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image),
            typeof(Button),
            typeof(GuvenliSahneGecisi));
        buttonObject.transform.SetParent(canvas, false);

        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.anchorMax = new Vector2(0.5f, 0f);
        rect.anchoredPosition = new Vector2(0f, 70f);
        rect.sizeDelta = new Vector2(390f, 74f);

        Image image = buttonObject.GetComponent<Image>();
        image.color = new Color(0.18f, 0.62f, 0.35f);

        GameObject textObject = new GameObject(
            "Text (TMP)",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(TextMeshProUGUI));
        textObject.transform.SetParent(buttonObject.transform, false);
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
        text.text = "Dışarıya çık";
        text.fontSize = 30f;
        text.fontStyle = FontStyles.Bold;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;

        Button button = buttonObject.GetComponent<Button>();
        GuvenliSahneGecisi transition = buttonObject.GetComponent<GuvenliSahneGecisi>();
        transition.hedefSahneAdi = "DisariYolSecimiSahnesi";
        UnityEventTools.AddPersistentListener(button.onClick, transition.SahneyeGec);
        buttonObject.SetActive(false);
        return buttonObject;
    }

    private static void AddStoryPanel(Transform canvas)
    {
        if (canvas == null)
        {
            return;
        }

        GameObject panel = new GameObject(
            "DepremBaslangicPaneli",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image));
        panel.transform.SetParent(canvas, false);
        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(680f, 220f);
        panel.GetComponent<Image>().color = new Color(0.06f, 0.16f, 0.22f, 0.94f);

        GameObject storyObject = new GameObject(
            "HikayeMetni",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(TextMeshProUGUI));
        storyObject.transform.SetParent(panel.transform, false);
        RectTransform storyRect = storyObject.GetComponent<RectTransform>();
        storyRect.anchorMin = Vector2.zero;
        storyRect.anchorMax = Vector2.one;
        storyRect.offsetMin = new Vector2(35f, 25f);
        storyRect.offsetMax = new Vector2(-35f, -25f);
        TextMeshProUGUI story = storyObject.GetComponent<TextMeshProUGUI>();
        story.text = "Sarsıntı başladı!\nSakin ol ve güvenli masaya yaklaş.\n\nE ile devam et";
        story.fontSize = 30f;
        story.alignment = TextAlignmentOptions.Center;
        story.color = Color.white;

        HikayePaneli storyController = panel.AddComponent<HikayePaneli>();
        storyController.aciklamaYazisi = story;
        storyController.panel = panel;
        storyController.baslik = "";
        storyController.aciklama =
            "Sarsıntı başladı!\nSakin ol ve güvenli masaya yaklaş.\n\nE ile devam et";
        storyController.otomatikKapanmaSuresi = 4f;
    }

    private static TextMeshProUGUI FindTmp(string name)
    {
        GameObject obj = GameObject.Find(name);
        return obj != null ? obj.GetComponent<TextMeshProUGUI>() : null;
    }
}
