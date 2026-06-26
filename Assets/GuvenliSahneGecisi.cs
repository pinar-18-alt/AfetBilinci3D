using UnityEngine;

public class GuvenliSahneGecisi : MonoBehaviour
{
    public string hedefSahneAdi = "DisariYolSecimiSahnesi";

    public void SahneyeGec()
    {
        if (OyunYonetici.info != null)
        {
            OyunYonetici.info.SahneyeGec(hedefSahneAdi);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(hedefSahneAdi);
        }
    }
}
