using UnityEngine;

public class YukariAsagiHareket : MonoBehaviour
{
    public float mesafe = 0.25f;
    public float hiz = 1.6f;
    public bool kamerayaBak = true;

    private Vector3 baslangicKonumu;

    private void Awake()
    {
        baslangicKonumu = transform.position;
    }

    private void LateUpdate()
    {
        float y = Mathf.Sin(Time.time * hiz) * mesafe;
        transform.position = baslangicKonumu + Vector3.up * y;

        if (!kamerayaBak || Camera.main == null)
        {
            return;
        }

        Vector3 hedef = transform.position - Camera.main.transform.position;
        hedef.y = 0f;
        if (hedef.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(hedef);
        }
    }
}
