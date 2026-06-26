using TMPro;
using UnityEngine;

public class TehlikeliAlanUyarisi : MonoBehaviour
{
    public TextMeshProUGUI uyariYazisi;
    public string mesaj = "Burası tehlikeli. Devrilebilecek eşyalardan uzaklaş.";

    private void OnTriggerEnter(Collider other)
    {
        if (OyuncuMu(other) && uyariYazisi != null)
        {
            uyariYazisi.text = mesaj;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (OyuncuMu(other) && uyariYazisi != null)
        {
            uyariYazisi.text = "Laptop masasının yanına git.";
        }
    }

    private bool OyuncuMu(Collider other)
    {
        return other.CompareTag("Player") || other.gameObject.name == "Oyuncu";
    }
}
