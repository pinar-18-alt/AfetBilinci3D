using UnityEngine;

public class BasitKameraTakip : MonoBehaviour
{
    public Transform hedef;
    public Vector3 mesafe = new Vector3(0f, 2.4f, -4.8f);
    public float takipHizi = 8f;
    public float bakisYuksekligi = 1.1f;

    private void LateUpdate()
    {
        if (hedef == null)
        {
            return;
        }

        Vector3 hedefKonum = hedef.TransformPoint(mesafe);
        transform.position = Vector3.Lerp(transform.position, hedefKonum, takipHizi * Time.deltaTime);

        Vector3 bakisNoktasi = hedef.position + Vector3.up * bakisYuksekligi;
        Quaternion hedefDonus = Quaternion.LookRotation(bakisNoktasi - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, hedefDonus, takipHizi * Time.deltaTime);
    }
}
