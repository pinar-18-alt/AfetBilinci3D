using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class DisariSahneSifirdanKurulum
{
    private const string ScenePath = "Assets/Scenes/DisariYolSecimiSahnesi.unity";
    private const string CharacterPath = "Assets/KidsCharacterFree/Prefabs/Boy0_Humanoid.prefab";
    private const string CityRoot = "Assets/ithappy/Cartoon_City_Free/Prefabs/";

    private const string RoadStraight = CityRoot + "Roads/road_001.prefab";
    private const string RoadCross = CityRoot + "Roads/road_003.prefab";
    private const string RoadTurn = CityRoot + "Roads/road_009.prefab";
    private const string SidewalkA = CityRoot + "Sidewalks/Set_B_Tiles_01.prefab";
    private const string SidewalkB = CityRoot + "Sidewalks/Set_B_Tiles_04.prefab";
    private const string SidewalkC = CityRoot + "Sidewalks/Set_B_Tiles_05.prefab";
    private const string BuildingGrid = CityRoot + "Buildings/Eco_Building_Grid.prefab";
    private const string BuildingGridNight = CityRoot + "Buildings/Eco_Building_Grid_NightLight.prefab";
    private const string BuildingSlope = CityRoot + "Buildings/Eco_Building_Slope.prefab";
    private const string BuildingTerrace = CityRoot + "Buildings/Eco_Building_Terrace.prefab";
    private const string BuildingTower = CityRoot + "Buildings/Regular_Building_TwistedTower_Large.prefab";
    private const string Car06 = CityRoot + "Cars/Car_06.prefab";
    private const string Car13 = CityRoot + "Cars/Car_13.prefab";
    private const string Car16 = CityRoot + "Cars/Car_16.prefab";
    private const string Van = CityRoot + "Cars/Van.prefab";
    private const string TrafficLight = CityRoot + "Props/traffic_light_001.prefab";
    private const string BusStop = CityRoot + "Props/Bus_Stop_02.prefab";
    private const string Fountain = CityRoot + "Props/Fountain_03.prefab";
    private const string Spotlight = CityRoot + "Props/Spotlight_01.prefab";
    private const string TrashCan = CityRoot + "Props/Trash_Can_04.prefab";
    private const string Signboard = CityRoot + "Billboards/Signboard_01.prefab";
    private const string Billboard = CityRoot + "Billboards/Billboard_4x1_03.prefab";
    private const string Bush06 = CityRoot + "Vegetation/Bush_06.prefab";
    private const string Bush07 = CityRoot + "Vegetation/Bush_07.prefab";
    private const string Bush10 = CityRoot + "Vegetation/Bush_10.prefab";
    private const string Palm03 = CityRoot + "Vegetation/Palm_03.prefab";
    private const string TentYellow = "Assets/Palmov Island/Low Poly Atmospheric Locations Pack/Prefabs/Environment/Camping environment/camping tent yellow.prefab";

    [MenuItem("Afet Bilinci/Dis Sahneyi Sifirdan Kur")]
    public static void Kur()
    {
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "DisariYolSecimiSahnesi";

        GameObject root = new GameObject("CartoonCityFinalScene");
        TextMeshProUGUI status = CreateUi();

        CreateCity(root);
        CreateAssemblyPark(root, status);
        CreatePlayer();
        CreateCamera();
        CreateLightAndSky();
        CreateFadeManager();

        EditorSceneManager.SaveScene(scene, ScenePath);
        AssetDatabase.SaveAssets();
        Debug.Log("Dis sahne yeni Cartoon City paketiyle temiz kuruldu. Eski yol secimi objeleri kaldirildi, oyuncu korundu.");
    }

    private static void CreateCity(GameObject parent)
    {
        CreateInvisibleCollider(parent, "YurunebilirGenisZemin", new Vector3(0f, -0.08f, 20f), new Vector3(92f, 0.16f, 76f));
        CreateInvisibleCollider(parent, "SolSinir", new Vector3(-45f, 1.5f, 20f), new Vector3(0.5f, 3f, 76f));
        CreateInvisibleCollider(parent, "SagSinir", new Vector3(45f, 1.5f, 20f), new Vector3(0.5f, 3f, 76f));
        CreateInvisibleCollider(parent, "BaslangicSiniri", new Vector3(0f, 1.5f, -18f), new Vector3(92f, 3f, 0.5f));
        CreateInvisibleCollider(parent, "BitisSiniri", new Vector3(0f, 1.5f, 58f), new Vector3(92f, 3f, 0.5f));

        CreateMainRoad(parent);
        CreateSidewalks(parent);
        CreateBuildings(parent);
        CreateCarsAndProps(parent);
        CreateRouteSigns(parent);
    }

    private static void CreateMainRoad(GameObject parent)
    {
        Material asphalt = GetMaterial("FinalClean_Asphalt", new Color(0.14f, 0.17f, 0.18f));
        Material line = GetMaterial("FinalClean_RoadLine", new Color(0.95f, 0.82f, 0.28f));
        Material grass = GetMaterial("FinalClean_Grass", new Color(0.24f, 0.57f, 0.25f));

        CreateBox(parent, "DuzSehirZemini", new Vector3(0f, -0.04f, 20f), new Vector3(92f, 0.08f, 76f), GetMaterial("FinalClean_Ground", new Color(0.52f, 0.50f, 0.46f)), false);
        CreateBox(parent, "AnaYol", new Vector3(0f, 0.02f, 16f), new Vector3(13.5f, 0.06f, 58f), asphalt, false);
        CreateBox(parent, "KavsakYolu", new Vector3(0f, 0.025f, 25f), new Vector3(52f, 0.06f, 10f), asphalt, false);
        CreateBox(parent, "ToplanmaParkCimeni", new Vector3(0f, 0.04f, 42f), new Vector3(25f, 0.08f, 17f), grass, false);
        CreateBox(parent, "ToplanmaMeydaniZemini", new Vector3(0f, 0.09f, 42f), new Vector3(13f, 0.08f, 8.5f), GetMaterial("FinalClean_Plaza", new Color(0.62f, 0.59f, 0.52f)), false);

        for (int i = 0; i < 9; i++)
        {
            CreateBox(parent, "YolCizgisi_" + i, new Vector3(0f, 0.08f, -8f + i * 5.5f), new Vector3(0.18f, 0.025f, 2.35f), line, false);
        }

        for (int i = 0; i < 6; i++)
        {
            CreateBox(parent, "KavsakCizgisi_" + i, new Vector3(-20f + i * 8f, 0.085f, 25f), new Vector3(3.2f, 0.025f, 0.18f), line, false);
        }
    }

    private static void CreateSidewalks(GameObject parent)
    {
        Material sidewalk = GetMaterial("FinalClean_Sidewalk", new Color(0.66f, 0.67f, 0.62f));

        CreateBox(parent, "SolAnaKaldirim", new Vector3(-9.2f, 0.065f, 16f), new Vector3(4.2f, 0.08f, 58f), sidewalk, false);
        CreateBox(parent, "SagAnaKaldirim", new Vector3(9.2f, 0.065f, 16f), new Vector3(4.2f, 0.08f, 58f), sidewalk, false);
        CreateBox(parent, "SolKavsakKaldirim", new Vector3(-18.5f, 0.07f, 18.5f), new Vector3(19f, 0.08f, 3.2f), sidewalk, false);
        CreateBox(parent, "SagKavsakKaldirim", new Vector3(18.5f, 0.07f, 31.5f), new Vector3(19f, 0.08f, 3.2f), sidewalk, false);
        CreateBox(parent, "ParkGirisYolu", new Vector3(0f, 0.075f, 34.5f), new Vector3(8f, 0.08f, 7f), sidewalk, false);
    }

    private static void CreateBuildings(GameObject parent)
    {
        string[] buildings =
        {
            BuildingGrid,
            BuildingSlope,
            BuildingTerrace
        };

        for (int i = 0; i < 9; i++)
        {
            float z = -10f + i * 7.2f;
            float rightScale = i % 3 == 0 ? 0.18f : 0.16f;
            float leftScale = i % 2 == 0 ? 0.17f : 0.16f;

            GameObject right = Spawn(buildings[i % buildings.Length], parent, "SagCepheBinasi_" + i, new Vector3(28f, 0f, z), new Vector3(0f, -90f, 0f), Vector3.one * rightScale);
            GameObject left = Spawn(buildings[(i + 1) % buildings.Length], parent, "SolCepheBinasi_" + i, new Vector3(-30f, 0f, z + 1.8f), new Vector3(0f, 90f, 0f), Vector3.one * leftScale);
            DisableColliders(right);
            DisableColliders(left);
        }

        for (int i = 0; i < 7; i++)
        {
            float z = -5f + i * 8f;

            GameObject rightBack = Spawn(buildings[(i + 2) % buildings.Length], parent, "SagArkaSiraBinasi_" + i, new Vector3(39f, 0f, z), new Vector3(0f, -92f, 0f), Vector3.one * 0.14f);
            GameObject leftBack = Spawn(buildings[i % buildings.Length], parent, "SolArkaSiraBinasi_" + i, new Vector3(-41f, 0f, z + 2f), new Vector3(0f, 88f, 0f), Vector3.one * 0.14f);
            DisableColliders(rightBack);
            DisableColliders(leftBack);
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject far = Spawn(buildings[(i + 1) % buildings.Length], parent, "UzakSehirSilueti_" + i, new Vector3(-18f + i * 9f, 0f, 55f), new Vector3(0f, 180f, 0f), Vector3.one * 0.13f);
            DisableColliders(far);
        }
    }

    private static void CreateCarsAndProps(GameObject parent)
    {
        Spawn(Car06, parent, "ParkEtmisArac_1", new Vector3(6.8f, 0.08f, 4f), new Vector3(0f, 180f, 0f), Vector3.one * 0.5f);
        Spawn(Car13, parent, "ParkEtmisArac_2", new Vector3(-6.8f, 0.08f, 17f), new Vector3(0f, 0f, 0f), Vector3.one * 0.5f);
        Spawn(Van, parent, "ServisAraci", new Vector3(17f, 0.08f, 25f), new Vector3(0f, -90f, 0f), Vector3.one * 0.5f);

        Spawn(BusStop, parent, "OtobusDuragi", new Vector3(10.8f, 0.08f, 33f), new Vector3(0f, -90f, 0f), Vector3.one * 0.55f);
        Spawn(TrashCan, parent, "CopKutusu", new Vector3(9.2f, 0.08f, 30f), Vector3.zero, Vector3.one * 0.55f);
    }

    private static void CreateRouteSigns(GameObject parent)
    {
        Material routeMat = GetMaterial("FinalClean_RouteGreen", new Color(0.08f, 0.72f, 0.30f));
        Vector3[] routePoints =
        {
            new Vector3(0f, 0.15f, -4f),
            new Vector3(0f, 0.15f, 7f),
            new Vector3(0f, 0.15f, 18f),
            new Vector3(0f, 0.15f, 29f),
            new Vector3(0f, 0.15f, 36f)
        };

        for (int i = 0; i < routePoints.Length; i++)
        {
            CreateGroundArrow(parent, "GuvenliRotaOku_" + i, routePoints[i], 0f, routeMat);
        }
    }

    private static void CreateAssemblyPark(GameObject parent, TextMeshProUGUI status)
    {
        Spawn(BusStop, parent, "ToplanmaBilgiNoktasi", new Vector3(7f, 0.1f, 40f), new Vector3(0f, -90f, 0f), Vector3.one * 0.45f);

        for (int i = 0; i < 10; i++)
        {
            float x = -10f + (i % 5) * 5f;
            float z = 39f + (i / 5) * 7f;
            Spawn(i % 2 == 0 ? Palm03 : Bush10, parent, "ParkBitkisi_" + i, new Vector3(x, 0.1f, z), Vector3.zero, Vector3.one * (i % 2 == 0 ? 0.42f : 0.75f));
        }

        Spawn(Bush06, parent, "ParkCali_1", new Vector3(-8f, 0.1f, 46f), Vector3.zero, Vector3.one * 0.75f);
        Spawn(Bush07, parent, "ParkCali_2", new Vector3(8f, 0.1f, 46f), Vector3.zero, Vector3.one * 0.75f);
        GameObject tent = Spawn(TentYellow, parent, "KurulacakCadir", new Vector3(0f, 0.12f, 42f), new Vector3(0f, -12f, 0f), Vector3.one * 0.72f);

        GameObject safe = new GameObject("CadirKurmaAlani");
        safe.transform.position = new Vector3(0f, 1f, 42f);
        BoxCollider collider = safe.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(11f, 2f, 9f);
        CadirKurmaGorevi task = safe.AddComponent<CadirKurmaGorevi>();
        task.durumYazisi = status;
        task.cadirGorseli = tent;
        task.gerekliSure = 2.2f;
        task.sonrakiSahne = "SkorEkrani";
    }

    private static void CreatePlayer()
    {
        GameObject player = new GameObject("Oyuncu");
        player.tag = "Player";
        player.transform.position = new Vector3(0f, 0.25f, -8.5f);
        player.transform.rotation = Quaternion.Euler(0f, -8f, 0f);
        player.AddComponent<OyuncuKontrol>();

        CharacterController controller = player.AddComponent<CharacterController>();
        controller.center = new Vector3(0f, 0.68f, 0f);
        controller.height = 1.35f;
        controller.radius = 0.28f;
        controller.stepOffset = 0.22f;

        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(CharacterPath);
        if (prefab != null)
        {
            GameObject character = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            character.name = "CocukKarakter";
            character.transform.SetParent(player.transform, false);
            FitHeight(character, 1.35f);
            RemoveCharacterDemoComponents(character);

            Animator animator = character.GetComponent<Animator>();
            if (animator != null)
            {
                animator.applyRootMotion = false;
            }
        }
    }

    private static void CreateCamera()
    {
        GameObject player = GameObject.Find("Oyuncu");
        GameObject cameraObject = new GameObject("Main Camera");
        cameraObject.tag = "MainCamera";
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.fieldOfView = 65f;
        camera.nearClipPlane = 0.08f;
        camera.farClipPlane = 160f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.53f, 0.78f, 0.98f);

        BasitKameraTakip follow = cameraObject.AddComponent<BasitKameraTakip>();
        if (player != null)
        {
            follow.hedef = player.transform;
        }
        follow.mesafe = new Vector3(0f, 2.4f, -4.8f);
        follow.takipHizi = 8f;

        cameraObject.transform.position = new Vector3(0f, 2.2f, -13.3f);
        cameraObject.transform.rotation = Quaternion.Euler(15f, 0f, 0f);
        cameraObject.AddComponent<AudioListener>();
    }

    private static TextMeshProUGUI CreateUi()
    {
        GameObject canvasObject = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);

        GameObject panel = new GameObject("YolYaziPaneli", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        panel.transform.SetParent(canvasObject.transform, false);
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 1f);
        panelRect.anchorMax = new Vector2(0.5f, 1f);
        panelRect.anchoredPosition = new Vector2(0f, -82f);
        panelRect.sizeDelta = new Vector2(980f, 105f);
        panel.GetComponent<Image>().color = new Color(0.05f, 0.16f, 0.22f, 0.82f);

        GameObject textObject = new GameObject("YolDurumYazisi", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        textObject.transform.SetParent(panel.transform, false);
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;

        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
        text.text = "Tabelalari takip et. Guvenli alana git.";
        text.fontSize = 32f;
        text.enableAutoSizing = true;
        text.fontSizeMin = 22f;
        text.fontSizeMax = 34f;
        text.fontStyle = FontStyles.Bold;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;

        new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        return text;
    }

    private static void CreateLightAndSky()
    {
        GameObject lightObject = new GameObject("Directional Light");
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.25f;
        light.color = new Color(1f, 0.94f, 0.84f);
        light.shadows = LightShadows.Soft;
        lightObject.transform.rotation = Quaternion.Euler(45f, -35f, 0f);

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientIntensity = 1.2f;

        RenderSettings.skybox = null;
    }

    private static void CreateFadeManager()
    {
        GameObject manager = new GameObject("KarartmaYonetici");
        EkranKarartma fade = manager.AddComponent<EkranKarartma>();

        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            return;
        }

        GameObject imageObject = new GameObject("SiyahEkran", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        imageObject.transform.SetParent(canvas.transform, false);
        RectTransform rect = imageObject.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image = imageObject.GetComponent<Image>();
        image.color = new Color(0f, 0f, 0f, 0f);
        image.raycastTarget = false;
        fade.siyahGorsel = image;
    }

    private static void CreateGroundArrow(GameObject parent, string name, Vector3 position, float yRotation, Material material)
    {
        GameObject arrow = new GameObject(name);
        arrow.transform.SetParent(parent.transform, false);
        arrow.transform.localPosition = position;
        arrow.transform.localEulerAngles = new Vector3(0f, yRotation, 0f);

        CreateBox(arrow, "Govde", Vector3.zero, new Vector3(0.55f, 0.055f, 1.25f), material, false);
        GameObject headLeft = CreateBox(arrow, "BasSol", new Vector3(-0.28f, 0f, 0.48f), new Vector3(0.42f, 0.055f, 0.72f), material, false);
        headLeft.transform.localEulerAngles = new Vector3(0f, -38f, 0f);
        GameObject headRight = CreateBox(arrow, "BasSag", new Vector3(0.28f, 0f, 0.48f), new Vector3(0.42f, 0.055f, 0.72f), material, false);
        headRight.transform.localEulerAngles = new Vector3(0f, 38f, 0f);
    }

    private static GameObject CreateBox(GameObject parent, string name, Vector3 position, Vector3 scale, Material material, bool colliderOn)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = name;
        if (parent != null)
        {
            box.transform.SetParent(parent.transform, false);
            box.transform.localPosition = position;
        }
        else
        {
            box.transform.position = position;
        }

        box.transform.localScale = scale;
        box.GetComponent<Renderer>().sharedMaterial = material;

        if (!colliderOn)
        {
            Object.DestroyImmediate(box.GetComponent<Collider>());
        }

        return box;
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
        return material;
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
        if (parent != null)
        {
            instance.transform.SetParent(parent.transform);
        }
        instance.transform.position = position;
        instance.transform.eulerAngles = rotation;
        instance.transform.localScale = scale;
        AlignBottomToY(instance, position.y);

        foreach (Collider collider in instance.GetComponentsInChildren<Collider>(true))
        {
            collider.enabled = false;
        }

        return instance;
    }

    private static void AlignBottomToY(GameObject target, float y)
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

        target.transform.position += Vector3.up * (y - bounds.min.y);
    }

    private static GameObject CreateInvisibleCollider(GameObject parent, string name, Vector3 position, Vector3 size)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent.transform, false);
        obj.transform.localPosition = position;
        BoxCollider collider = obj.AddComponent<BoxCollider>();
        collider.size = size;
        return obj;
    }

    private static void DisableColliders(GameObject target)
    {
        if (target == null)
        {
            return;
        }

        foreach (Collider collider in target.GetComponentsInChildren<Collider>(true))
        {
            collider.enabled = false;
        }
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
        text.rectTransform.sizeDelta = new Vector2(5f, 1.6f);
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

        target.transform.localScale = Vector3.one * (targetHeight / Mathf.Max(bounds.size.y, 0.001f));
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
}
