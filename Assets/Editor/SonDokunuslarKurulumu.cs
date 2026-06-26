using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class SonDokunuslarKurulumu
{
    private const string DepremSahnesi = "Assets/Scenes/SampleScene.unity";

    [MenuItem("Afet Bilinci/Son Dokunuslari Uygula")]
    public static void Uygula()
    {
        EditorSceneManager.OpenScene(DepremSahnesi);
        GuvenliMasaVurgusuKur();
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();
        Debug.Log("Son dokunuslar uygulandi: click sesi runtime'da aktif, guvenli masa vurgusu kuruldu.");
    }

    public static void GuvenliMasaVurgusuKur()
    {
        DestroyNamed("GuvenliMasaVurgusu");

        Material green = MaterialOlustur("GuvenliMasa_YesilVurgu", new Color(0.18f, 0.9f, 0.42f));
        Material yellow = MaterialOlustur("GuvenliMasa_SariVurgu", new Color(1f, 0.88f, 0.25f));

        GameObject root = new GameObject("GuvenliMasaVurgusu");
        root.transform.position = new Vector3(-1.9f, 0.08f, 2.5f);

        CreateRing(root, green);
        CreateArrow(root, yellow);
        CreateWorldText(root, "GuvenliMasaYazisi", "GUVENLI MASA", new Vector3(0f, 2.0f, 0f), 0f, green.color, 0.75f);

        GameObject lightObject = new GameObject("YanipSonenIsik");
        lightObject.transform.SetParent(root.transform, false);
        lightObject.transform.localPosition = new Vector3(0f, 1.8f, 0f);
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Point;
        light.range = 4.2f;
        light.intensity = 1.6f;
        light.shadows = LightShadows.None;

        GuvenliMasaVurgusu vurgu = root.AddComponent<GuvenliMasaVurgusu>();
        vurgu.hedefIsigi = light;
        vurgu.vurguluParcalar = root.GetComponentsInChildren<Renderer>(true);

        CokKapanTutunGorevi gorev =
            Object.FindFirstObjectByType<CokKapanTutunGorevi>(FindObjectsInactive.Include);
        if (gorev != null)
        {
            gorev.tamamlanincaKapanacakVurgu = root;
            EditorUtility.SetDirty(gorev);
        }
    }

    private static void CreateRing(GameObject parent, Material material)
    {
        const int count = 16;
        const float radius = 1.45f;
        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2f / count;
            GameObject segment = GameObject.CreatePrimitive(PrimitiveType.Cube);
            segment.name = "VurguHalka_" + i;
            segment.transform.SetParent(parent.transform, false);
            segment.transform.localPosition = new Vector3(Mathf.Cos(angle) * radius, 0.04f, Mathf.Sin(angle) * radius);
            segment.transform.localEulerAngles = new Vector3(0f, -angle * Mathf.Rad2Deg, 0f);
            segment.transform.localScale = new Vector3(0.55f, 0.06f, 0.16f);
            segment.GetComponent<Renderer>().sharedMaterial = material;
            Object.DestroyImmediate(segment.GetComponent<Collider>());
        }
    }

    private static void CreateArrow(GameObject parent, Material material)
    {
        GameObject arrow = new GameObject("MasaYonOku");
        arrow.transform.SetParent(parent.transform, false);
        arrow.transform.localPosition = new Vector3(0f, 0.1f, -2.05f);
        arrow.transform.localEulerAngles = Vector3.zero;

        CreateBox(arrow, "OkGovde", Vector3.zero, new Vector3(0.55f, 0.06f, 1.0f), material);
        GameObject left = CreateBox(arrow, "OkBasSol", new Vector3(-0.25f, 0f, 0.45f), new Vector3(0.42f, 0.06f, 0.75f), material);
        left.transform.localEulerAngles = new Vector3(0f, -38f, 0f);
        GameObject right = CreateBox(arrow, "OkBasSag", new Vector3(0.25f, 0f, 0.45f), new Vector3(0.42f, 0.06f, 0.75f), material);
        right.transform.localEulerAngles = new Vector3(0f, 38f, 0f);
    }

    private static GameObject CreateBox(GameObject parent, string name, Vector3 localPosition, Vector3 localScale, Material material)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = name;
        box.transform.SetParent(parent.transform, false);
        box.transform.localPosition = localPosition;
        box.transform.localScale = localScale;
        box.GetComponent<Renderer>().sharedMaterial = material;
        Object.DestroyImmediate(box.GetComponent<Collider>());
        return box;
    }

    private static void CreateWorldText(
        GameObject parent,
        string name,
        string value,
        Vector3 localPosition,
        float yRotation,
        Color color,
        float fontSize)
    {
        GameObject obj = new GameObject(name, typeof(TextMeshPro));
        obj.transform.SetParent(parent.transform, false);
        obj.transform.localPosition = localPosition;
        obj.transform.localEulerAngles = new Vector3(0f, yRotation, 0f);
        TextMeshPro text = obj.GetComponent<TextMeshPro>();
        text.text = value;
        text.fontSize = fontSize;
        text.fontStyle = FontStyles.Bold;
        text.alignment = TextAlignmentOptions.Center;
        text.color = color;
        text.rectTransform.sizeDelta = new Vector2(4f, 1f);
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
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }
            material = new Material(shader);
            AssetDatabase.CreateAsset(material, path);
        }
        material.color = color;
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", color * 0.6f);
        EditorUtility.SetDirty(material);
        return material;
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
