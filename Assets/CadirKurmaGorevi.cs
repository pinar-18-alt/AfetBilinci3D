using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CadirKurmaGorevi : MonoBehaviour
{
    public TextMeshProUGUI durumYazisi;
    public GameObject cadirGorseli;
    public float gerekliSure = 2.2f;
    public string sonrakiSahne = "SkorEkrani";

    private bool oyuncuAlanda;
    private bool tamamlandi;
    private float sayac;
    private Vector3 cadirHedefOlcek = Vector3.one;

    private void Start()
    {
        if (cadirGorseli != null)
        {
            cadirHedefOlcek = cadirGorseli.transform.localScale;
            cadirGorseli.transform.localScale = cadirHedefOlcek * 0.08f;
            cadirGorseli.SetActive(false);
        }
    }

    private void Update()
    {
        if (!oyuncuAlanda || tamamlandi)
        {
            return;
        }

        if (Input.GetKey(KeyCode.E))
        {
            sayac += Time.deltaTime;

            if (cadirGorseli != null)
            {
                cadirGorseli.SetActive(true);
                float oran = Mathf.Clamp01(sayac / gerekliSure);
                cadirGorseli.transform.localScale =
                    Vector3.Lerp(cadirHedefOlcek * 0.08f, cadirHedefOlcek, oran);
            }

            YaziYaz("Cadir kuruluyor...");

            if (sayac >= gerekliSure)
            {
                Tamamla();
            }
        }
        else if (sayac > 0f)
        {
            sayac = Mathf.Max(0f, sayac - Time.deltaTime * 1.5f);
            YaziYaz("E tusuna basili tut: Cadiri kur");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!OyuncuMu(other) || tamamlandi)
        {
            return;
        }

        oyuncuAlanda = true;
        YaziYaz("E tusuna basili tut: Cadiri kur");
    }

    private void OnTriggerExit(Collider other)
    {
        if (OyuncuMu(other) && !tamamlandi)
        {
            oyuncuAlanda = false;
            sayac = 0f;
            YaziYaz("Guvenli alana don.");
        }
    }

    private void Tamamla()
    {
        tamamlandi = true;
        YaziYaz("Harika! Cadir hazir.");

        if (OyunYonetici.info != null)
        {
            OyunYonetici.info.GorevTamamlandi("Guvenli alanda cadir kuruldu.");
            MiniSinavAkisi.Baslat(sonrakiSahne);
        }
        else
        {
            MiniSinavAkisi.Baslat(sonrakiSahne);
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
