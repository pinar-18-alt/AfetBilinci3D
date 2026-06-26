using UnityEngine;

public class OyuncuKontrol : MonoBehaviour
{
    public float hareketHizi = 3.2f;
    public float kosmaHizi = 5.2f;
    public float donusHizi = 150f;

    private CharacterController karakterKontrol;
    private Animator animator;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IdleState = Animator.StringToHash("Base Layer.idle");
    private static readonly int MoveState = Animator.StringToHash("Base Layer.move");
    private bool hareketEdiyordu;

    private void Awake()
    {
        karakterKontrol = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>(true);

        if (animator != null)
        {
            animator.applyRootMotion = false;
        }
    }

    private void Update()
    {
        float ileriGeri = Input.GetAxis("Vertical");
        float sagSol = Input.GetAxis("Horizontal");
        bool kosuyor = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float aktifHiz = kosuyor && ileriGeri > 0.05f ? kosmaHizi : hareketHizi;

        transform.Rotate(0f, sagSol * donusHizi * Time.deltaTime, 0f);

        Vector3 hareket = transform.forward * ileriGeri * aktifHiz;
        if (karakterKontrol != null)
        {
            hareket.y = -2f;
            karakterKontrol.Move(hareket * Time.deltaTime);
        }
        else
        {
            transform.Translate(hareket * Time.deltaTime, Space.World);
        }

        if (animator != null)
        {
            bool hareketEdiyor = Mathf.Abs(ileriGeri) > 0.05f ||
                                 Mathf.Abs(sagSol) > 0.05f;
            animator.SetFloat(Speed, Mathf.Abs(ileriGeri) > 0.05f ? (kosuyor ? 1.35f : 1f) : 0.65f);

            if (hareketEdiyor != hareketEdiyordu)
            {
                animator.CrossFade(hareketEdiyor ? MoveState : IdleState, 0.12f);
                hareketEdiyordu = hareketEdiyor;
            }
        }
    }
}
