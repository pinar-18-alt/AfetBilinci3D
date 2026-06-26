using TMPro;
using UnityEngine;

public class GorevEtkilesimi : MonoBehaviour
{
    public enum EtkilesimTuru
    {
        Bas,
        BasiliTut
    }

    public enum GorevSonucu
    {
        SadeceMesaj,
        DogruCantaEsyasi,
        YanlisCantaEsyasi,
        GuvenliTahliye,
        RiskliTahliye,
        GuvenliYol,
        RiskliYol
    }

    [Header("Gorev")]
    public EtkilesimTuru etkilesimTuru = EtkilesimTuru.Bas;
    public GorevSonucu gorevSonucu = GorevSonucu.SadeceMesaj;
    public string eylemMetni = "E tuşuna bas";
    public string basariliMesaj = "Harika!";
    public string uyariMesaji = "Bunu tekrar düşünelim.";
    public float basiliTutmaSuresi = 1.5f;
    public bool birKezTamamlansin = true;

    [Header("Sonuc")]
    public int puanDegisimi = 0;
    public bool nesneyiYokEt = false;
    public GameObject tamamlanincaAktifOlacak;
    public string sonrakiSahneAdi = "";

    [Header("UI")]
    public TextMeshProUGUI yonergeYazisi;
    public TextMeshProUGUI bilgiYazisi;

    private bool oyuncuAlanda;
    private bool tamamlandi;
    private float basiliTutmaSayaci;

    private void Update()
    {
        if (!oyuncuAlanda || tamamlandi)
        {
            return;
        }

        if (etkilesimTuru == EtkilesimTuru.Bas)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GoreviTamamla();
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.E))
            {
                basiliTutmaSayaci += Time.deltaTime;
                float kalanSure = Mathf.Max(0f, basiliTutmaSuresi - basiliTutmaSayaci);
                YaziYaz(yonergeYazisi, "E tuşunu basılı tut: " + kalanSure.ToString("0.0") + " sn");

                if (basiliTutmaSayaci >= basiliTutmaSuresi)
                {
                    GoreviTamamla();
                }
            }
            else
            {
                basiliTutmaSayaci = 0f;
                YaziYaz(yonergeYazisi, eylemMetni);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!OyuncuMu(other) || tamamlandi)
        {
            return;
        }

        oyuncuAlanda = true;
        basiliTutmaSayaci = 0f;
        YaziYaz(yonergeYazisi, eylemMetni);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!OyuncuMu(other))
        {
            return;
        }

        oyuncuAlanda = false;
        basiliTutmaSayaci = 0f;
        YaziYaz(yonergeYazisi, "");
    }

    private bool OyuncuMu(Collider other)
    {
        return other.CompareTag("Player") || other.gameObject.name == "Oyuncu";
    }

    private void GoreviTamamla()
    {
        if (birKezTamamlansin)
        {
            tamamlandi = true;
        }

        YaziYaz(yonergeYazisi, "");

        string mesaj = MesajSec();
        YaziYaz(bilgiYazisi, mesaj);

        if (OyunYonetici.info != null)
        {
            if (puanDegisimi != 0)
            {
                OyunYonetici.info.PuanDegistir(puanDegisimi);
            }

            if (gorevSonucu == GorevSonucu.DogruCantaEsyasi ||
                gorevSonucu == GorevSonucu.GuvenliTahliye ||
                gorevSonucu == GorevSonucu.GuvenliYol)
            {
                OyunYonetici.info.GorevTamamlandi(mesaj);
            }
        }

        if (tamamlanincaAktifOlacak != null)
        {
            tamamlanincaAktifOlacak.SetActive(true);
        }

        if (!string.IsNullOrWhiteSpace(sonrakiSahneAdi))
        {
            if (OyunYonetici.info != null)
            {
                OyunYonetici.info.SahneyeGec(sonrakiSahneAdi);
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sonrakiSahneAdi);
            }
        }

        if (nesneyiYokEt)
        {
            Destroy(gameObject);
        }
    }

    private string MesajSec()
    {
        switch (gorevSonucu)
        {
            case GorevSonucu.DogruCantaEsyasi:
                return string.IsNullOrWhiteSpace(basariliMesaj) ? "Güzel seçim! Çantaya ekledin." : basariliMesaj;
            case GorevSonucu.YanlisCantaEsyasi:
                return string.IsNullOrWhiteSpace(uyariMesaji) ? "Bu eşya öncelikli değil." : uyariMesaji;
            case GorevSonucu.GuvenliTahliye:
                return string.IsNullOrWhiteSpace(basariliMesaj) ? "Doğru yol! Merdivene yönel." : basariliMesaj;
            case GorevSonucu.RiskliTahliye:
                return string.IsNullOrWhiteSpace(uyariMesaji) ? "Asansör güvenli değil." : uyariMesaji;
            case GorevSonucu.GuvenliYol:
                return string.IsNullOrWhiteSpace(basariliMesaj) ? "Toplanma alanına ulaştın." : basariliMesaj;
            case GorevSonucu.RiskliYol:
                return string.IsNullOrWhiteSpace(uyariMesaji) ? "Bu yol riskli. Diğer yolu dene." : uyariMesaji;
            default:
                return string.IsNullOrWhiteSpace(basariliMesaj) ? "Tamamlandı." : basariliMesaj;
        }
    }

    private void YaziYaz(TextMeshProUGUI hedef, string metin)
    {
        if (hedef != null)
        {
            hedef.text = metin;
        }
    }
}


