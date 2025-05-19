using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Key : MonoBehaviour
{
    public AudioClip sound;
    public Animator animator;
    public AudioSource audioSource;
    public string keyName;
    public GameObject child1;

    public static string LastPressedKey;

    // Per colore evidenziato
    public Material defaultMat;
    public Material highlightMat;
    private Renderer rend;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        audioSource.clip = sound;

        try
        {
            child1 = transform.GetChild(1).gameObject;
            rend = child1.GetComponent<Renderer>();
            if (rend != null && defaultMat != null)
                rend.material = defaultMat;
        }
        catch (Exception e) {
            Debug.LogWarning(e);
        }
    }

    public void PlaySound()
    {
        audioSource.volume = 1;
        audioSource.Play();

        animator.SetBool("Pressed", true);
        StopAllCoroutines();
        SetLastPressedKey();
    }

    public void StopSound()
    {
        animator.SetBool("Pressed", false);
        StartCoroutine(DecreaseVolume());
    }

    IEnumerator DecreaseVolume()
    {
        while (audioSource.volume > 0.01f)
        {
            audioSource.volume -= 0.02f;
            yield return null;
        }
    }

    private void SetLastPressedKey()
    {
        LastPressedKey = keyName;
        Debug.Log(LastPressedKey);
    }

    // 🔶 Colore evidenziato
    public void Highlight()
    {
        if (rend != null && highlightMat != null)
            rend.material = highlightMat;
    }

    public void ResetColor()
    {
        Debug.Log("Azione");
        if (rend != null && defaultMat != null)
            rend.material = defaultMat;
    }
}
