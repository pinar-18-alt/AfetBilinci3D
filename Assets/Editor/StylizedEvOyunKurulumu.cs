using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StylizedEvOyunKurulumu
{
    private const string KaynakEv =
        "Assets/StylArts/StylizedHouseInterior/Scene/URP_Stylized_House_Interior.unity";
    private const string CantaSahnesi = "Assets/Scenes/DepremCantasiSahnesi.unity";
    private const string YedekSahne = "Assets/Scenes/Backups/DepremCantasiSahnesi_Eski.unity";
    private const string PrefabRoot =
        "Assets/StylArts/StylizedHouseInterior/Art/Prefabs/";

    public static void BatchCantaSahnesiniKur()
    {
        EnsureBackup();

        Scene oldScene = EditorSceneManager.OpenScene(CantaSahnesi, OpenSceneMode.Single);
        Scene houseScene = EditorSceneManager.OpenScene(KaynakEv, OpenSceneMode.Additive);

        string[] rootsToMove =
        {
            "Canvas", "EventSystem", "Oyuncu", "CantaYonetici",
            "Kup_Su", "Kup_Fener", "Kup_Laptop", "Kup_Oyuncak"
        };

        foreach (string rootName in rootsToMove)
        {
            GameObject root = FindRoot(oldScene, rootName);
            if (root != null)
            {
                SceneManager.MoveGameObjectToScene(root, houseScene);
            }
        }

        EditorSceneManager.CloseScene(oldScene, true);
        SceneManager.SetActiveScene(houseScene);

        ConfigureHouse();
        ConfigurePlayer();
        ConfigureItems();
        ConfigureUi();

        EditorSceneManager.SaveScene(houseScene, CantaSahnesi, true);
        AssetDatabase.SaveAssets();
        Debug.Log("Stylized ev, deprem cantasi sahnesine kuruldu.");
        EditorApplication.Exit(0);
    }

    private static void EnsureBackup()
    {
        const string backupFolder = "Assets/Scenes/Backups";
        if (!AssetDatabase.IsValidFolder(backupFolder))
        {
            AssetDatabase.CreateFolder("Assets/Scenes", "Backups");
        }

        if (AssetDatabase.LoadAssetAtPath<SceneAsset>(YedekSahne) == null)
        {
            AssetDatabase.CopyAsset(CantaSahnesi, YedekSahne);
        }
    }

    private static void ConfigureHouse()
    {
        GameObject cameras = GameObject.Find("Cameras");
        if (cameras != null)
        {
            cameras.SetActive(false);
            EditorUtility.SetDirty(cameras);
        }

        GameObject environment = new GameObject("Oyun_EvAyarlari");
        BoxCollider floorSafety = environment.AddComponent<BoxCollider>();
        floorSafety.center = new Vector3(-0.7f, 0.05f, 2.58f);
        floorSafety.size = new Vector3(19.4f, 0.18f, 18.5f);

        Light[] lights = Object.FindObjectsByType<Light>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);
        foreach (Light light in lights)
        {
            if (!light.gameObject.activeInHierarchy)
            {
                continue;
            }

            light.intensity = Mathf.Min(light.intensity, 2.2f);
            EditorUtility.SetDirty(light);
        }
    }

    private static void ConfigurePlayer()
    {
        GameObject player = GameObject.Find("Oyuncu");
        if (player == null)
        {
            return;
        }

        player.tag = "Player";
        player.transform.position = new Vector3(3.2f, 1.2f, -1.6f);
        player.transform.eulerAngles = new Vector3(0f, -90f, 0f);

        MeshRenderer capsule = player.GetComponent<MeshRenderer>();
        if (capsule != null)
        {
            capsule.enabled = false;
        }

        Camera camera = player.GetComponentInChildren<Camera>(true);
        if (camera != null)
        {
            camera.gameObject.SetActive(true);
            camera.tag = "MainCamera";
            camera.transform.localPosition = new Vector3(0f, 2.15f, -4.4f);
            camera.transform.localEulerAngles = new Vector3(10f, 0f, 0f);
            camera.fieldOfView = 62f;
            camera.nearClipPlane = 0.08f;
            camera.farClipPlane = 120f;
            EditorUtility.SetDirty(camera);
        }

        EditorUtility.SetDirty(player);
    }

    private static void ConfigureItems()
    {
        ConfigureItem(
            "Kup_Su",
            new Vector3(-6.2f, 1.22f, 4.55f),
            PrefabRoot + "SM_Bottles_08.prefab",
            new Vector3(0.28f, 0.55f, 0.28f),
            "E tuşuna bas: Suyu al");

        ConfigureItem(
            "Kup_Fener",
            new Vector3(0.1f, 0.78f, -4.95f),
            "Assets/Akduman/ToonTastic - Electronic Devices/Prefabs/Prop_FlashLight.prefab",
            new Vector3(0.55f, 0.3f, 0.3f),
            "E tuşuna bas: Feneri al");

        ConfigureItem(
            "Kup_Laptop",
            new Vector3(-1.9f, 1.28f, 2.5f),
            "Assets/ThirdParty/KenneyFurnitureKit/Models/FBX format/laptop.fbx",
            new Vector3(0.75f, 0.38f, 0.58f),
            "E tuşuna bas: Kontrol et",
            new Vector3(0f, 0.35f, -0.65f),
            new Vector3(2.8f, 1.7f, 2.5f));

        ConfigureItem(
            "Kup_Oyuncak",
            new Vector3(1.1f, 0.25f, -3.4f),
            "Assets/ThirdParty/KenneyFurnitureKit/Models/FBX format/bear.fbx",
            new Vector3(0.55f, 0.72f, 0.5f),
            "E tuşuna bas: Kontrol et");
    }

    private static void ConfigureItem(
        string name,
        Vector3 position,
        string visualPath,
        Vector3 targetSize,
        string prompt,
        Vector3? triggerCenter = null,
        Vector3? triggerSize = null)
    {
        GameObject item = GameObject.Find(name);
        if (item == null)
        {
            return;
        }

        item.transform.position = position;
        item.transform.rotation = Quaternion.identity;
        item.transform.localScale = Vector3.one;

        for (int i = item.transform.childCount - 1; i >= 0; i--)
        {
            Object.DestroyImmediate(item.transform.GetChild(i).gameObject);
        }

        GameObject visualAsset = AssetDatabase.LoadAssetAtPath<GameObject>(visualPath);
        if (visualAsset != null)
        {
            GameObject visual = (GameObject)PrefabUtility.InstantiatePrefab(visualAsset);
            visual.name = name + "_Gorsel";
            visual.transform.SetParent(item.transform, false);
            FitVisual(visual, targetSize);
        }

        BoxCollider trigger = item.GetComponent<BoxCollider>();
        if (trigger != null)
        {
            trigger.isTrigger = true;
            trigger.center = triggerCenter ?? new Vector3(0f, 0.45f, 0f);
            trigger.size = triggerSize ?? new Vector3(1.7f, 1.7f, 1.7f);
            EditorUtility.SetDirty(trigger);
        }

        CantaEsyasiEtkilesimi interaction = item.GetComponent<CantaEsyasiEtkilesimi>();
        if (interaction != null)
        {
            interaction.yonergeMetni = prompt;
            EditorUtility.SetDirty(interaction);
        }

        EditorUtility.SetDirty(item);
    }

    private static void FitVisual(GameObject visual, Vector3 targetSize)
    {
        Renderer[] renderers = visual.GetComponentsInChildren<Renderer>(true);
        if (renderers.Length == 0)
        {
            return;
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
        visual.transform.localScale *= scale;

        renderers = visual.GetComponentsInChildren<Renderer>(true);
        bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        visual.transform.position += visual.transform.parent.position -
                                     new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
    }

    private static void ConfigureUi()
    {
        TextMeshProUGUI status = FindTmp("CantaDurumYazisi");
        if (status != null)
        {
            status.text = "Evde su ve feneri bul.";
            status.fontSize = 32f;
            status.fontSizeMin = 24f;
            status.fontSizeMax = 34f;
            status.enableAutoSizing = true;
            status.raycastTarget = false;
            EditorUtility.SetDirty(status);
        }

        TextMeshProUGUI buttonText = FindTmp("Text (TMP)");
        if (buttonText != null)
        {
            buttonText.text = "Çantam hazır";
            EditorUtility.SetDirty(buttonText);
        }
    }

    private static GameObject FindRoot(Scene scene, string name)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            if (root.name == name)
            {
                return root;
            }
        }
        return null;
    }

    private static TextMeshProUGUI FindTmp(string name)
    {
        foreach (TextMeshProUGUI text in Object.FindObjectsByType<TextMeshProUGUI>(
                     FindObjectsInactive.Include,
                     FindObjectsSortMode.None))
        {
            if (text.gameObject.name == name)
            {
                return text;
            }
        }
        return null;
    }
}
