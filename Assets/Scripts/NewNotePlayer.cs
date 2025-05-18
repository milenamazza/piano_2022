using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Questo componente gestisce la riproduzione audio delle note
public class NotePlayer : MonoBehaviour
{
    // Riferimento all'AudioSource per riprodurre i suoni
    private AudioSource audioSource; 
    
    // Metodo chiamato all'avvio del GameObject
    public void Start()
    {
        // Prova a recuperare un AudioSource già presente sul GameObject
        audioSource = GetComponent<AudioSource>();

        // Se non esiste, lo aggiunge dinamicamente e mostra un warning
        if (audioSource == null){
            Debug.LogWarning("Nessun AudioSource trovato sul GameObject! Lo aggiungo.");
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    /// <summary>
    /// Riproduce un file audio associato al nome della nota passato come stringa
    /// </summary>
    /// <param name="noteName">Il nome della nota (es. "C4", "DSharp3")</param>
    private void PlayNoteString(string noteName){
        // Riproduce solo se il gioco è in modalità "inPlay"
        if (GameManager.Instance.GetInPlay()) {
            // Costruisce il percorso della clip dentro Resources/Notes/
            string path = $"Notes/{noteName}";

            // Carica la clip audio dal path
            AudioClip clip = Resources.Load<AudioClip>(path);

            if (clip != null) {
                // Se l'AudioSource è presente, riproduce la nota
                if (audioSource != null){
                    audioSource.PlayOneShot(clip);
                } 
                else Debug.Log("AudioSource non assegnato!");
            }
            else Debug.Log($"Clip '{noteName}' non trovato");
        }
    }

    /// <summary>
    /// Ricava il nome della nota da un GameObject (tasto del piano) e la riproduce
    /// </summary>
    /// <param name="noteKey">Il GameObject del tasto premuto</param>
    public void PlayNoteKey(GameObject noteKey ){        
        // Il nome dell'ottava è preso dal nome del genitore (es. "Octave_3")
        Transform parent = noteKey.transform.parent;

        // Ricava il nome base della nota (es. "C", "D", rimuovendo "Key_" e "_Black")
        string baseName = noteKey.name.Replace("Key_", "").Replace("_Black", "");

        // Ricava l'ottava (numero) dal nome del genitore
        string octave = parent.name.Replace("Octave_", "");

        // Se è un tasto nero, aggiunge "Sharp" (es. C -> CSharp)
        if (noteKey.name.Contains("_Black"))
            baseName += "Sharp";

        // Costruisce il nome completo della nota (es. "C4", "FSharp2")
        string noteName = baseName + octave;

        // Riproduce la nota
        PlayNoteString(noteName); //suona la nota

        GameManager.Instance.RegisterPlayedNote(noteName);

    }

    /// <summary>
    /// Riproduce la nota corrente da indovinare, presa dal GameManager
    /// </summary>
    public void PlayCurrnetNote(){ 
        string noteName = GameManager.Instance.GetCurrentNote();
        Debug.Log(noteName); // debug del nome della nota
        PlayNoteString(noteName);
    }
}
