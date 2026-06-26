using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class TahliyeKontrol : MonoBehaviour
{
    public TextMeshProUGUI uiMetni;
    public GameObject devamButonu;
    public bool dogruSecimdeOtomatikGec;
    public string hedefSahneAdi = "DisariYolSecimiSahnesi";

    private bool oyuncuAlanda;
    private bool secimYapildi;

    private void Start()
    {
        if (devamButonu != null)
        {
            devamButonu.SetActive(false);
        }
    }

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

        if (gameObject.name == "Asansor")
        {
            YaziYaz("E tuşuna bas: Asansörü kontrol et");
        }
        else if (gameObject.name == "Merdiven")
        {
            YaziYaz("E tuşuna bas: Merdiveni kullan");
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

    public void SonrakiSahneyeGec()
    {
        if (OyunYonetici.info != null)
        {
            OyunYonetici.info.SahneyeGec(hedefSahneAdi);
        }
        else
        {
            SceneManager.LoadScene(hedefSahneAdi);
        }
    }

    private void SecimiDegerlendir()
    {
        if (gameObject.name == "Asansor")
        {
            YaziYaz("Asansör güvenli değil. Merdivene git.");
            if (OyunYonetici.info != null)
            {
                OyunYonetici.info.PuanDegistir(-10);
            }
        }
        else if (gameObject.name == "Merdiven")
        {
            secimYapildi = true;
            YaziYaz("Doğru secim! Merdivenle ilerle.");

            if (OyunYonetici.info != null)
            {
                OyunYonetici.info.GorevTamamlandi("Merdiven seçildi.");
            }

            if (dogruSecimdeOtomatikGec)
            {
                StartCoroutine(OtomatikGec());
            }
            else
            {
                StartCoroutine(ButonuGecikmeliAc());
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

    private IEnumerator ButonuGecikmeliAc()
    {
        yield return new WaitForSeconds(1f);

        if (devamButonu != null)
        {
            devamButonu.SetActive(true);
        }
    }

    private IEnumerator OtomatikGec()
    {
        yield return new WaitForSeconds(1.2f);
        SonrakiSahneyeGec();
    }

    private void YaziYaz(string metin)
    {
        if (uiMetni != null)
        {
            uiMetni.text = metin;
        }
    }
}



