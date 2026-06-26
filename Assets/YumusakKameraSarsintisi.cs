using UnityEngine;

public class YumusakKameraSarsintisi : MonoBehaviour
{
    public bool sarsintiAktif = true;
    public float siddet = 0.025f;
    public float hiz = 18f;

    private Vector3 baslangicKonumu;

    private void Start()
    {
        baslangicKonumu = transform.localPosition;
    }

    private void Update()
    {
        if (!sarsintiAktif)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, baslangicKonumu, Time.deltaTime * 8f);
            return;
        }

        float x = (Mathf.PerlinNoise(Time.time * hiz, 0f) - 0.5f) * siddet;
        float y = (Mathf.PerlinNoise(0f, Time.time * hiz) - 0.5f) * siddet;
        transform.localPosition = baslangicKonumu + new Vector3(x, y, 0f);
    }
}
