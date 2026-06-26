using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class DepremSahnesiIkinciDuzenleme
{
    private const string ScenePath = "Assets/Scenes/SampleScene.unity";
    private const string StairsPath =
        "Assets/ThirdParty/KenneyFurnitureKit/Models/FBX format/stairsOpen.fbx";

    [MenuItem("Afet Bilinci/Deprem Sahnesi Ikinci Duzenleme")]
    public static void Duzenle()
    {
        EditorSceneManager.OpenScene(ScenePath);

        DestroyNamed("CokKapanTutunAlani");
        DestroyNamed("TahliyeSecenekleri");
        DestroyNamed("ApartmanKoridoru");
        DestroyNamed("TehlikeliAlanlar");

        TextMeshProUGUI status = FindTmp("TalimatYazisi");
        GameObject continueButton = GameObject.Find("DisariyaDevamButonu");
        YumusakKameraSarsintisi shake =
            Object.FindFirstObjectByType<YumusakKameraSarsintisi>();

        GameObject dangerRoot = CreateDangerZones(status);
        GameObject hallRoot = CreateApartmentHall(status, continueButton);
        hallRoot.SetActive(false);

        GameObject safeArea = new GameObject("CokKapanTutunAlani");
        safeArea.transform.position = new Vector3(-1.9f, 0.65f, 2.5f);
        BoxCollider safeTrigger = safeArea.AddComponent<BoxCollider>();
        safeTrigger.isTrigger = true;
        safeTrigger.size = new Vector3(2.25f, 1.35f, 2.15f);

        CokKapanTutunGorevi task = safeArea.AddComponent<CokKapanTutunGorevi>();
        task.yonergeYazisi = status;
        task.bilgiYazisi = status;
        task.gerekliSure = 3f;
        task.tamamlanincaAktifOlacak = hallRoot;
        task.tamamlanincaKapanacak = dangerRoot;
        task.kameraSarsintisi = shake;

        OpenHouseExit();

        if (status != null)
        {
            status.text = "Sarsıntı başladı. Laptop masasının yanına git.";
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();
        Debug.Log("Guvenli alan, tehlikeli bolgeler ve apartman koridoru kuruldu.");
    }

    private static GameObject CreateDangerZones(TextMeshProUGUI status)
    {
        GameObject root = new GameObject("TehlikeliAlanlar");
        CreateDangerZone(
            root.transform,
            "MutfakMasasi_Tehlikeli",
            new Vector3(-6.73f, 0.75f, -0.18f),
            new Vector3(3f, 1.5f, 3.8f),
            "Mutfak masası güvenli değil. Laptop masasının yanına git.",
            status);
        CreateDangerZone(
            root.transform,
            "TVUnitesi_Tehlikeli",
            new Vector3(0.1f, 0.75f, -4.95f),
            new Vector3(3.4f, 1.5f, 2.2f),
            "TV ünitesinden uzaklaş. Devrilebilir.",
            status);
        return root;
    }

    private static void CreateDangerZone(
        Transform parent,
        string name,
        Vector3 position,
        Vector3 size,
        string message,
        TextMeshProUGUI status)
    {
        GameObject zone = new GameObject(name);
        zone.transform.SetParent(parent);
        zone.transform.position = position;
        BoxCollider trigger = zone.AddComponent<BoxCollider>();
        trigger.isTrigger = true;
        trigger.size = size;
        TehlikeliAlanUyarisi warning = zone.AddComponent<TehlikeliAlanUyarisi>();
        warning.uyariYazisi = status;
        warning.mesaj = message;
    }

    private static GameObject CreateApartmentHall(
        TextMeshProUGUI status,
        GameObject continueButton)
    {
        GameObject root = new GameObject("ApartmanKoridoru");
        Material floor = MaterialOlustur("Koridor_Zemin", new Color(0.23f, 0.27f, 0.29f));
        Material wall = MaterialOlustur("Koridor_Duvar", new Color(0.72f, 0.75f, 0.72f));
        Material accent = MaterialOlustur("Koridor_Vurgu", new Color(0.18f, 0.47f, 0.55f));
        Material elevator = MaterialOlustur("Asansor_Kapi", new Color(0.34f, 0.38f, 0.41f));

        CreateBox(root, "KoridorZemini", new Vector3(8.1f, 0.08f, 3.3f), new Vector3(7.2f, 0.16f, 4.4f), floor);
        CreateBox(root, "KoridorTavani", new Vector3(8.1f, 3.05f, 3.3f), new Vector3(7.2f, 0.12f, 4.4f), wall);
        CreateBox(root, "KoridorDuvarSol", new Vector3(8.1f, 1.55f, 1.05f), new Vector3(7.2f, 3f, 0.15f), wall);
        CreateBox(root, "KoridorDuvarSag", new Vector3(8.1f, 1.55f, 5.55f), new Vector3(7.2f, 3f, 0.15f), wall);
        CreateBox(root, "KoridorSonDuvar", new Vector3(11.65f, 1.55f, 3.3f), new Vector3(0.15f, 3f, 4.4f), wall);
        CreateBox(root, "YonlendirmeSeridi", new Vector3(8.1f, 0.17f, 3.3f), new Vector3(6.5f, 0.03f, 0.35f), accent);

        CreateBox(root, "AsansorKapiSol", new Vector3(11.52f, 1.3f, 1.9f), new Vector3(0.18f, 2.45f, 0.95f), elevator);
        CreateBox(root, "AsansorKapiSag", new Vector3(11.52f, 1.3f, 2.9f), new Vector3(0.18f, 2.45f, 0.95f), elevator);
        CreateLabel(root, "ASANSÖR", new Vector3(11.38f, 2.72f, 2.4f), new Vector3(0f, -90f, 0f));

        GameObject elevatorZone = new GameObject("Asansor");
        elevatorZone.transform.SetParent(root.transform);
        elevatorZone.transform.position = new Vector3(10.65f, 0.9f, 2.4f);
        BoxCollider elevatorTrigger = elevatorZone.AddComponent<BoxCollider>();
        elevatorTrigger.isTrigger = true;
        elevatorTrigger.size = new Vector3(1.8f, 1.8f, 2.3f);
        TahliyeKontrol elevatorControl = elevatorZone.AddComponent<TahliyeKontrol>();
        elevatorControl.uiMetni = status;
        elevatorControl.devamButonu = continueButton;

        GameObject stairs = SpawnFitted(
            StairsPath,
            root,
            "MerdivenModeli",
            new Vector3(9.35f, 0.15f, 4.55f),
            new Vector3(0f, 90f, 0f),
            new Vector3(2.8f, 2.1f, 2.2f));
        if (stairs != null)
        {
            CreateLabel(root, "MERDİVEN", new Vector3(9.35f, 2.55f, 5.25f), new Vector3(0f, 180f, 0f));
        }

        GameObject stairsZone = new GameObject("Merdiven");
        stairsZone.transform.SetParent(root.transform);
        stairsZone.transform.position = new Vector3(9.35f, 0.85f, 4.45f);
        BoxCollider stairsTrigger = stairsZone.AddComponent<BoxCollider>();
        stairsTrigger.isTrigger = true;
        stairsTrigger.size = new Vector3(2.5f, 1.7f, 2.2f);
        TahliyeKontrol stairsControl = stairsZone.AddComponent<TahliyeKontrol>();
        stairsControl.uiMetni = status;
        stairsControl.devamButonu = continueButton;

        return root;
    }

    private static void OpenHouseExit()
    {
        foreach (Transform transform in Object.FindObjectsByType<Transform>(
                     FindObjectsInactive.Include,
                     FindObjectsSortMode.None))
        {
            if (transform.name.Contains("Door_Proxy_208"))
            {
                transform.gameObject.SetActive(false);
                EditorUtility.SetDirty(transform.gameObject);
            }
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

    private static void CreateLabel(
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
        label.fontSize = 3.8f;
        label.alignment = TextAlignmentOptions.Center;
        label.color = Color.white;
        label.rectTransform.sizeDelta = new Vector2(3f, 0.7f);
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

        instance.transform.position += floorPosition -
                                       new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
        return instance;
    }

    private static Material MaterialOlustur(string name, Color color)
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
        EditorUtility.SetDirty(material);
        return material;
    }

    private static TextMeshProUGUI FindTmp(string name)
    {
        GameObject obj = GameObject.Find(name);
        return obj != null ? obj.GetComponent<TextMeshProUGUI>() : null;
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
