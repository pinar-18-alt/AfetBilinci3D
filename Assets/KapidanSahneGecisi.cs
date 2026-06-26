using UnityEngine;
using UnityEngine.SceneManagement;

public class KapidanSahneGecisi : MonoBehaviour
{
    public string hedefSahneAdi = "ApartmanKoridoruSahnesi";

    private bool geciliyor;

    private void OnTriggerEnter(Collider other)
    {
        if (geciliyor || !OyuncuMu(other))
        {
            return;
        }

        geciliyor = true;
        if (OyunYonetici.info != null)
        {
            OyunYonetici.info.SahneyeGec(hedefSahneAdi);
        }
        else
        {
            SceneManager.LoadScene(hedefSahneAdi);
        }
    }

    private bool OyuncuMu(Collider other)
    {
        return other.CompareTag("Player") || other.gameObject.name == "Oyuncu";
    }
}
