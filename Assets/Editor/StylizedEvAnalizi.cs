using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StylizedEvAnalizi
{
    private const string EvSahnesi =
        "Assets/StylArts/StylizedHouseInterior/Scene/URP_Stylized_House_Interior.unity";

    public static void BatchAnalizEt()
    {
        Scene scene = EditorSceneManager.OpenScene(EvSahnesi, OpenSceneMode.Single);
        StringBuilder report = new StringBuilder();
        report.AppendLine("SCENE=" + scene.path);

        Renderer[] renderers = Object.FindObjectsByType<Renderer>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        if (renderers.Length > 0)
        {
            Bounds all = renderers[0].bounds;
            foreach (Renderer renderer in renderers.Skip(1))
            {
                all.Encapsulate(renderer.bounds);
            }

            report.AppendLine("ALL_BOUNDS_CENTER=" + Format(all.center));
            report.AppendLine("ALL_BOUNDS_SIZE=" + Format(all.size));
        }

        string[] keywords =
        {
            "Floor", "Wall", "Door", "Table", "Desk", "Kitchen",
            "Couch", "Chair", "Carpet", "Bottle", "Radio", "Shelf"
        };

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            report.AppendLine("ROOT=" + Describe(root));
        }

        foreach (Transform transform in Object.FindObjectsByType<Transform>(
                     FindObjectsInactive.Include,
                     FindObjectsSortMode.None))
        {
            if (!keywords.Any(keyword =>
                    transform.name.IndexOf(keyword, System.StringComparison.OrdinalIgnoreCase) >= 0))
            {
                continue;
            }

            Renderer[] objectRenderers = transform.GetComponentsInChildren<Renderer>(true);
            string boundsText = "none";
            if (objectRenderers.Length > 0)
            {
                Bounds bounds = objectRenderers[0].bounds;
                foreach (Renderer renderer in objectRenderers.Skip(1))
                {
                    bounds.Encapsulate(renderer.bounds);
                }
                boundsText = Format(bounds.center) + "|" + Format(bounds.size);
            }

            report.AppendLine(
                "MATCH=" + GetPath(transform) +
                "|pos=" + Format(transform.position) +
                "|bounds=" + boundsText);
        }

        string reportPath = Path.GetFullPath("Temp/StylizedEvAnalizi.txt");
        File.WriteAllText(reportPath, report.ToString());
        Debug.Log("Stylized ev analizi yazildi: " + reportPath);
        EditorApplication.Exit(0);
    }

    private static string Describe(GameObject obj)
    {
        return obj.name + "|active=" + obj.activeSelf + "|pos=" + Format(obj.transform.position);
    }

    private static string Format(Vector3 value)
    {
        return $"{value.x:F3},{value.y:F3},{value.z:F3}";
    }

    private static string GetPath(Transform transform)
    {
        string path = transform.name;
        while (transform.parent != null)
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }
        return path;
    }
}
