using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkorKontrol : MonoBehaviour
{
    [Header("UI Metin Alanlari")]
    public TextMeshProUGUI tebrikYazisi;
    public TextMeshProUGUI isimMetni;
    public TextMeshProUGUI puanMetni;
    public TextMeshProUGUI sureMetni;

    private void Start()
    {
        if (OyunYonetici.info == null)
        {
            return;
        }

        OyunYonetici.info.OyunuBitir();

        if (tebrikYazisi != null)
        {
            tebrikYazisi.text = "Harika iş!\nGüvenli alanı buldun.";
        }

        if (isimMetni != null)
        {
            isimMetni.text = "Oyuncu: " + OyunYonetici.info.oyuncuAdi;
        }

        if (puanMetni != null)
        {
            puanMetni.text = "Puan: " + OyunYonetici.info.oyuncuPuani + " / 100";
        }

        if (sureMetni != null)
        {
            int sonSaniye = Mathf.RoundToInt(OyunYonetici.info.gecenSure);
            sureMetni.text = "Süre: " + sonSaniye + " sn  |  Görev: " + OyunYonetici.info.tamamlananGorevSayisi;
        }
    }

    public void YenidenOynaButonu()
    {
        if (OyunYonetici.info != null)
        {
            Destroy(OyunYonetici.info.gameObject);
        }

        EkranKarartma karartma = Object.FindFirstObjectByType<EkranKarartma>();
        if (karartma != null)
        {
            karartma.YeniSahneyeGec("GirisEkrani");
        }
        else
        {
            SceneManager.LoadScene("GirisEkrani");
        }
    }
}
