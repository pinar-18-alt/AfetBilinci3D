using TMPro;
using UnityEngine;

public class CokKapanTutunGorevi : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI yonergeYazisi;
    public TextMeshProUGUI bilgiYazisi;

    [Header("Gorev")]
    public float gerekliSure = 3f;
    public GameObject tamamlanincaAktifOlacak;
    public GameObject tamamlanincaKapanacak;
    public GameObject tamamlanincaKapanacakVurgu;
    public YumusakKameraSarsintisi kameraSarsintisi;

    private void Start()
    {
        if (tamamlanincaAktifOlacak != null)
        {
            tamamlanincaAktifOlacak.SetActive(false);
        }

        if (kameraSarsintisi != null)
        {
            kameraSarsintisi.sarsintiAktif = true;
        }

        YaziYaz(bilgiYazisi, "Sarsıntı başladı. Güvenli masaya yaklaş.");
    }

    private bool oyuncuAlanda;
    private bool tamamlandi;
    private float sayac;
    private Transform karakterGorseli;
    private Vector3 normalYerelKonum;
    private Quaternion normalYerelDonus;
    private Vector3 normalYerelOlcek;

    private void Update()
    {
        if (!oyuncuAlanda || tamamlandi)
        {
            return;
        }

        if (Input.GetKey(KeyCode.E))
        {
            sayac += Time.deltaTime;
            float oran = Mathf.Clamp01(sayac / gerekliSure);
            KorunmaPozu(oran);
            YaziYaz(yonergeYazisi, "E tuşunu basılı tut: " + Mathf.RoundToInt(oran * 100f) + "%");

            if (sayac >= gerekliSure)
            {
                GoreviTamamla();
            }
        }
        else
        {
            sayac = 0f;
            KorunmaPozu(0f);
            YaziYaz(yonergeYazisi, "E tuşunu basılı tut: Çök-Kapan-Tutun");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!OyuncuMu(other) || tamamlandi)
        {
            return;
        }

        oyuncuAlanda = true;
        Animator animator = other.GetComponentInChildren<Animator>(true);
        if (animator != null)
        {
            karakterGorseli = animator.transform;
            normalYerelKonum = karakterGorseli.localPosition;
            normalYerelDonus = karakterGorseli.localRotation;
            normalYerelOlcek = karakterGorseli.localScale;
        }
        sayac = 0f;
        YaziYaz(yonergeYazisi, "E tuşunu basılı tut: Çök-Kapan-Tutun");
        YaziYaz(bilgiYazisi, "Masanın yanında kal. Sakin ve güvendesin.");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!OyuncuMu(other))
        {
            return;
        }

        oyuncuAlanda = false;
        sayac = 0f;
        KorunmaPozu(0f);

        if (!tamamlandi)
        {
            YaziYaz(yonergeYazisi, "");
            YaziYaz(bilgiYazisi, "Masanın yanına gel.");
        }
    }

    private void GoreviTamamla()
    {
        tamamlandi = true;
        YaziYaz(yonergeYazisi, "");
        YaziYaz(bilgiYazisi, "Harika! Şimdi çıkışa ilerle.");

        if (kameraSarsintisi != null)
        {
            kameraSarsintisi.sarsintiAktif = false;
        }

        if (OyunYonetici.info != null)
        {
            OyunYonetici.info.GorevTamamlandi("Çök-Kapan-Tutun tamamlandı.");
        }

        if (tamamlanincaAktifOlacak != null)
        {
            tamamlanincaAktifOlacak.SetActive(true);
        }

        if (tamamlanincaKapanacak != null)
        {
            tamamlanincaKapanacak.SetActive(false);
        }

        if (tamamlanincaKapanacakVurgu != null)
        {
            tamamlanincaKapanacakVurgu.SetActive(false);
        }

        KorunmaPozu(0f);
    }

    private bool OyuncuMu(Collider other)
    {
        return other.CompareTag("Player") || other.gameObject.name == "Oyuncu";
    }

    private void YaziYaz(TextMeshProUGUI hedef, string metin)
    {
        if (hedef != null)
        {
            hedef.text = metin;
        }
    }

    private void KorunmaPozu(float oran)
    {
        if (karakterGorseli == null)
        {
            return;
        }

        float yumusakOran = Mathf.SmoothStep(0f, 1f, oran);
        // Karakter kokunu zeminin altina itmeden korunma pozu verir.
        Vector3 hedefKonum = normalYerelKonum + new Vector3(0f, 0.03f, 0.12f);
        Quaternion hedefDonus = normalYerelDonus * Quaternion.Euler(32f, 0f, 0f);
        Vector3 hedefOlcek = Vector3.Scale(
            normalYerelOlcek,
            new Vector3(1.04f, 0.78f, 1.04f));

        karakterGorseli.localPosition = Vector3.Lerp(normalYerelKonum, hedefKonum, yumusakOran);
        karakterGorseli.localRotation = Quaternion.Slerp(normalYerelDonus, hedefDonus, yumusakOran);
        karakterGorseli.localScale = Vector3.Lerp(normalYerelOlcek, hedefOlcek, yumusakOran);
    }
}
