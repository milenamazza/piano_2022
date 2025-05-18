using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMelodiaManager
{
    private GameObject screen;
    private List<Note> playerMelody = new List<Note>();
    private AudioSource audioSource;

    private Note currentCorrectNote => GameManager.Instance.GetCurrentNoteObj();

    public void InitManager(GameObject screen)
    {
        this.screen = screen;
        audioSource = GameManager.Instance.GetComponent<AudioSource>();
    }

    public void Win()
    {
        screen.SetActive(false);
    }

    public void ResetMelody()
    {
        playerMelody.Clear();
    }

    // Chiamato quando si conferma una nota
    public void OnConfirm(string playedNote)
    {
        if (playedNote == currentCorrectNote.ToString())
        {
            playerMelody.Add(currentCorrectNote);
            GameManager.Instance.PlayFeedbackSound(true);
            GameManager.Instance.AdvanceNote();
        }
        else
        {
            GameManager.Instance.PlayFeedbackSound(false);
        }
    }

    public void PlayOriginalMelody()
    {
        var melody = GameManager.Instance.noteManager.GenerateTimedMelody(GameManager.Instance.GetMelody());
        GameManager.Instance.StartCoroutine(PlayTimedMelodyCoroutine(melody));
    }

    public void PlayPlayerMelody()
    {
        var fullMelody = GameManager.Instance.noteManager.GenerateTimedMelody(GameManager.Instance.GetMelody());
        int count = Mathf.Min(playerMelody.Count, fullMelody.Count);
        var partialMelody = fullMelody.GetRange(0, count);

        GameManager.Instance.StartCoroutine(PlayTimedMelodyCoroutine(partialMelody));
    }

    private IEnumerator PlayTimedMelodyCoroutine(List<TimedNote> melody)
    {
        foreach (var timed in melody)
        {
            string clipPath = $"Notes/{timed.note}";
            AudioClip clip = Resources.Load<AudioClip>(clipPath);

            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
                yield return new WaitForSeconds(timed.duration);
                audioSource.Stop();
            }
        }
    }
}
