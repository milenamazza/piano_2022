using System.Collections.Generic;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

// Questo componente gestisce la riproduzione audio delle note
public class NotePlayer : MonoBehaviour
{
    private AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null){
            Debug.Log("Nessun AudioSource trovato sul GameObject! Lo aggiungo.");
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private string GetNoteNameFromKey(GameObject noteKey)
    {
        Transform parent = noteKey.transform.parent;
        string baseName = noteKey.name.Replace("Key_", "").Replace("_Black", "");
        string octave = parent.name.Replace("Octave_", "");

        if (noteKey.name.Contains("_Black"))
            baseName += "Sharp";

        return baseName + octave;
    }

    private void PlayNoteString(string noteName)
    {
        if (GameManager.Instance.GetInPlay()) {
            string path = $"Notes/{noteName}";
            AudioClip clip = Resources.Load<AudioClip>(path);

            if (clip != null && audioSource != null){
                audioSource.PlayOneShot(clip);
            } else {
                Debug.Log($"Clip '{noteName}' non trovato o AudioSource mancante");
            }
        }
    }

    public void PlayNoteKey(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
    {
        GameObject noteKey = args.interactableObject.transform.gameObject;
        string noteName = GetNoteNameFromKey(noteKey);
        PlayNoteString(noteName);
        GameManager.Instance.RegisterPlayedNote(noteName);
    }


    public void PlayCurrnetNote()
    {
        string noteName = GameManager.Instance.GetCurrentNote();
        Debug.Log(noteName);
        PlayNoteString(noteName);
    }

    // 🔊 Inizia a suonare la nota (finché non viene rilasciata)
    public void StartNote(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
    {
        if (!GameManager.Instance.GetInPlay()) return;

        GameObject noteKey = args.interactableObject.transform.gameObject;
        string noteName = GetNoteNameFromKey(noteKey);
        string path = $"Notes/{noteName}";
        AudioClip clip = Resources.Load<AudioClip>(path);

        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.loop = false;
            audioSource.Play();
            GameManager.Instance.RegisterPlayedNote(noteName);
        }
        else
        {
            Debug.LogWarning($"Clip '{noteName}' non trovato.");
        }
    }



    // 🛑 Ferma la riproduzione con sfumatura
    public void StopNote()
{
    if (fadeOutRoutine != null)
        StopCoroutine(fadeOutRoutine);

    fadeOutRoutine = StartCoroutine(FadeOutAndStop(0.2f)); // durata fade-out in secondi
}

    private Coroutine fadeOutRoutine;

    private IEnumerator FadeOutAndStop(float duration)
    {
        if (audioSource == null || !audioSource.isPlaying)
            yield break;

        float startVolume = audioSource.volume;

        float time = 0f;
        while (time < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
        audioSource.volume = startVolume; // reset per il prossimo suono
    }


    
}
