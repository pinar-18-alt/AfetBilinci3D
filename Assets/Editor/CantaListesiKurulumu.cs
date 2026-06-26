using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class CantaListesiKurulumu
{
    private const string ScenePath = "Assets/Scenes/DepremCantasiSahnesi.unity";
    private const string WaterPrefab = "Assets/Low Poly Cartoon Food and Groceries Pack/Prefabs/water_bottle.prefab";
    private const string FlashlightPrefab = "Assets/Akduman/ToonTastic - Electronic Devices/Prefabs/Prop_FlashLight.prefab";
    private const string CanPrefab = "Assets/StylArts/StylizedHouseInterior/Art/Prefabs/SM_FoodCan03.prefab";
    private const string WalkiePrefab = "Assets/Akduman/ToonTastic - Electronic Devices/Prefabs/Prop_WalkieTalkie.prefab";
    private const string TeddyPrefab = "Assets/Christmas-Presents/Prefabs/Teddybear.prefab";

    [MenuItem("Afet Bilinci/Canta Listesi ve 5 Esyayi Kur")]
    public static void Kur()
    {
        EditorSceneManager.OpenScene(ScenePath);

        TextMeshProUGUI status = FindTmp("CantaDurumYazisi");
        CantaKontrol control = Object.FindFirstObjectByType<CantaKontrol>(FindObjectsInactive.Include);
        CantaListesiUI list = CreateListPanel();

        if (control != null)
        {
            control.toplamGerekliEsya = 5;
            control.listeUI = list;
            control.durumYazisi = status;
            EditorUtility.SetDirty(control);
        }

        ConfigureItem("Kup_Su", "su", "Su", new Vector3(-6.55f, 1.05f, 3.90f), ItemVisual.Water, new Vector3(0.22f, 0.42f, 0.22f), new Vector3(0f, 25f, 0f), "E tusuna bas: Suyu al", "Su, acil durumda susuz kalmamani saglar.", control, status);
        ConfigureItem("Kup_Fener", "fener", "Fener", new Vector3(0.05f, 0.78f, -4.78f), ItemVisual.Flashlight, new Vector3(0.46f, 0.22f, 0.22f), new Vector3(0f, 90f, 0f), "E tusuna bas: Feneri al", "Fener, elektrik kesilirse yolu gormene yardim eder.", control, status);
        ConfigureItem("Kup_Konserve", "konserve", "Konserve", new Vector3(-5.35f, 1.05f, 4.18f), ItemVisual.Can, new Vector3(0.24f, 0.34f, 0.24f), new Vector3(0f, -15f, 0f), "E tusuna bas: Konserveyi al", "Konserve, bozulmadan saklanabilen yiyecektir.", control, status);
        ConfigureItem("Kup_SaglikKiti", "saglik", "Saglik kiti", new Vector3(1.45f, 1.12f, 4.72f), ItemVisual.FirstAid, new Vector3(0.48f, 0.32f, 0.26f), new Vector3(0f, 180f, 0f), "E tusuna bas: Saglik kitini al", "Saglik kiti, kucuk yaralanmalarda ilk yardim icindir.", control, status);
        ConfigureItem("Kup_Telsiz", "telsiz", "Telsiz", new Vector3(0.55f, 0.78f, -4.58f), ItemVisual.Radio, new Vector3(0.32f, 0.40f, 0.22f), new Vector3(0f, 18f, 0f), "E tusuna bas: Telsizi al", "Telsiz, haberlesmek ve yardim almak icin kullanilir.", control, status);

        ConfigureWrongItem("Kup_Laptop", control, status);
        ConfigureWrongItem("Kup_Oyuncak", control, status);

        if (status != null)
        {
            status.text = "Acil durum cantani hazirla. Listedeki 5 esyayi bul.";
            EditorUtility.SetDirty(status);
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();
        Debug.Log("Canta listesi gercek assetlerle duzenlendi.");
    }

    private enum ItemVisual
    {
        Water,
        Flashlight,
        Can,
        FirstAid,
        Radio
    }

    private static CantaListesiUI CreateListPanel()
    {
        DestroyNamed("CantaListePaneli");

        Canvas canvas = Object.FindFirstObjectByType<Canvas>(FindObjectsInactive.Include);
        if (canvas == null)
        {
            return null;
        }

        GameObject panel = new GameObject("CantaListePaneli", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(CantaListesiUI));
        panel.transform.SetParent(canvas.transform, false);

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1f, 0.5f);
        rect.anchorMax = new Vector2(1f, 0.5f);
        rect.pivot = new Vector2(1f, 0.5f);
        rect.anchoredPosition = new Vector2(-34f, 0f);
        rect.sizeDelta = new Vector2(365f, 320f);

        Image image = panel.GetComponent<Image>();
        image.color = new Color(1f, 0.96f, 0.78f, 0.92f);

        TextMeshProUGUI title = CreateUiText(panel.transform, "CantaListeBaslik", "Acil durum cantamda:", 26f, FontStyles.Bold);
        SetFixedRect(title.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -20f), new Vector2(320f, 38f), new Vector2(0.5f, 1f));
        title.alignment = TextAlignmentOptions.Center;
        title.color = new Color(0.12f, 0.25f, 0.34f);

        CantaListesiUI list = panel.GetComponent<CantaListesiUI>();
        string[,] rows =
        {
            { "su", "Su" },
            { "fener", "Fener" },
            { "konserve", "Konserve" },
            { "saglik", "Saglik kiti" },
            { "telsiz", "Telsiz" }
        };

        for (int i = 0; i < rows.GetLength(0); i++)
        {
            TextMeshProUGUI row = CreateUiText(panel.transform, "CantaListe_" + rows[i, 0], "[ ] " + rows[i, 1], 25f, FontStyles.Bold);
            SetFixedRect(row.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -74f - i * 43f), new Vector2(305f, 34f), new Vector2(0.5f, 1f));
            row.color = new Color(0.18f, 0.22f, 0.25f);
            list.satirlar.Add(new CantaListesiUI.ListeSatiri { esyaId = rows[i, 0], yazi = row });
        }

        EditorUtility.SetDirty(panel);
        return list;
    }

    private static void ConfigureItem(
        string name,
        string id,
        string displayName,
        Vector3 position,
        ItemVisual visual,
        Vector3 targetSize,
        Vector3 localEuler,
        string prompt,
        string explanation,
        CantaKontrol control,
        TextMeshProUGUI status)
    {
        GameObject item = GameObject.Find(name);
        if (item == null)
        {
            item = new GameObject(name);
        }

        item.transform.position = position;
        item.transform.rotation = Quaternion.identity;
        item.transform.localScale = Vector3.one;

        for (int i = item.transform.childCount - 1; i >= 0; i--)
        {
            Object.DestroyImmediate(item.transform.GetChild(i).gameObject);
        }

        CreateVisual(item.transform, visual, targetSize, localEuler);
        CreateWorldLabel(item.transform, displayName);

        BoxCollider trigger = item.GetComponent<BoxCollider>();
        if (trigger == null)
        {
            trigger = item.AddComponent<BoxCollider>();
        }

        trigger.isTrigger = true;
        trigger.center = new Vector3(0f, 0.45f, 0f);
        trigger.size = new Vector3(1.35f, 1.15f, 1.35f);

        CantaEsyasiEtkilesimi interaction = item.GetComponent<CantaEsyasiEtkilesimi>();
        if (interaction == null)
        {
            interaction = item.AddComponent<CantaEsyasiEtkilesimi>();
        }

        interaction.esyaTuru = CantaEsyasiEtkilesimi.EsyaTuru.Dogru;
        interaction.esyaId = id;
        interaction.yonergeMetni = prompt;
        interaction.aciklamaMetni = explanation;
        interaction.cantaKontrol = control;
        interaction.yonergeYazisi = status;

        EditorUtility.SetDirty(item);
        EditorUtility.SetDirty(interaction);
    }

    private static void ConfigureWrongItem(string name, CantaKontrol control, TextMeshProUGUI status)
    {
        GameObject item = GameObject.Find(name);
        if (item == null)
        {
            item = new GameObject(name);
        }

        if (name == "Kup_Oyuncak")
        {
            item.transform.position = new Vector3(-2.2f, 0.2f, -3.55f);
            item.transform.rotation = Quaternion.identity;
            item.transform.localScale = Vector3.one;

            for (int i = item.transform.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(item.transform.GetChild(i).gameObject);
            }

            CreateToyVisual(item.transform);

            BoxCollider trigger = item.GetComponent<BoxCollider>();
            if (trigger == null)
            {
                trigger = item.AddComponent<BoxCollider>();
            }
            trigger.isTrigger = true;
            trigger.center = new Vector3(0f, 0.35f, 0f);
            trigger.size = new Vector3(1.25f, 1.0f, 1.25f);
        }

        CantaEsyasiEtkilesimi interaction = item.GetComponent<CantaEsyasiEtkilesimi>();
        if (interaction == null)
        {
            interaction = item.AddComponent<CantaEsyasiEtkilesimi>();
        }

        interaction.esyaTuru = CantaEsyasiEtkilesimi.EsyaTuru.Yanlis;
        interaction.esyaId = "";
        interaction.cantaKontrol = control;
        interaction.yonergeYazisi = status;
        interaction.yonergeMetni = "E tusuna bas: Kontrol et";
        interaction.yanlisMesaj = "Bu esya bekleyebilir. Listedeki acil esyalari bul.";
        EditorUtility.SetDirty(interaction);
    }

    private static void CreateToyVisual(Transform parent)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(TeddyPrefab);
        if (prefab != null)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
            instance.name = "Oyuncak_Ayicik";
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.Euler(0f, 25f, 0f);
            instance.transform.localScale = Vector3.one;
            RemoveChildColliders(instance);
            FitVisualToSize(parent, instance.transform, new Vector3(0.55f, 0.62f, 0.45f));
            return;
        }

        Material yellow = MaterialOlustur("CantaSari", new Color(1f, 0.82f, 0.18f));
        CreateBox(parent, "Oyuncak_Kutu", new Vector3(0f, 0.2f, 0f), new Vector3(0.45f, 0.4f, 0.45f), yellow);
    }

    private static void CreateVisual(Transform parent, ItemVisual visual, Vector3 targetSize, Vector3 localEuler)
    {
        string prefabPath = GetPrefabPath(visual);
        if (!string.IsNullOrEmpty(prefabPath))
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab != null)
            {
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
                instance.name = visual + "_Visual";
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localRotation = Quaternion.Euler(localEuler);
                instance.transform.localScale = Vector3.one;
                RemoveChildColliders(instance);
                FitVisualToSize(parent, instance.transform, targetSize);
                return;
            }
        }

        if (visual == ItemVisual.FirstAid)
        {
            Material red = MaterialOlustur("CantaKirmizi", new Color(0.86f, 0.18f, 0.16f));
            Material white = MaterialOlustur("CantaBeyaz", new Color(0.96f, 0.96f, 0.92f));
            CreateFirstAidFallback(parent, targetSize, red, white);
        }
    }

    private static string GetPrefabPath(ItemVisual visual)
    {
        return visual switch
        {
            ItemVisual.Water => WaterPrefab,
            ItemVisual.Flashlight => FlashlightPrefab,
            ItemVisual.Can => CanPrefab,
            ItemVisual.Radio => WalkiePrefab,
            _ => ""
        };
    }

    private static void CreateFirstAidFallback(Transform parent, Vector3 targetSize, Material red, Material white)
    {
        CreateBox(parent, "SaglikCantasi", new Vector3(0f, targetSize.y * 0.5f, 0f), targetSize, white);
        CreateBox(parent, "SaglikArtiDikey", new Vector3(0f, targetSize.y * 0.53f, -targetSize.z * 0.51f), new Vector3(targetSize.x * 0.18f, targetSize.y * 0.64f, 0.025f), red);
        CreateBox(parent, "SaglikArtiYatay", new Vector3(0f, targetSize.y * 0.53f, -targetSize.z * 0.525f), new Vector3(targetSize.x * 0.55f, targetSize.y * 0.18f, 0.025f), red);
        CreateBox(parent, "SaglikTutacak", new Vector3(0f, targetSize.y + 0.04f, 0f), new Vector3(targetSize.x * 0.55f, 0.05f, targetSize.z * 0.18f), white);
    }

    private static TextMeshProUGUI CreateUiText(Transform parent, string name, string text, float fontSize, FontStyles style)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        obj.transform.SetParent(parent, false);

        TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.fontStyle = style;
        tmp.alignment = TextAlignmentOptions.Left;
        tmp.enableAutoSizing = true;
        tmp.fontSizeMin = 18f;
        tmp.fontSizeMax = fontSize;
        return tmp;
    }

    private static void SetFixedRect(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 size, Vector2 pivot)
    {
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = pivot;
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = size;
    }

    private static void CreateWorldLabel(Transform parent, string text)
    {
        GameObject obj = new GameObject("Etiket_" + text, typeof(TextMeshPro));
        obj.transform.SetParent(parent, false);
        obj.transform.localPosition = new Vector3(0f, 0.62f, 0f);

        TextMeshPro tmp = obj.GetComponent<TextMeshPro>();
        tmp.text = text;
        tmp.fontSize = 0.26f;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = new Color(0.08f, 0.18f, 0.26f);
        tmp.rectTransform.sizeDelta = new Vector2(2.5f, 0.5f);
    }

    private static GameObject CreateBox(Transform parent, string name, Vector3 localPosition, Vector3 localScale, Material material)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = name;
        box.transform.SetParent(parent, false);
        box.transform.localPosition = localPosition;
        box.transform.localScale = localScale;
        box.GetComponent<Renderer>().sharedMaterial = material;
        Object.DestroyImmediate(box.GetComponent<Collider>());
        return box;
    }

    private static void RemoveChildColliders(GameObject root)
    {
        foreach (Collider collider in root.GetComponentsInChildren<Collider>(true))
        {
            Object.DestroyImmediate(collider);
        }
    }

    private static void FitVisualToSize(Transform parent, Transform visual, Vector3 targetSize)
    {
        if (!TryGetBounds(visual, out Bounds bounds))
        {
            return;
        }

        Vector3 size = bounds.size;
        float scale = Mathf.Min(
            targetSize.x / Mathf.Max(size.x, 0.001f),
            targetSize.y / Mathf.Max(size.y, 0.001f),
            targetSize.z / Mathf.Max(size.z, 0.001f));
        visual.localScale *= scale;

        if (!TryGetBounds(visual, out bounds))
        {
            return;
        }

        Vector3 desiredAnchor = parent.position;
        Vector3 currentAnchor = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
        visual.position += desiredAnchor - currentAnchor;
    }

    private static bool TryGetBounds(Transform root, out Bounds bounds)
    {
        Renderer[] renderers = root.GetComponentsInChildren<Renderer>(true);
        bounds = new Bounds(root.position, Vector3.zero);
        bool found = false;

        foreach (Renderer renderer in renderers)
        {
            if (!found)
            {
                bounds = renderer.bounds;
                found = true;
            }
            else
            {
                bounds.Encapsulate(renderer.bounds);
            }
        }

        return found;
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
