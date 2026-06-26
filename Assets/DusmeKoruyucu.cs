using UnityEngine;

public class DusmeKoruyucu : MonoBehaviour
{
    public float minimumY = -1.5f;

    private Vector3 guvenliKonum;
    private CharacterController controller;

    private void Awake()
    {
        guvenliKonum = transform.position;
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (transform.position.y >= minimumY)
        {
            return;
        }

        if (controller != null)
        {
            controller.enabled = false;
        }

        transform.position = guvenliKonum;

        if (controller != null)
        {
            controller.enabled = true;
        }
    }
}
