using UnityEngine;  

public class PlayMelody : MonoBehaviour
{
     public AudioClip clip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        //audioSource.playOnAwake = false;
        if (clip != null){
            audioSource.clip = clip;
        }
    }

    public void OnHover(bool enter){
        if(enter) OnHoverEnter();
        else OnHoverExit();
    }

    private void OnHoverEnter(){
        if (audioSource != null && !audioSource.isPlaying){
            audioSource.Play();
        }
    }

    private void OnHoverExit(){
        if (audioSource != null && audioSource.isPlaying){
            audioSource.Stop();
        }
    }
}
