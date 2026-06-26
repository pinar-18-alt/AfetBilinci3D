using UnityEngine;

public class GuvenliMasaVurgusu : MonoBehaviour
{
    public Light hedefIsigi;
    public Renderer[] vurguluParcalar;
    public float yanipSonmeHizi = 3.5f;
    public float minIsik = 0.45f;
    public float maxIsik = 2.2f;

    private Material[] materyaller;

    private void Awake()
    {
        if (vurguluParcalar == null || vurguluParcalar.Length == 0)
        {
            vurguluParcalar = GetComponentsInChildren<Renderer>(true);
        }

        materyaller = new Material[vurguluParcalar.Length];
        for (int i = 0; i < vurguluParcalar.Length; i++)
        {
            if (vurguluParcalar[i] == null)
            {
                continue;
            }

            materyaller[i] = vurguluParcalar[i].material;
            materyaller[i].EnableKeyword("_EMISSION");
        }
    }

    private void Update()
    {
        float pulse = (Mathf.Sin(Time.time * yanipSonmeHizi) + 1f) * 0.5f;
        float intensity = Mathf.Lerp(minIsik, maxIsik, pulse);
        Color glow = Color.Lerp(new Color(0.2f, 0.9f, 0.45f), new Color(1f, 0.95f, 0.35f), pulse);

        if (hedefIsigi != null)
        {
            hedefIsigi.intensity = intensity;
            hedefIsigi.color = glow;
        }

        foreach (Material material in materyaller)
        {
            if (material != null)
            {
                material.SetColor("_EmissionColor", glow * intensity);
            }
        }
    }

    public void Kapat()
    {
        gameObject.SetActive(false);
    }
}
