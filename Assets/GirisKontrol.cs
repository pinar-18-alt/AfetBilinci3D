using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GirisKontrol : MonoBehaviour
{
    [Header("Giris Bilesenleri")]
    public GameObject isimGiris;
    public TextMeshProUGUI karsilamaMetni;
    public Button baslaButonu;

    private TMP_InputField isimKutusu;

    private void Start()
    {
        if (isimGiris != null)
        {
            isimKutusu = isimGiris.GetComponent<TMP_InputField>();
        }

        if (karsilamaMetni != null)
        {
            karsilamaMetni.text = "Adını yaz, maceraya başlayalım.";
        }

        if (baslaButonu != null)
        {
            baslaButonu.gameObject.SetActive(false);
        }
    }

    public void IsimKontrolEt()
    {
        string oyuncuAdi = OyuncuAdiniAl();

        if (oyuncuAdi.Length > 1)
        {
            if (karsilamaMetni != null)
            {
                karsilamaMetni.text = "Merhaba " + oyuncuAdi + "! Hazırsan başlayalım.";
            }

            if (baslaButonu != null)
            {
                baslaButonu.gameObject.SetActive(true);
            }

            if (OyunYonetici.info != null)
            {
                OyunYonetici.info.oyuncuAdi = oyuncuAdi;
            }
        }
        else
        {
            if (karsilamaMetni != null)
            {
                karsilamaMetni.text = "Adını yaz, maceraya başlayalım.";
            }

            if (baslaButonu != null)
            {
                baslaButonu.gameObject.SetActive(false);
            }
        }
    }

    public void OyunaBasla()
    {
        string oyuncuAdi = OyuncuAdiniAl();
        if (oyuncuAdi.Length < 2)
        {
            if (karsilamaMetni != null)
            {
                karsilamaMetni.text = "Başlamak için adını yaz.";
            }
            return;
        }

        if (OyunYonetici.info != null)
        {
            OyunYonetici.info.OyunuBaslat(oyuncuAdi);
            OyunYonetici.info.SahneyeGec("DepremCantasiSahnesi");
        }
        else
        {
            SceneManager.LoadScene("DepremCantasiSahnesi");
        }
    }

    private string OyuncuAdiniAl()
    {
        if (isimKutusu == null && isimGiris != null)
        {
            isimKutusu = isimGiris.GetComponent<TMP_InputField>();
        }

        if (isimKutusu == null)
        {
            return "";
        }

        return isimKutusu.text.Trim();
    }
}
