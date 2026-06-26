using System.IO;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class FinalCocukDostuCila
{
    private const string GirisSahnesi = "Assets/Scenes/GirisEkrani.unity";
    private const string SkorSahnesi = "Assets/Scenes/SkorEkrani.unity";
    private const string KoridorSahnesi = "Assets/Scenes/ApartmanKoridoruSahnesi.unity";
    private const string DisSahne = "Assets/Scenes/DisariYolSecimiSahnesi.unity";
    private const string GirisArkaplan = "Assets/Generated/AfetBilinciUI/GirisArkaplan.png";
    private const string SkorArkaplan = "Assets/Generated/AfetBilinciUI/SkorArkaplan.png";
    private const string CharacterPath = "Assets/KidsCharacterFree/Prefabs/Boy0_Humanoid.prefab";
    private const string StreetRoot = "Assets/LowpolyStreetPack/Prefabs/";
    private const string FurnitureRoot = "Assets/ThirdParty/KenneyFurnitureKit/Models/FBX format/";

    [MenuItem("Afet Bilinci/Final Cocuk Dostu Cila")]
    public static void Uygula()
    {
        ImportSprite(GirisArkaplan);
        ImportSprite(SkorArkaplan);
        GirisEkraniniDuzenle();
        SkorEkraniniDuzenle();
        KoridoruDuzenle();
        DisariSahneSifirdanKurulum.Kur();
        AssetDatabase.SaveAssets();
        Debug.Log("Final cocuk dostu cila tamamlandi.");
    }

    public static void BatchUygula()
    {
        Uygula();
        EditorApplication.Exit(0);
    }

    private static void GirisEkraniniDuzenle()
    {
        EditorSceneManager.OpenScene(GirisSahnesi);
        Canvas canvas = FindOrCreateCanvas();
        SetBackground(canvas, GirisArkaplan);

        RectTransform card = CreateUiPanel(
            canvas.transform,
            "GirisKartPaneli",
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0f, 165f),
            new Vector2(720f, 255f),
            new Color(1f, 0.96f, 0.84f, 0.88f));

        TextMeshProUGUI title = GetOrCreateUiText(card, "GirisBaslikYazisi");
        SetupText(title, "Kahraman adını yaz", 34f, new Color(0.18f, 0.17f, 0.16f));
        SetRect(title.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -34f), new Vector2(640f, 56f));

        TextMeshProUGUI welcome = FindTmp("KarsilamaMetni");
        if (welcome != null)
        {
            welcome.transform.SetParent(card, false);
            SetupText(welcome, "Adını yaz, maceraya başlayalım.", 27f, new Color(0.16f, 0.28f, 0.34f));
            SetRect(welcome.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -88f), new Vector2(640f, 48f));
        }

        TMP_InputField input = RebuildNameInput(card);

        Button startButton = FindButton("BaslaButonu");
        if (startButton != null)
        {
            startButton.transform.SetParent(card, false);
            startButton.gameObject.SetActive(false);
            SetRect(startButton.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 42f), new Vector2(360f, 62f));
            StyleButton(startButton, "Başla", new Color(0.18f, 0.59f, 0.37f), Color.white, 30f);
        }

        BringFadeToFront();
        SaveScene();
    }

    private static void SkorEkraniniDuzenle()
    {
        EditorSceneManager.OpenScene(SkorSahnesi);
        Canvas canvas = FindOrCreateCanvas();
        SetBackground(canvas, SkorArkaplan);
        DisableObject("Panel");

        RectTransform card = CreateUiPanel(
            canvas.transform,
            "SkorKartPaneli",
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0f, 240f),
            new Vector2(760f, 420f),
            new Color(1f, 0.96f, 0.84f, 0.9f));

        PositionScoreText("TebrikYazisi_TMP", card, new Vector2(0f, -54f), new Vector2(700f, 105f), 38f, new Color(0.12f, 0.32f, 0.4f));
        PositionScoreText("Isim_TMP", card, new Vector2(0f, -148f), new Vector2(650f, 52f), 28f, new Color(0.18f, 0.17f, 0.16f));
        PositionScoreText("Puan_TMP", card, new Vector2(0f, -208f), new Vector2(650f, 52f), 30f, new Color(0.16f, 0.48f, 0.32f));
        PositionScoreText("Sure_TMP", card, new Vector2(0f, -270f), new Vector2(650f, 52f), 25f, new Color(0.18f, 0.17f, 0.16f));

        Button replay = FindButton("YenidenBaslaButonu");
        if (replay != null)
        {
            replay.transform.SetParent(card, false);
            SetRect(replay.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 42f), new Vector2(360f, 64f));
            StyleButton(replay, "Tekrar Oyna", new Color(0.94f, 0.49f, 0.18f), Color.white, 30f);
        }

        BringFadeToFront();
        SaveScene();
    }

    private static void KoridoruDuzenle()
    {
        EditorSceneManager.OpenScene(KoridorSahnesi);

        Material floor = GetMaterial("Koridor_CocukZemin", new Color(0.36f, 0.58f, 0.66f));
        Material wall = GetMaterial("Koridor_CocukDuvar", new Color(0.70f, 0.86f, 0.90f));
        Material ceiling = GetMaterial("Koridor_CocukTavan", new Color(0.88f, 0.92f, 0.88f));
        Material elevator = GetMaterial("Koridor_AsansorYumusak", new Color(0.26f, 0.31f, 0.35f));
        Material green = GetMaterial("Koridor_YesilYon", new Color(0.30f, 0.72f, 0.42f));
        Material red = GetMaterial("Koridor_KirmiziUyari", new Color(0.92f, 0.36f, 0.25f));
        Material yellow = GetMaterial("Koridor_SariVurgu", new Color(0.98f, 0.76f, 0.25f));

        AssignMaterial("Zemin", floor);
        AssignMaterial("SolDuvar", wall);
        AssignMaterial("SagDuvar", wall);
        AssignMaterial("ArkaDuvar", wall);
        AssignMaterial("Tavan", ceiling);
        AssignMaterial("AsansorCerceve", elevator);
        AssignMaterial("AsansorKapiSol", elevator);
        AssignMaterial("AsansorKapiSag", elevator);

        GameObject root = GetOrCreateRoot("Koridor_CocukDekoru");
        ClearChildren(root);

        CreateNoColliderBox(root, "YesilYol_1", new Vector3(0.9f, 0.075f, 2.4f), new Vector3(0.32f, 0.04f, 2.3f), green);
        CreateNoColliderBox(root, "YesilYol_2", new Vector3(1.75f, 0.08f, 4.55f), new Vector3(1.7f, 0.045f, 0.34f), green);
        CreateArrow(root, "MerdivenOku", new Vector3(1.65f, 0.12f, 3.95f), 35f, green);
        CreateNoColliderBox(root, "AsansorUyariSeridi", new Vector3(-2.25f, 0.1f, 5.05f), new Vector3(1.9f, 0.05f, 0.35f), red);
        CreateNoColliderBox(root, "DuvarRenkSeridiSol", new Vector3(-3.92f, 1.05f, 3.3f), new Vector3(0.03f, 0.22f, 6.2f), yellow);
        CreateNoColliderBox(root, "DuvarRenkSeridiSag", new Vector3(3.92f, 1.05f, 3.3f), new Vector3(0.03f, 0.22f, 6.2f), yellow);

        CreateWorldText(root, "AsansorKisaUyari", "Asansör beklesin", new Vector3(-2.25f, 2.45f, 6.35f), 0f, red.color, 1.25f);
        CreateWorldText(root, "MerdivenKisaUyari", "Merdiven güvenli", new Vector3(2.25f, 2.45f, 6.35f), 0f, green.color, 1.25f);
        CreateWorldText(root, "KoridorPoster", "Sakin ol\nMerdiveni seç", new Vector3(-3.88f, 1.85f, 3.7f), 90f, new Color(0.08f, 0.25f, 0.32f), 0.95f);

        SpawnFitted(FurnitureRoot + "pottedPlant.fbx", root, "KoridorBitkisi", new Vector3(3.35f, 0.05f, 2.2f), Vector3.zero, new Vector3(0.55f, 0.95f, 0.55f));
        SpawnFitted(FurnitureRoot + "benchCushionLow.fbx", root, "KoridorKucukBank", new Vector3(-3.05f, 0.05f, 2.1f), new Vector3(0f, 90f, 0f), new Vector3(1.25f, 0.65f, 0.55f));

        TextMeshProUGUI instruction = FindTmp("TalimatYazisi");
        if (instruction != null)
        {
            SetupText(instruction, "Asansörü kontrol et, merdiveni seç.", 31f, Color.white);
        }

        FixCorridorPlayerAndTriggers();
        SaveScene();
    }

    private static void DisariyiDuzenle()
    {
        EditorSceneManager.OpenScene(DisSahne);

        GameObject root = GetOrCreateRoot("Disari_HikayeDekoru");
        ClearChildren(root);

        Material green = GetMaterial("Disari_YesilYol", new Color(0.24f, 0.68f, 0.33f));
        Material blue = GetMaterial("Disari_ToplanmaMavi", new Color(0.35f, 0.68f, 0.84f));
        Material yellow = GetMaterial("Disari_SariTabela", new Color(0.98f, 0.78f, 0.24f));
        Material red = GetMaterial("Disari_RiskliKirmizi", new Color(0.88f, 0.25f, 0.22f));
        Material building = GetMaterial("Disari_BinaCephe", new Color(0.83f, 0.82f, 0.76f));

        CreateNoColliderBox(root, "ApartmanCephe", new Vector3(0f, 1.35f, -3.8f), new Vector3(5.8f, 2.7f, 0.22f), building);
        CreateNoColliderBox(root, "CikisKapisiGorsel", new Vector3(0f, 0.95f, -3.62f), new Vector3(1.15f, 1.9f, 0.12f), GetMaterial("Disari_Kapi", new Color(0.44f, 0.23f, 0.16f)));
        CreateNoColliderBox(root, "GuvenliYol_Baslangic", new Vector3(-0.45f, 0.055f, -0.6f), new Vector3(1.35f, 0.04f, 3.8f), green);
        CreateNoColliderBox(root, "GuvenliYol_Donus", new Vector3(-2.15f, 0.06f, 2.0f), new Vector3(3.4f, 0.04f, 1.35f), green);
        CreateNoColliderBox(root, "GuvenliYol_Park", new Vector3(-4.05f, 0.06f, 4.25f), new Vector3(1.55f, 0.04f, 3.2f), green);
        CreateNoColliderBox(root, "ToplanmaAlaniZemini", new Vector3(-4.05f, 0.07f, 6.1f), new Vector3(4.1f, 0.05f, 2.45f), blue);
        CreateNoColliderBox(root, "RiskliBolgeSeridi", new Vector3(2.55f, 0.07f, 0.85f), new Vector3(1.9f, 0.05f, 3.4f), red);
        CreateArrow(root, "DisariYonOku1", new Vector3(-0.45f, 0.14f, 0.25f), 0f, green);
        CreateArrow(root, "DisariYonOku2", new Vector3(-1.65f, 0.14f, 2.0f), -90f, green);
        CreateArrow(root, "DisariYonOku3", new Vector3(-4.05f, 0.14f, 4.1f), 0f, green);

        CreateWorldText(root, "CikisBilgi", "Binadan çıktın", new Vector3(0f, 2.95f, -3.45f), 0f, new Color(0.08f, 0.25f, 0.32f), 1.2f);
        CreateFloatingWorldText(root, "GuvenliAlanAsiliYazi", "GÜVENLİ\nALAN", new Vector3(-4.25f, 3.9f, 6.15f), new Vector3(0f, 20f, 0f), green.color, 1.55f);
        CreateWorldText(root, "GuvenliYolTabela1", "OKLARI İZLE", new Vector3(-1.35f, 1.45f, 1.45f), -35f, green.color, 1.0f);
        CreateWorldText(root, "GuvenliYolTabela2", "TOPLANMA ALANI", new Vector3(-4.8f, 1.75f, 4.6f), 35f, yellow.color, 1.15f);
        CreateWorldText(root, "ToplanmaTabela", "BURASI\nGÜVENLİ", new Vector3(-4.05f, 1.9f, 6.35f), 0f, yellow.color, 1.25f);
        CreateWorldText(root, "RiskUyari", "YÜKSEK BİNA\nVE DİREKLER", new Vector3(2.6f, 1.55f, 0.9f), -20f, red.color, 1.0f);

        Spawn("Assets/LowpolyStreetPack/Prefabs/StreetProps/RoadBlocks/RoadCone_A.prefab", root, "RiskKoni1", new Vector3(1.55f, 0f, -0.55f), Vector3.zero, Vector3.one);
        Spawn("Assets/LowpolyStreetPack/Prefabs/StreetProps/RoadBlocks/RoadCone_A.prefab", root, "RiskKoni2", new Vector3(3.15f, 0f, 1.35f), Vector3.zero, Vector3.one);
        Spawn("Assets/LowpolyStreetPack/Prefabs/StreetProps/RoadBlocks/RoadCone_A.prefab", root, "RiskKoni3", new Vector3(1.75f, 0f, 2.7f), Vector3.zero, Vector3.one);
        Spawn("Assets/LowpolyStreetPack/Prefabs/StreetProps/Bench/Bench_A.prefab", root, "ToplanmaBankiYeni", new Vector3(-2.65f, 0f, 6.35f), new Vector3(0f, -25f, 0f), Vector3.one);
        Spawn("Assets/LowpolyStreetPack/Prefabs/Foliage/Trees/Tree_A_V01.prefab", root, "ToplanmaAgaci", new Vector3(-5.9f, 0f, 5.65f), Vector3.zero, Vector3.one);
        Spawn("Assets/LowpolyStreetPack/Prefabs/Foliage/Trees_Big/Bush_A.prefab", root, "ToplanmaCali1", new Vector3(-5.75f, 0f, 6.9f), Vector3.zero, Vector3.one);
        Spawn("Assets/LowpolyStreetPack/Prefabs/Foliage/Trees_Big/Bush_D.prefab", root, "ToplanmaCali2", new Vector3(-2.25f, 0f, 5.1f), Vector3.zero, Vector3.one);
        Spawn("Assets/LowpolyStreetPack/Prefabs/StreetProps/ParkLamp/ParkLamp.prefab", root, "ToplanmaLambasi", new Vector3(-1.4f, 0f, 3.15f), Vector3.zero, Vector3.one);

        FixOutsidePlayer();
        FixStreetChoice("Yol_Guvenli", new Vector3(-4.05f, 1f, 6.15f), new Vector3(4.1f, 2f, 2.35f));
        FixStreetChoice("Yol_Tehlikeli", new Vector3(2.55f, 1f, 0.85f), new Vector3(1.9f, 2f, 3.4f));

        TextMeshProUGUI status = FindTmp("YolDurumYazisi");
        if (status != null)
        {
            SetupText(status, "Sol taraftaki güvenli alana ilerle.", 30f, Color.white);
        }

        SaveScene();
    }

    private static void FixCorridorPlayerAndTriggers()
    {
        GameObject player = GameObject.Find("Oyuncu");
        if (player != null)
        {
            player.transform.position = new Vector3(0f, 0.22f, 1.0f);
            Camera camera = player.GetComponentInChildren<Camera>(true);
            if (camera != null)
            {
                camera.transform.localPosition = new Vector3(0f, 1.5f, -3.35f);
                camera.transform.localEulerAngles = new Vector3(9f, 0f, 0f);
                camera.fieldOfView = 65f;
            }
        }

        FixTahliyeTrigger("Asansor", new Vector3(-2.25f, 1f, 5.25f), new Vector3(1.6f, 1.8f, 0.9f));
        FixTahliyeTrigger("Merdiven", new Vector3(2.25f, 1f, 5.0f), new Vector3(1.8f, 1.8f, 1.25f));
    }

    private static void FixOutsidePlayer()
    {
        GameObject player = GameObject.Find("Oyuncu");
        if (player == null)
        {
            player = new GameObject("Oyuncu");
        }

        player.tag = "Player";
        player.transform.position = new Vector3(0f, 0.2f, -2.55f);
        player.transform.eulerAngles = Vector3.zero;

        OyuncuKontrol control = player.GetComponent<OyuncuKontrol>();
        if (control == null)
        {
            control = player.AddComponent<OyuncuKontrol>();
        }
        control.hareketHizi = 3.2f;
        control.donusHizi = 150f;

        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = player.AddComponent<CharacterController>();
        }
        controller.center = new Vector3(0f, 0.68f, 0f);
        controller.height = 1.35f;
        controller.radius = 0.28f;
        controller.stepOffset = 0.22f;

        MeshRenderer robotRenderer = player.GetComponent<MeshRenderer>();
        if (robotRenderer != null)
        {
            robotRenderer.enabled = false;
        }

        for (int i = player.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = player.transform.GetChild(i);
            if (child.GetComponent<Camera>() == null)
            {
                Object.DestroyImmediate(child.gameObject);
            }
        }

        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(CharacterPath);
        if (prefab != null)
        {
            GameObject character = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            character.name = "CocukKarakter";
            character.transform.SetParent(player.transform, false);
            character.transform.localPosition = Vector3.zero;
            character.transform.localRotation = Quaternion.identity;
            FitHeight(character, 1.35f);

            Animator animator = character.GetComponent<Animator>();
            if (animator != null)
            {
                animator.applyRootMotion = false;
            }
            RemoveCharacterDemoComponents(character);
        }

        Camera camera = player.GetComponentInChildren<Camera>(true);
        if (camera == null)
        {
            GameObject cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.SetParent(player.transform, false);
            camera = cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();
        }

        camera.transform.localPosition = new Vector3(0f, 2.35f, -4.15f);
        camera.transform.localEulerAngles = new Vector3(12f, 0f, 0f);
        camera.fieldOfView = 66f;
        camera.nearClipPlane = 0.08f;
    }

    private static void FixTahliyeTrigger(string name, Vector3 position, Vector3 size)
    {
        GameObject obj = GameObject.Find(name);
        if (obj == null)
        {
            return;
        }

        obj.transform.position = position;
        foreach (Collider existing in obj.GetComponents<Collider>())
        {
            Object.DestroyImmediate(existing);
        }

        BoxCollider collider = obj.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.center = Vector3.zero;
        collider.size = size;
    }

    private static void FixStreetChoice(string name, Vector3 position, Vector3 size)
    {
        GameObject obj = GameObject.Find(name);
        if (obj == null)
        {
            return;
        }

        obj.transform.position = position;
        BoxCollider collider = obj.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = obj.AddComponent<BoxCollider>();
        }
        collider.isTrigger = true;
        collider.center = Vector3.zero;
        collider.size = size;

        TextMeshProUGUI status = FindTmp("YolDurumYazisi");
        SokakSecimi choice = obj.GetComponent<SokakSecimi>();
        if (choice == null)
        {
            choice = obj.AddComponent<SokakSecimi>();
        }
        choice.durumYazisi = status;
    }

    private static void FitHeight(GameObject target, float targetHeight)
    {
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>(true);
        if (renderers.Length == 0)
        {
            return;
        }

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        target.transform.localScale = Vector3.one *
                                      (targetHeight / Mathf.Max(bounds.size.y, 0.001f));

        renderers = target.GetComponentsInChildren<Renderer>(true);
        bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        target.transform.position += target.transform.parent.position -
                                     new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
    }

    private static void RemoveCharacterDemoComponents(GameObject character)
    {
        foreach (MonoBehaviour behaviour in character.GetComponentsInChildren<MonoBehaviour>(true))
        {
            if (behaviour != null && behaviour.GetType().FullName == "Sample.KidsScript")
            {
                Object.DestroyImmediate(behaviour);
            }
        }

        foreach (CharacterController controller in character.GetComponentsInChildren<CharacterController>(true))
        {
            Object.DestroyImmediate(controller);
        }
    }

    private static void PositionScoreText(string name, RectTransform card, Vector2 position, Vector2 size, float fontSize, Color color)
    {
        TextMeshProUGUI text = FindTmp(name);
        if (text == null)
        {
            return;
        }

        text.transform.SetParent(card, false);
        SetupText(text, text.text, fontSize, color);
        SetRect(text.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), position, size);
    }

    private static TMP_InputField RebuildNameInput(RectTransform card)
    {
        foreach (TMP_InputField existing in Object.FindObjectsByType<TMP_InputField>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            Object.DestroyImmediate(existing.gameObject);
        }

        GameObject inputObject = new GameObject("IsimInputField", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(TMP_InputField));
        inputObject.transform.SetParent(card, false);
        RectTransform inputRect = inputObject.GetComponent<RectTransform>();
        SetRect(inputRect, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, -8f), new Vector2(520f, 62f));

        Image inputImage = inputObject.GetComponent<Image>();
        inputImage.color = Color.white;

        GameObject viewportObject = new GameObject("Text Area", typeof(RectTransform), typeof(RectMask2D));
        viewportObject.transform.SetParent(inputObject.transform, false);
        RectTransform viewportRect = viewportObject.GetComponent<RectTransform>();
        SetRect(viewportRect, Vector2.zero, Vector2.one, Vector2.zero, new Vector2(-28f, -16f));

        TextMeshProUGUI placeholder = CreateInputText(viewportObject.transform, "Placeholder", "Adını buraya yaz", new Color(0.36f, 0.42f, 0.45f, 0.72f));
        TextMeshProUGUI text = CreateInputText(viewportObject.transform, "Text", "", new Color(0.1f, 0.15f, 0.18f));
        text.enableAutoSizing = false;
        text.fontSize = 27f;

        TMP_InputField input = inputObject.GetComponent<TMP_InputField>();
        input.textViewport = viewportRect;
        input.textComponent = text;
        input.placeholder = placeholder;
        input.characterLimit = 18;
        input.lineType = TMP_InputField.LineType.SingleLine;
        input.SetTextWithoutNotify("");

        GirisKontrol control = Object.FindFirstObjectByType<GirisKontrol>(FindObjectsInactive.Include);
        if (control != null)
        {
            SerializedObject serialized = new SerializedObject(control);
            serialized.FindProperty("isimGiris").objectReferenceValue = inputObject;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            UnityEventTools.AddVoidPersistentListener(input.onValueChanged, control.IsimKontrolEt);
            EditorUtility.SetDirty(control);
        }

        EditorUtility.SetDirty(inputObject);
        return input;
    }

    private static TextMeshProUGUI CreateInputText(Transform parent, string name, string value, Color color)
    {
        GameObject textObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        textObject.transform.SetParent(parent, false);
        RectTransform rect = textObject.GetComponent<RectTransform>();
        SetRect(rect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
        text.text = value;
        text.fontSize = 25f;
        text.color = color;
        text.alignment = TextAlignmentOptions.MidlineLeft;
        text.margin = new Vector4(12f, 0f, 12f, 0f);
        text.raycastTarget = false;
        return text;
    }

    private static void SetBackground(Canvas canvas, string path)
    {
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        RectTransform background = CreateUiPanel(
            canvas.transform,
            "TemaArkaplani",
            Vector2.zero,
            Vector2.one,
            Vector2.zero,
            Vector2.zero,
            Color.white);
        Image image = background.GetComponent<Image>();
        image.sprite = sprite;
        image.preserveAspect = false;
        image.raycastTarget = false;
        background.SetAsFirstSibling();
    }

    private static RectTransform CreateUiPanel(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size, Color color)
    {
        Transform existing = parent.Find(name);
        GameObject obj = existing != null
            ? existing.gameObject
            : new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        obj.transform.SetParent(parent, false);
        RectTransform rect = obj.GetComponent<RectTransform>();
        SetRect(rect, anchorMin, anchorMax, position, size);
        Image image = obj.GetComponent<Image>();
        image.color = color;
        image.raycastTarget = false;
        EditorUtility.SetDirty(obj);
        return rect;
    }

    private static void SetRect(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size)
    {
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        EditorUtility.SetDirty(rect);
    }

    private static void SetupText(TextMeshProUGUI text, string value, float fontSize, Color color)
    {
        text.text = value;
        text.fontSize = fontSize;
        text.enableAutoSizing = true;
        text.fontSizeMin = Mathf.Max(18f, fontSize - 10f);
        text.fontSizeMax = fontSize;
        text.fontStyle = FontStyles.Bold;
        text.alignment = TextAlignmentOptions.Center;
        text.color = color;
        text.raycastTarget = false;
        EditorUtility.SetDirty(text);
    }

    private static void StyleButton(Button button, string label, Color buttonColor, Color textColor, float fontSize)
    {
        Image image = button.GetComponent<Image>();
        if (image != null)
        {
            image.color = buttonColor;
        }

        TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>(true);
        if (text != null)
        {
            SetupText(text, label, fontSize, textColor);
        }
        EditorUtility.SetDirty(button);
    }

    private static void CreateArrow(GameObject parent, string name, Vector3 position, float yRotation, Material material)
    {
        GameObject arrow = new GameObject(name);
        arrow.transform.SetParent(parent.transform);
        arrow.transform.position = position;
        arrow.transform.eulerAngles = new Vector3(0f, yRotation, 0f);
        CreateNoColliderBox(arrow, "Govde", Vector3.zero, new Vector3(0.26f, 0.03f, 0.85f), material);
        CreateNoColliderBox(arrow, "BasSol", new Vector3(-0.18f, 0f, 0.38f), new Vector3(0.26f, 0.03f, 0.48f), material).transform.localEulerAngles = new Vector3(0f, -35f, 0f);
        CreateNoColliderBox(arrow, "BasSag", new Vector3(0.18f, 0f, 0.38f), new Vector3(0.26f, 0.03f, 0.48f), material).transform.localEulerAngles = new Vector3(0f, 35f, 0f);
    }

    private static GameObject CreateNoColliderBox(GameObject parent, string name, Vector3 position, Vector3 scale, Material material)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = name;
        box.transform.SetParent(parent.transform, false);
        box.transform.localPosition = position;
        box.transform.localScale = scale;
        Renderer renderer = box.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial = material;
        }
        Collider collider = box.GetComponent<Collider>();
        if (collider != null)
        {
            Object.DestroyImmediate(collider);
        }
        return box;
    }

    private static void CreateWorldText(GameObject parent, string name, string value, Vector3 position, float yRotation, Color color, float fontSize)
    {
        GameObject obj = new GameObject(name, typeof(TextMeshPro));
        obj.transform.SetParent(parent.transform);
        obj.transform.position = position;
        obj.transform.eulerAngles = new Vector3(0f, yRotation, 0f);
        TextMeshPro text = obj.GetComponent<TextMeshPro>();
        text.text = value;
        text.fontSize = fontSize;
        text.fontStyle = FontStyles.Bold;
        text.alignment = TextAlignmentOptions.Center;
        text.color = color;
        text.rectTransform.sizeDelta = new Vector2(3.4f, 1.2f);
    }

    private static void CreateFloatingWorldText(GameObject parent, string name, string value, Vector3 position, Vector3 rotation, Color color, float fontSize)
    {
        GameObject obj = new GameObject(name, typeof(TextMeshPro), typeof(YukariAsagiHareket));
        obj.transform.SetParent(parent.transform);
        obj.transform.position = position;
        obj.transform.eulerAngles = rotation;
        TextMeshPro text = obj.GetComponent<TextMeshPro>();
        text.text = value;
        text.fontSize = fontSize;
        text.fontStyle = FontStyles.Bold;
        text.alignment = TextAlignmentOptions.Center;
        text.color = color;
        text.rectTransform.sizeDelta = new Vector2(5f, 2f);

        YukariAsagiHareket movement = obj.GetComponent<YukariAsagiHareket>();
        movement.mesafe = 0.28f;
        movement.hiz = 1.4f;
        movement.kamerayaBak = false;
    }

    private static TextMeshProUGUI GetOrCreateUiText(RectTransform parent, string name)
    {
        Transform existing = parent.Find(name);
        GameObject obj = existing != null
            ? existing.gameObject
            : new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        obj.transform.SetParent(parent, false);
        return obj.GetComponent<TextMeshProUGUI>();
    }

    private static GameObject Spawn(string path, GameObject parent, string name, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (prefab == null)
        {
            return null;
        }

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instance.name = name;
        instance.transform.SetParent(parent.transform);
        instance.transform.position = position;
        instance.transform.eulerAngles = rotation;
        instance.transform.localScale = scale;
        foreach (Collider collider in instance.GetComponentsInChildren<Collider>(true))
        {
            collider.enabled = false;
        }
        return instance;
    }

    private static GameObject SpawnFitted(string path, GameObject parent, string name, Vector3 floorPosition, Vector3 rotation, Vector3 targetSize)
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

        float scale = Mathf.Min(
            targetSize.x / Mathf.Max(bounds.size.x, 0.001f),
            targetSize.y / Mathf.Max(bounds.size.y, 0.001f),
            targetSize.z / Mathf.Max(bounds.size.z, 0.001f));
        instance.transform.localScale = Vector3.one * scale;

        renderers = instance.GetComponentsInChildren<Renderer>(true);
        bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        instance.transform.position += floorPosition - new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
        return instance;
    }

    private static void ImportSprite(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning("Gorsel bulunamadi: " + path);
            return;
        }

        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer == null)
        {
            return;
        }

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.mipmapEnabled = false;
        importer.maxTextureSize = 2048;
        importer.SaveAndReimport();
    }

    private static Canvas FindOrCreateCanvas()
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>(FindObjectsInactive.Include);
        if (canvas != null)
        {
            CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
            if (scaler != null)
            {
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920f, 1080f);
            }
            return canvas;
        }

        GameObject obj = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvas = obj.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler newScaler = obj.GetComponent<CanvasScaler>();
        newScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        newScaler.referenceResolution = new Vector2(1920f, 1080f);
        return canvas;
    }

    private static TextMeshProUGUI FindTmp(string name)
    {
        GameObject obj = GameObject.Find(name);
        return obj != null ? obj.GetComponent<TextMeshProUGUI>() : null;
    }

    private static Button FindButton(string name)
    {
        GameObject obj = GameObject.Find(name);
        return obj != null ? obj.GetComponent<Button>() : null;
    }

    private static void DisableObject(string name)
    {
        GameObject obj = GameObject.Find(name);
        if (obj != null)
        {
            obj.SetActive(false);
            EditorUtility.SetDirty(obj);
        }
    }

    private static GameObject GetOrCreateRoot(string name)
    {
        GameObject root = GameObject.Find(name);
        return root != null ? root : new GameObject(name);
    }

    private static void ClearChildren(GameObject root)
    {
        for (int i = root.transform.childCount - 1; i >= 0; i--)
        {
            Object.DestroyImmediate(root.transform.GetChild(i).gameObject);
        }
    }

    private static Material GetMaterial(string name, Color color)
    {
        const string folder = "Assets/Generated/AfetBilinciMaterials";
        if (!AssetDatabase.IsValidFolder("Assets/Generated"))
        {
            AssetDatabase.CreateFolder("Assets", "Generated");
        }
        if (!AssetDatabase.IsValidFolder(folder))
        {
            AssetDatabase.CreateFolder("Assets/Generated", "AfetBilinciMaterials");
        }

        string path = folder + "/" + name + ".mat";
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

    private static void AssignMaterial(string objectName, Material material)
    {
        GameObject obj = GameObject.Find(objectName);
        if (obj == null)
        {
            return;
        }

        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial = material;
        }
    }

    private static void BringFadeToFront()
    {
        GameObject fade = GameObject.Find("SiyahEkran");
        if (fade != null)
        {
            fade.transform.SetAsLastSibling();
        }
    }

    private static void SaveScene()
    {
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
    }
}
