using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class StylizedEvMateryalDuzeltici
{
    private static readonly string[] MaterialRoots =
    {
        "Assets/StylArts/StylizedHouseInterior/Art",
        "Assets/StylArts/StylizedHouseInterior/HDRP/Art"
    };
    private const string CantaSahnesi =
        "Assets/Scenes/DepremCantasiSahnesi.unity";
    private const string VersionKey =
        "AfetBilinci.StylizedEvMateryalDuzeltme.v2";

    static StylizedEvMateryalDuzeltici()
    {
        EditorApplication.delayCall += OtomatikDuzelt;
    }

    [MenuItem("Afet Bilinci/Stylized Ev Materyallerini Duzelt")]
    public static void Duzelt()
    {
        Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
        if (urpLit == null)
        {
            Debug.LogError("URP/Lit shader bulunamadi.");
            return;
        }

        string[] materialGuids = AssetDatabase.FindAssets("t:Material", MaterialRoots);
        int converted = 0;

        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null || material.shader == urpLit)
            {
                continue;
            }

            Texture baseMap = FirstTexture(material, "_BaseMap", "_MainTex", "_BaseColorMap");
            Texture normalMap = FirstTexture(material, "_BumpMap", "_NormalMap");
            Texture maskMap = FirstTexture(material, "_MaskMap", "_MetallicGlossMap");
            Color baseColor = FirstColor(material, Color.white, "_BaseColor", "_Color");
            float metallic = FirstFloat(material, 0f, "_Metallic");
            float smoothness = FirstFloat(material, 0.35f, "_Smoothness", "_Glossiness");
            bool transparent = baseColor.a < 0.99f ||
                               FirstFloat(material, 0f, "_Surface") > 0.5f;

            material.shader = urpLit;
            material.SetColor("_BaseColor", baseColor);
            material.SetFloat("_Metallic", Mathf.Clamp01(metallic));
            material.SetFloat("_Smoothness", Mathf.Clamp01(smoothness));

            if (baseMap != null)
            {
                material.SetTexture("_BaseMap", baseMap);
            }

            if (normalMap != null)
            {
                material.SetTexture("_BumpMap", normalMap);
                material.EnableKeyword("_NORMALMAP");
            }

            if (maskMap != null)
            {
                material.SetTexture("_MetallicGlossMap", maskMap);
                material.EnableKeyword("_METALLICSPECGLOSSMAP");
            }

            if (transparent)
            {
                material.SetFloat("_Surface", 1f);
                material.SetFloat("_Blend", 0f);
                material.SetFloat("_SrcBlend", 5f);
                material.SetFloat("_DstBlend", 10f);
                material.SetFloat("_ZWrite", 0f);
                material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                material.renderQueue = 3000;
            }
            else
            {
                material.SetFloat("_Surface", 0f);
                material.SetFloat("_ZWrite", 1f);
                material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
                material.renderQueue = -1;
            }

            EditorUtility.SetDirty(material);
            converted++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        DuzeltKamera();
        EditorPrefs.SetBool(VersionKey, true);
        Debug.Log("Stylized ev materyalleri URP/Lit'e donusturuldu: " + converted);
    }

    [MenuItem("Afet Bilinci/Stylized Evi Son Kez Onar")]
    public static void SonOnarim()
    {
        Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
        string[] materialGuids = AssetDatabase.FindAssets("t:Material", MaterialRoots);
        int recovered = 0;

        foreach (string guid in materialGuids)
        {
            Material material = AssetDatabase.LoadAssetAtPath<Material>(
                AssetDatabase.GUIDToAssetPath(guid));
            if (material == null)
            {
                continue;
            }

            Texture baseMap = FindSavedTexture(
                material,
                "basecolor", "basemap", "maintex", "diffuse", "albedo",
                "_bc", "_d", "color");
            Texture normalMap = FindSavedTexture(
                material,
                "normal", "bump", "_n");
            Texture maskMap = FindSavedTexture(
                material,
                "mask", "orm", "mrao", "metallic", "mre", "rom");

            material.shader = urpLit;

            if (baseMap != null)
            {
                material.SetTexture("_BaseMap", baseMap);
                recovered++;
            }

            if (normalMap != null)
            {
                material.SetTexture("_BumpMap", normalMap);
                material.EnableKeyword("_NORMALMAP");
            }

            if (maskMap != null)
            {
                material.SetTexture("_MetallicGlossMap", maskMap);
            }

            material.SetFloat("_Metallic", 0f);
            material.SetFloat("_Smoothness", 0.3f);
            EditorUtility.SetDirty(material);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        DuzeltSahneGorunumu();
        Debug.Log("Stylized ev son onarimi tamamlandi. Kurtarilan ana doku: " + recovered);
    }

    private static void OtomatikDuzelt()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode ||
            EditorPrefs.GetBool(VersionKey, false))
        {
            return;
        }

        Duzelt();
    }

    private static void DuzeltKamera()
    {
        if (EditorSceneManager.GetActiveScene().path != CantaSahnesi)
        {
            EditorSceneManager.OpenScene(CantaSahnesi);
        }

        foreach (Camera camera in Object.FindObjectsByType<Camera>(
                     FindObjectsInactive.Include,
                     FindObjectsSortMode.None))
        {
            if (!camera.gameObject.activeInHierarchy)
            {
                continue;
            }

            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.53f, 0.73f, 0.82f);
            EditorUtility.SetDirty(camera);
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
    }

    private static void DuzeltSahneGorunumu()
    {
        if (EditorSceneManager.GetActiveScene().path != CantaSahnesi)
        {
            EditorSceneManager.OpenScene(CantaSahnesi);
        }

        GameObject lightRoot = GameObject.Find("Lights");
        if (lightRoot != null)
        {
            lightRoot.SetActive(false);
            EditorUtility.SetDirty(lightRoot);
        }

        GameObject lightObject = GameObject.Find("Oyun_AnaIsik");
        if (lightObject == null)
        {
            lightObject = new GameObject("Oyun_AnaIsik");
        }

        Light mainLight = lightObject.GetComponent<Light>();
        if (mainLight == null)
        {
            mainLight = lightObject.AddComponent<Light>();
        }

        mainLight.type = LightType.Directional;
        mainLight.color = new Color(1f, 0.94f, 0.84f);
        mainLight.intensity = 1.35f;
        mainLight.shadows = LightShadows.Soft;
        lightObject.transform.rotation = Quaternion.Euler(48f, -32f, 0f);

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = new Color(0.63f, 0.72f, 0.78f);
        RenderSettings.ambientEquatorColor = new Color(0.48f, 0.46f, 0.43f);
        RenderSettings.ambientGroundColor = new Color(0.24f, 0.23f, 0.22f);
        RenderSettings.ambientIntensity = 1.15f;

        GameObject player = GameObject.Find("Oyuncu");
        if (player != null)
        {
            player.transform.position = new Vector3(3.2f, 0.22f, -1.6f);
            Camera camera = player.GetComponentInChildren<Camera>(true);
            if (camera != null)
            {
                camera.transform.localPosition = new Vector3(0f, 1.55f, -3.1f);
                camera.transform.localEulerAngles = new Vector3(8f, 0f, 0f);
                camera.fieldOfView = 65f;
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = new Color(0.53f, 0.73f, 0.82f);
                EditorUtility.SetDirty(camera);
            }
            EditorUtility.SetDirty(player);
        }

        EditorUtility.SetDirty(lightObject);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
        SceneView.RepaintAll();
    }

    private static Texture FindSavedTexture(Material material, params string[] hints)
    {
        SerializedObject serialized = new SerializedObject(material);
        SerializedProperty textures =
            serialized.FindProperty("m_SavedProperties.m_TexEnvs");

        if (textures == null)
        {
            return null;
        }

        Texture fallback = null;
        for (int i = 0; i < textures.arraySize; i++)
        {
            SerializedProperty entry = textures.GetArrayElementAtIndex(i);
            string propertyName =
                entry.FindPropertyRelative("first").stringValue.ToLowerInvariant();
            Texture texture = entry
                .FindPropertyRelative("second.m_Texture")
                .objectReferenceValue as Texture;

            if (texture == null)
            {
                continue;
            }

            fallback ??= texture;
            foreach (string hint in hints)
            {
                if (propertyName.Contains(hint))
                {
                    return texture;
                }
            }
        }

        return hints.Length > 0 && hints[0] == "basecolor" ? fallback : null;
    }

    private static Texture FirstTexture(Material material, params string[] names)
    {
        foreach (string name in names)
        {
            if (material.HasProperty(name))
            {
                Texture texture = material.GetTexture(name);
                if (texture != null)
                {
                    return texture;
                }
            }
        }
        return null;
    }

    private static Color FirstColor(
        Material material,
        Color fallback,
        params string[] names)
    {
        foreach (string name in names)
        {
            if (material.HasProperty(name))
            {
                return material.GetColor(name);
            }
        }
        return fallback;
    }

    private static float FirstFloat(
        Material material,
        float fallback,
        params string[] names)
    {
        foreach (string name in names)
        {
            if (material.HasProperty(name))
            {
                return material.GetFloat(name);
            }
        }
        return fallback;
    }
}
