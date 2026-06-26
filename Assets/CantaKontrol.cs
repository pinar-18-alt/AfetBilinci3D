using UnityEngine;
using TMPro;

public class CantaKontrol : MonoBehaviour
{
    [Header("UI Bilesenleri")]
    public TextMeshProUGUI durumYazisi;
    public GameObject tahliyeButonu;

    [Header("Gorev Ayarlari")]
    public int toplamGerekliEsya = 2;
    public CantaListesiUI listeUI;

    private int toplananDogruEsyaSayisi = 0;

    private void Start()
    {
        if (tahliyeButonu != null)
        {
            tahliyeButonu.SetActive(false);
        }

        YaziYaz("Deprem çantanı hazırla. Eşyaya yaklaş ve E tuşuna bas.");
    }

    public void DogruEsyaTopla(GameObject eşya)
    {
        DogruEsyaTopla(eşya, null, null);
    }

    public void DogruEsyaTopla(GameObject eşya, string esyaId)
    {
        DogruEsyaTopla(eşya, esyaId, null);
    }

    public void DogruEsyaTopla(GameObject eşya, string esyaId, string aciklamaMetni)
    {
        if (eşya != null)
        {
            Destroy(eşya);
        }

        toplananDogruEsyaSayisi++;

        if (OyunYonetici.info != null)
        {
            OyunYonetici.info.GorevTamamlandi("Çantaya doğru eşya eklendi.");
        }

        if (listeUI == null)
        {
            listeUI = FindFirstObjectByType<CantaListesiUI>();
        }

        if (listeUI != null)
        {
            listeUI.EsyaToplandi(esyaId);
        }

        if (toplananDogruEsyaSayisi >= toplamGerekliEsya)
        {
            YaziYaz(AciklamayiBirlestir(aciklamaMetni, "Çantan hazır! Güvenli odaya geçebilirsin."));

            if (tahliyeButonu != null)
            {
                tahliyeButonu.SetActive(true);
            }
        }
        else
        {
            int kalan = toplamGerekliEsya - toplananDogruEsyaSayisi;
            YaziYaz(AciklamayiBirlestir(aciklamaMetni, kalan + " eşya daha gerekiyor."));
        }
    }

    public void YanlisEsyaSecildi(string mesaj)
    {
        YaziYaz(mesaj);

        if (OyunYonetici.info != null)
        {
            OyunYonetici.info.PuanDegistir(-10);
        }
    }

    public void SonrakiSahneyeGec()
    {
        if (OyunYonetici.info != null)
        {
            OyunYonetici.info.SahneyeGec("SampleScene");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }
    }

    private void YaziYaz(string metin)
    {
        if (durumYazisi != null)
        {
            durumYazisi.text = metin;
        }
    }

    private string AciklamayiBirlestir(string aciklamaMetni, string devamMetni)
    {
        if (string.IsNullOrWhiteSpace(aciklamaMetni))
        {
            return "Güzel seçim! " + devamMetni;
        }

        return aciklamaMetni + "\n" + devamMetni;
    }
}




