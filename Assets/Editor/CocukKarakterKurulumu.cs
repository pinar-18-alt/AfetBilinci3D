using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class CocukKarakterKurulumu
{
    private const string ScenePath = "Assets/Scenes/DepremCantasiSahnesi.unity";
    private const string CharacterPath =
        "Assets/KidsCharacterFree/Prefabs/Boy0_Humanoid.prefab";

    [MenuItem("Afet Bilinci/Cocuk Karakteri ve Esyalari Kur")]
    public static void Kur()
    {
        EditorSceneManager.OpenScene(ScenePath);

        GameObject player = GameObject.Find("Oyuncu");
        if (player == null)
        {
            Debug.LogError("Oyuncu bulunamadi.");
            return;
        }

        RemoveOldVisuals(player);
        AddChildCharacter(player);
        ConfigureController(player);
        PlaceItems();
        FixNegativeColliderWarnings();

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();
        Debug.Log("Cocuk karakter ve canta esyalari kuruldu.");
    }

    private static void RemoveOldVisuals(GameObject player)
    {
        Camera camera = player.GetComponentInChildren<Camera>(true);
        for (int i = player.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = player.transform.GetChild(i);
            if (camera != null && child == camera.transform)
            {
                continue;
            }

            Object.DestroyImmediate(child.gameObject);
        }

        MeshRenderer rootRenderer = player.GetComponent<MeshRenderer>();
        if (rootRenderer != null)
        {
            rootRenderer.enabled = false;
        }
    }

    private static void AddChildCharacter(GameObject player)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(CharacterPath);
        if (prefab == null)
        {
            Debug.LogError("Cocuk karakter prefabi bulunamadi.");
            return;
        }

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
            animator.updateMode = AnimatorUpdateMode.Normal;
        }

        MonoBehaviour demoScript = character.GetComponent<Sample.KidsScript>();
        if (demoScript != null)
        {
            Object.DestroyImmediate(demoScript);
        }
    }

    private static void ConfigureController(GameObject player)
    {
        CapsuleCollider capsule = player.GetComponent<CapsuleCollider>();
        if (capsule != null)
        {
            capsule.enabled = false;
        }

        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = player.AddComponent<CharacterController>();
        }

        controller.center = new Vector3(0f, 0.68f, 0f);
        controller.height = 1.35f;
        controller.radius = 0.28f;
        controller.stepOffset = 0.22f;
        controller.skinWidth = 0.04f;
        controller.slopeLimit = 45f;

        player.transform.position = new Vector3(3.2f, 0.22f, -1.6f);

        Camera camera = player.GetComponentInChildren<Camera>(true);
        if (camera != null)
        {
            camera.transform.localPosition = new Vector3(0f, 1.5f, -3.35f);
            camera.transform.localEulerAngles = new Vector3(9f, 0f, 0f);
        }
    }

    private static void PlaceItems()
    {
        SetItem("Kup_Su", new Vector3(-6.2f, 1.3f, 4.55f));
        SetItem("Kup_Fener", new Vector3(0.1f, 0.68f, -4.95f));
        SetItem(
            "Kup_Laptop",
            new Vector3(-1.9f, 1.24f, 2.5f),
            new Vector3(0f, 0.35f, -0.65f),
            new Vector3(2.8f, 1.7f, 2.5f));
        SetItem("Kup_Oyuncak", new Vector3(1.1f, 0.22f, -3.4f));
    }

    private static void SetItem(
        string name,
        Vector3 position,
        Vector3? triggerCenter = null,
        Vector3? triggerSize = null)
    {
        GameObject item = GameObject.Find(name);
        if (item == null)
        {
            return;
        }

        item.transform.position = position;
        BoxCollider trigger = item.GetComponent<BoxCollider>();
        if (trigger != null)
        {
            trigger.isTrigger = true;
            trigger.center = triggerCenter ?? new Vector3(0f, 0.45f, 0f);
            trigger.size = triggerSize ?? new Vector3(1.25f, 1.5f, 1.25f);
        }

        EditorUtility.SetDirty(item);
    }

    private static void FixNegativeColliderWarnings()
    {
        foreach (BoxCollider collider in Object.FindObjectsByType<BoxCollider>(
                     FindObjectsInactive.Include,
                     FindObjectsSortMode.None))
        {
            Vector3 size = collider.size;
            Vector3 fixedSize = new Vector3(
                Mathf.Max(Mathf.Abs(size.x), 0.001f),
                Mathf.Max(Mathf.Abs(size.y), 0.001f),
                Mathf.Max(Mathf.Abs(size.z), 0.001f));

            if (size != fixedSize)
            {
                collider.size = fixedSize;
                EditorUtility.SetDirty(collider);
            }
        }
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
}
