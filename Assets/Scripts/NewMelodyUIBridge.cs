using UnityEngine;

public class MelodyUIBridge : MonoBehaviour
{
    public void PlayOriginal()
    {
        GameManager.Instance?.PlayMelodiaOriginale();
    }

    public void PlayUserMelody()
    {
        GameManager.Instance?.PlayMelodiaUtente();
    }

    public void ResetMelody()
    {
        GameManager.Instance?.ResetMelodia();
    }
}

