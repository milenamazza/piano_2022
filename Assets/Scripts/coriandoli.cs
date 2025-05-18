using UnityEngine;

public class CelebrationManager : MonoBehaviour
{
    public ParticleSystem confetti;

    public void PlayConfetti()
    {
        if (confetti != null)
            confetti.Play();
    }
}

