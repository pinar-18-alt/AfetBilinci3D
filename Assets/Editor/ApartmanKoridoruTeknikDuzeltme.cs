using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class ApartmanKoridoruTeknikDuzeltme
{
    private const string ScenePath = "Assets/Scenes/ApartmanKoridoruSahnesi.unity";
    private const string ReferenceScenePath = "Assets/Scenes/SampleScene.unity";

    [MenuItem("Afet Bilinci/Apartman Koridoru Kamera ve Zemin Duzelt")]
    public static void Duzelt()
    {
        CameraSettings referenceCamera = ReadReferenceCamera();
        TransformSettings referenceCharacter = ReadReferenceCharacter();

        EditorSceneManager.OpenScene(ScenePath);

        GameObject player = GameObject.Find("Oyuncu");
        if (player != null)
        {
            player.transform.position = new Vector3(0f, 0.22f, 1.15f);

            Camera camera = player.GetComponentInChildren<Camera>(true);
            if (camera != null)
            {
                ApplyCameraSettings(camera, referenceCamera);
                EditorUtility.SetDirty(camera);
                EditorUtility.SetDirty(camera.transform);
            }

            Animator animator = player.GetComponentInChildren<Animator>(true);
            if (animator != null && referenceCharacter.isValid)
            {
                animator.transform.localPosition = referenceCharacter.localPosition;
                animator.transform.localRotation = referenceCharacter.localRotation;
                animator.transform.localScale = referenceCharacter.localScale;
                EditorUtility.SetDirty(animator.transform);
            }

            RemoveCharacterDemoComponents(player);

            DusmeKoruyucu guard = player.GetComponent<DusmeKoruyucu>();
            if (guard == null)
            {
                guard = player.AddComponent<DusmeKoruyucu>();
            }
            guard.minimumY = -1.2f;
            EditorUtility.SetDirty(player);
        }

        DestroyNamed("YonSeridi");

        GameObject safetyRoot = GameObject.Find("KoridorGuvenlikSinirlari");
        if (safetyRoot == null)
        {
            safetyRoot = new GameObject("KoridorGuvenlikSinirlari");
        }
        ClearChildren(safetyRoot);

        CreateInvisibleBox(
            safetyRoot,
            "AltGuvenlikZemini",
            new Vector3(0f, -0.22f, 3.2f),
            new Vector3(11f, 0.4f, 10f));
        CreateInvisibleBox(
            safetyRoot,
            "SolEkBariyer",
            new Vector3(-4.05f, 1.25f, 3.2f),
            new Vector3(0.18f, 2.5f, 7f));
        CreateInvisibleBox(
            safetyRoot,
            "SagEkBariyer",
            new Vector3(4.05f, 1.25f, 3.2f),
            new Vector3(0.18f, 2.5f, 7f));
        CreateInvisibleBox(
            safetyRoot,
            "MerdivenAltiGuvenlik",
            new Vector3(2.2f, 0.05f, 5.55f),
            new Vector3(3.2f, 0.25f, 3f));

        ColliderleriKapat("MerdivenModeli");
        ColliderleriKapat("MerdivenKapisi");
        TetikleyiciyiDuzelt(
            "Asansor",
            new Vector3(-2.25f, 1f, 5.55f),
            new Vector3(1.8f, 1.8f, 1.2f));
        TetikleyiciyiDuzelt(
            "Merdiven",
            new Vector3(2.2f, 1f, 5.05f),
            new Vector3(1.8f, 1.8f, 1.3f));

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
        Debug.Log("Koridor kamerasi onceki sahneyle eslesti, orta serit kaldirildi.");
    }

    private static CameraSettings ReadReferenceCamera()
    {
        EditorSceneManager.OpenScene(ReferenceScenePath);
        GameObject player = GameObject.Find("Oyuncu");
        Camera camera = player != null
            ? player.GetComponentInChildren<Camera>(true)
            : Camera.main;

        if (camera == null)
        {
            return CameraSettings.Default;
        }

        return new CameraSettings
        {
            localPosition = camera.transform.localPosition,
            localRotation = camera.transform.localRotation,
            fieldOfView = camera.fieldOfView,
            nearClipPlane = camera.nearClipPlane,
            farClipPlane = camera.farClipPlane,
            isValid = true
        };
    }

    private static TransformSettings ReadReferenceCharacter()
    {
        GameObject player = GameObject.Find("Oyuncu");
        Animator animator = player != null
            ? player.GetComponentInChildren<Animator>(true)
            : null;

        if (animator == null)
        {
            return default;
        }

        return new TransformSettings
        {
            localPosition = animator.transform.localPosition,
            localRotation = animator.transform.localRotation,
            localScale = animator.transform.localScale,
            isValid = true
        };
    }

    private static void ApplyCameraSettings(
        Camera camera,
        CameraSettings settings)
    {
        if (!settings.isValid)
        {
            settings = CameraSettings.Default;
        }

        camera.transform.localPosition = settings.localPosition;
        camera.transform.localRotation = settings.localRotation;
        camera.fieldOfView = settings.fieldOfView;
        camera.nearClipPlane = settings.nearClipPlane;
        camera.farClipPlane = settings.farClipPlane;
    }

    private static void CreateInvisibleBox(
        GameObject parent,
        string name,
        Vector3 position,
        Vector3 size)
    {
        GameObject box = new GameObject(name);
        box.transform.SetParent(parent.transform);
        box.transform.position = position;
        BoxCollider collider = box.AddComponent<BoxCollider>();
        collider.size = size;
    }

    private static void ClearChildren(GameObject parent)
    {
        for (int i = parent.transform.childCount - 1; i >= 0; i--)
        {
            Object.DestroyImmediate(parent.transform.GetChild(i).gameObject);
        }
    }

    private static void ColliderleriKapat(string objectName)
    {
        GameObject obj = GameObject.Find(objectName);
        if (obj == null)
        {
            return;
        }

        foreach (Collider collider in obj.GetComponentsInChildren<Collider>(true))
        {
            collider.enabled = false;
            EditorUtility.SetDirty(collider);
        }
    }

    private static void TetikleyiciyiDuzelt(
        string objectName,
        Vector3 position,
        Vector3 size)
    {
        GameObject obj = GameObject.Find(objectName);
        if (obj == null)
        {
            return;
        }

        obj.transform.position = position;
        BoxCollider trigger = obj.GetComponent<BoxCollider>();
        if (trigger != null)
        {
            trigger.isTrigger = true;
            trigger.center = Vector3.zero;
            trigger.size = size;
            EditorUtility.SetDirty(trigger);
        }
        EditorUtility.SetDirty(obj.transform);
    }

    private static void DestroyNamed(string objectName)
    {
        GameObject obj = GameObject.Find(objectName);
        if (obj != null)
        {
            Object.DestroyImmediate(obj);
        }
    }

    private static void RemoveCharacterDemoComponents(GameObject player)
    {
        foreach (MonoBehaviour behaviour in
                 player.GetComponentsInChildren<MonoBehaviour>(true))
        {
            if (behaviour != null &&
                behaviour.GetType().FullName == "Sample.KidsScript")
            {
                Object.DestroyImmediate(behaviour);
            }
        }

        foreach (CharacterController controller in
                 player.GetComponentsInChildren<CharacterController>(true))
        {
            if (controller.gameObject != player)
            {
                Object.DestroyImmediate(controller);
            }
        }
    }

    private struct CameraSettings
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
        public float fieldOfView;
        public float nearClipPlane;
        public float farClipPlane;
        public bool isValid;

        public static CameraSettings Default => new CameraSettings
        {
            localPosition = new Vector3(0f, 1.5f, -3.35f),
            localRotation = Quaternion.Euler(9f, 0f, 0f),
            fieldOfView = 65f,
            nearClipPlane = 0.08f,
            farClipPlane = 1000f,
            isValid = true
        };
    }

    private struct TransformSettings
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;
        public bool isValid;
    }
}
