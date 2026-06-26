using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class ApartmanKoridoruSahnesiKurulumu
{
    private const string EvSahnesi = "Assets/Scenes/SampleScene.unity";
    private const string KoridorSahnesi = "Assets/Scenes/ApartmanKoridoruSahnesi.unity";
    private const string CharacterPath =
        "Assets/KidsCharacterFree/Prefabs/Boy0_Humanoid.prefab";
    private const string StairsPath =
        "Assets/ThirdParty/KenneyFurnitureKit/Models/FBX format/stairsOpen.fbx";
    private const string DoorwayPath =
        "Assets/ThirdParty/KenneyFurnitureKit/Models/FBX format/doorwayOpen.fbx";

    [MenuItem("Afet Bilinci/Ev Cikisi ve Apartman Koridorunu Kur")]
    public static void Kur()
    {
        EvCikisiniKur();
        KoridoruKur();
        BuildSettingsGuncelle();
        AssetDatabase.SaveAssets();
        Debug.Log("Ev cikisi ve apartman koridoru sahnesi kuruldu.");
    }

    private static void EvCikisiniKur()
    {
        EditorSceneManager.OpenScene(EvSahnesi);
        DestroyNamed("ApartmanKoridoru");
        DestroyNamed("TahliyeSecenekleri");
        DestroyNamed("EvCikisTetikleyici");

        foreach (Transform transform in Object.FindObjectsByType<Transform>(
                     FindObjectsInactive.Include,
                     FindObjectsSortMode.None))
        {
            if (transform.name.Contains("Door_Proxy_208"))
            {
                transform.gameObject.SetActive(true);
                EditorUtility.SetDirty(transform.gameObject);
            }
        }

        GameObject exitTrigger = new GameObject("EvCikisTetikleyici");
        exitTrigger.transform.position = new Vector3(4.05f, 1f, 3.3f);
        BoxCollider collider = exitTrigger.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(1.6f, 2f, 1.4f);
        KapidanSahneGecisi transition = exitTrigger.AddComponent<KapidanSahneGecisi>();
        transition.hedefSahneAdi = "ApartmanKoridoruSahnesi";
        exitTrigger.SetActive(false);

        CokKapanTutunGorevi task =
            Object.FindFirstObjectByType<CokKapanTutunGorevi>();
        if (task != null)
        {
            task.tamamlanincaAktifOlacak = exitTrigger;
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
    }

    private static void KoridoruKur()
    {
        Scene scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);
        scene.name = "ApartmanKoridoruSahnesi";

        Material floor = GetMaterial("Apartman_Zemin", new Color(0.24f, 0.27f, 0.29f));
        Material wall = GetMaterial("Apartman_Duvar", new Color(0.72f, 0.75f, 0.72f));
        Material elevator = GetMaterial("Apartman_Asansor", new Color(0.32f, 0.36f, 0.39f));

        GameObject environment = new GameObject("ApartmanKoridoru");
        CreateBox(environment, "Zemin", new Vector3(0f, 0f, 3.5f), new Vector3(8f, 0.2f, 7f), floor);
        CreateBox(environment, "Tavan", new Vector3(0f, 3f, 3.5f), new Vector3(8f, 0.15f, 7f), wall);
        CreateBox(environment, "SolDuvar", new Vector3(-4f, 1.5f, 3.5f), new Vector3(0.15f, 3f, 7f), wall);
        CreateBox(environment, "SagDuvar", new Vector3(4f, 1.5f, 3.5f), new Vector3(0.15f, 3f, 7f), wall);
        CreateBox(environment, "ArkaDuvar", new Vector3(0f, 1.5f, 7f), new Vector3(8f, 3f, 0.15f), wall);
        CreateElevator(environment, elevator);
        CreateStairs(environment);
        CreatePlayer();
        TextMeshProUGUI status = CreateUi();
        BindChoiceTriggers(status);
        CreateLight();

        EditorSceneManager.SaveScene(scene, KoridorSahnesi);
    }

    private static void CreateElevator(GameObject parent, Material material)
    {
        CreateBox(parent, "AsansorCerceve", new Vector3(-2.25f, 1.45f, 6.82f), new Vector3(2.5f, 2.8f, 0.22f), material);
        CreateBox(parent, "AsansorKapiSol", new Vector3(-2.85f, 1.35f, 6.65f), new Vector3(1.12f, 2.55f, 0.14f), material);
        CreateBox(parent, "AsansorKapiSag", new Vector3(-1.65f, 1.35f, 6.65f), new Vector3(1.12f, 2.55f, 0.14f), material);
        CreateWorldLabel(parent, "ASANSÖR", new Vector3(-2.25f, 2.75f, 6.45f), Vector3.zero);
    }

    private static void CreateStairs(GameObject parent)
    {
        SpawnFitted(
            DoorwayPath,
            parent,
            "MerdivenKapisi",
            new Vector3(2.2f, 0.1f, 6.72f),
            Vector3.zero,
            new Vector3(2.5f, 2.85f, 0.45f));
        SpawnFitted(
            StairsPath,
            parent,
            "MerdivenModeli",
            new Vector3(2.2f, 0.12f, 5.65f),
            new Vector3(0f, 180f, 0f),
            new Vector3(2.8f, 2.25f, 2.6f));
        CreateWorldLabel(parent, "MERDİVEN", new Vector3(2.2f, 2.75f, 6.4f), Vector3.zero);
    }

    private static void CreatePlayer()
    {
        GameObject player = new GameObject("Oyuncu");
        player.tag = "Player";
        player.transform.position = new Vector3(0f, 0.2f, 0.8f);
        player.AddComponent<OyuncuKontrol>();

        CharacterController controller = player.AddComponent<CharacterController>();
        controller.center = new Vector3(0f, 0.68f, 0f);
        controller.height = 1.35f;
        controller.radius = 0.28f;
        controller.stepOffset = 0.22f;

        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(CharacterPath);
        GameObject character = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        character.name = "CocukKarakter";
        character.transform.SetParent(player.transform, false);
        character.transform.localRotation = Quaternion.identity;
        FitHeight(character, 1.35f);
        Animator animator = character.GetComponent<Animator>();
        if (animator != null)
        {
            animator.applyRootMotion = false;
        }
        RemoveCharacterDemoComponents(character);

        GameObject cameraObject = new GameObject("Main Camera");
        cameraObject.tag = "MainCamera";
        cameraObject.transform.SetParent(player.transform);
        cameraObject.transform.localPosition = new Vector3(0f, 1.5f, -3.35f);
        cameraObject.transform.localEulerAngles = new Vector3(9f, 0f, 0f);
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.fieldOfView = 65f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.45f, 0.62f, 0.7f);
        cameraObject.AddComponent<AudioListener>();
    }

    private static TextMeshProUGUI CreateUi()
    {
        GameObject canvasObject = new GameObject(
            "Canvas",
            typeof(Canvas),
            typeof(CanvasScaler),
            typeof(GraphicRaycaster));
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);

        GameObject panel = new GameObject(
            "TalimatPaneli",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image));
        panel.transform.SetParent(canvasObject.transform, false);
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 1f);
        panelRect.anchorMax = new Vector2(0.5f, 1f);
        panelRect.anchoredPosition = new Vector2(0f, -70f);
        panelRect.sizeDelta = new Vector2(900f, 92f);
        panel.GetComponent<Image>().color = new Color(0.05f, 0.16f, 0.22f, 0.9f);

        GameObject textObject = new GameObject(
            "TalimatYazisi",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(TextMeshProUGUI));
        textObject.transform.SetParent(panel.transform, false);
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
        text.text = "Merdiveni veya asansörü kontrol et.";
        text.fontSize = 32f;
        text.fontStyle = FontStyles.Bold;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;

        new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        return text;
    }

    private static void BindChoiceTriggers(TextMeshProUGUI status)
    {
        GameObject elevatorZone = new GameObject("Asansor");
        elevatorZone.transform.position = new Vector3(-2.25f, 1f, 5.55f);
        BoxCollider elevatorCollider = elevatorZone.AddComponent<BoxCollider>();
        elevatorCollider.isTrigger = true;
        elevatorCollider.size = new Vector3(1.8f, 1.8f, 1.2f);
        TahliyeKontrol elevator = elevatorZone.AddComponent<TahliyeKontrol>();
        elevator.uiMetni = status;
        elevator.dogruSecimdeOtomatikGec = true;

        GameObject stairsZone = new GameObject("Merdiven");
        stairsZone.transform.position = new Vector3(2.2f, 1f, 5.05f);
        BoxCollider stairsCollider = stairsZone.AddComponent<BoxCollider>();
        stairsCollider.isTrigger = true;
        stairsCollider.size = new Vector3(1.8f, 1.8f, 1.3f);
        TahliyeKontrol stairs = stairsZone.AddComponent<TahliyeKontrol>();
        stairs.uiMetni = status;
        stairs.dogruSecimdeOtomatikGec = true;
        stairs.hedefSahneAdi = "DisariYolSecimiSahnesi";
    }

    private static void CreateLight()
    {
        GameObject lightObject = new GameObject("Directional Light");
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.35f;
        light.color = new Color(1f, 0.94f, 0.84f);
        light.shadows = LightShadows.Soft;
        lightObject.transform.rotation = Quaternion.Euler(45f, -35f, 0f);
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientIntensity = 1.1f;
    }

    private static void BuildSettingsGuncelle()
    {
        List<EditorBuildSettingsScene> scenes =
            new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        if (!scenes.Exists(scene => scene.path == KoridorSahnesi))
        {
            int outsideIndex = scenes.FindIndex(
                scene => scene.path.EndsWith("DisariYolSecimiSahnesi.unity"));
            int insertIndex = outsideIndex >= 0 ? outsideIndex : scenes.Count;
            scenes.Insert(insertIndex, new EditorBuildSettingsScene(KoridorSahnesi, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }

    private static GameObject CreateBox(
        GameObject parent,
        string name,
        Vector3 position,
        Vector3 scale,
        Material material)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = name;
        box.transform.SetParent(parent.transform);
        box.transform.position = position;
        box.transform.localScale = scale;
        box.GetComponent<Renderer>().sharedMaterial = material;
        return box;
    }

    private static void CreateWorldLabel(
        GameObject parent,
        string text,
        Vector3 position,
        Vector3 rotation)
    {
        GameObject labelObject = new GameObject(text + "_Etiket", typeof(TextMeshPro));
        labelObject.transform.SetParent(parent.transform);
        labelObject.transform.position = position;
        labelObject.transform.eulerAngles = rotation;
        TextMeshPro label = labelObject.GetComponent<TextMeshPro>();
        label.text = text;
        label.fontSize = 3.5f;
        label.alignment = TextAlignmentOptions.Center;
        label.color = Color.white;
        label.rectTransform.sizeDelta = new Vector2(3f, 0.7f);
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
            material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            AssetDatabase.CreateAsset(material, path);
        }
        material.color = color;
        return material;
    }

    private static GameObject SpawnFitted(
        string path,
        GameObject parent,
        string name,
        Vector3 floorPosition,
        Vector3 rotation,
        Vector3 targetSize)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (prefab == null)
        {
            return null;
        }

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instance.name = name;
        instance.transform.SetParent(parent.transform);
        instance.transform.eulerAngles = rotation;
        FitToBox(instance, targetSize, floorPosition);
        return instance;
    }

    private static void FitHeight(GameObject target, float targetHeight)
    {
        Bounds bounds = GetBounds(target);
        target.transform.localScale = Vector3.one *
                                      (targetHeight / Mathf.Max(bounds.size.y, 0.001f));
        bounds = GetBounds(target);
        target.transform.position += target.transform.parent.position -
                                     new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
    }

    private static void FitToBox(GameObject target, Vector3 size, Vector3 floorPosition)
    {
        Bounds bounds = GetBounds(target);
        float scale = Mathf.Min(
            size.x / Mathf.Max(bounds.size.x, 0.001f),
            size.y / Mathf.Max(bounds.size.y, 0.001f),
            size.z / Mathf.Max(bounds.size.z, 0.001f));
        target.transform.localScale = Vector3.one * scale;
        bounds = GetBounds(target);
        target.transform.position += floorPosition -
                                     new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
    }

    private static Bounds GetBounds(GameObject target)
    {
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>(true);
        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }
        return bounds;
    }

    private static void RemoveCharacterDemoComponents(GameObject character)
    {
        foreach (MonoBehaviour behaviour in
                 character.GetComponentsInChildren<MonoBehaviour>(true))
        {
            if (behaviour != null &&
                behaviour.GetType().FullName == "Sample.KidsScript")
            {
                Object.DestroyImmediate(behaviour);
            }
        }

        foreach (CharacterController controller in
                 character.GetComponentsInChildren<CharacterController>(true))
        {
            Object.DestroyImmediate(controller);
        }
    }

    private static void DestroyNamed(string name)
    {
        GameObject obj = GameObject.Find(name);
        if (obj != null)
        {
            Object.DestroyImmediate(obj);
        }
    }
}
