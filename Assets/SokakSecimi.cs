using UnityEngine;
using TMPro;

public class SokakSecimi : MonoBehaviour
{
    [Header("UI Bileseni")]
    public TextMeshProUGUI durumYazisi;

    private bool oyuncuAlanda;
    private bool secimYapildi;

    private void Update()
    {
        if (oyuncuAlanda && !secimYapildi && Input.GetKeyDown(KeyCode.E))
        {
            SecimiDegerlendir();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!OyuncuMu(other))
        {
            return;
        }

        oyuncuAlanda = true;
        HiziKes(other);

        if (gameObject.name == "Yol_Guvenli")
        {
            YaziYaz("E tuşuna bas: Toplanma alanına git");
        }
        else if (gameObject.name == "Yol_Tehlikeli")
        {
            YaziYaz("Elektrik direkleri ve yüksek binaların olduğu yollar tehlikelidir.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (OyuncuMu(other))
        {
            oyuncuAlanda = false;
            if (!secimYapildi)
            {
                YaziYaz("");
            }
        }
    }

    private void SecimiDegerlendir()
    {
        if (gameObject.name == "Yol_Guvenli")
        {
            secimYapildi = true;
            YaziYaz("Harika! Toplanma alanına ulaştın.");

            if (OyunYonetici.info != null)
            {
                OyunYonetici.info.GorevTamamlandi("Güvenli toplanma alanına ulaşıldı.");
                OyunYonetici.info.SahneyeGec("SkorEkrani");
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("SkorEkrani");
            }
        }
        else if (gameObject.name == "Yol_Tehlikeli")
        {
            YaziYaz("Elektrik direkleri ve yüksek binaların olduğu yollar tehlikelidir.");

            if (OyunYonetici.info != null)
            {
                OyunYonetici.info.PuanDegistir(-10);
            }
        }
    }

    private void HiziKes(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    private bool OyuncuMu(Collider other)
    {
        return other.CompareTag("Player") || other.gameObject.name == "Oyuncu";
    }

    private void YaziYaz(string metin)
    {
        if (durumYazisi != null)
        {
            durumYazisi.text = metin;
        }
    }
}


