using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;



public class HoverHighlight : MonoBehaviour
{
    public static List<int> AllowedOctaves = new List<int>(); // Ottave attive globali

    public Material defaultMaterial;
    public Material hoverMaterial;
    public Material disabledMaterial;

    private MeshRenderer meshRenderer;
    private int myOctave;
    private bool isEnabled = true;

    void Awake()
    {
        meshRenderer = transform.Find("Key")?.GetComponent<MeshRenderer>();

        // Calcola la mia ottava leggendo il nome del parent (es: "Octave_4")
        Transform parent = transform.parent;
        if (parent != null && parent.name.StartsWith("Octave_"))
            int.TryParse(parent.name.Replace("Octave_", ""), out myOctave);

        UpdateKeyState(); // Appena instanziato → si adatta da solo
    }

    public void UpdateKeyState()
    {
        isEnabled = AllowedOctaves.Contains(myOctave);

        // Cambia materiale
        meshRenderer.material = isEnabled ? defaultMaterial : disabledMaterial;

        // Disattiva collider
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = isEnabled;

        // Disattiva interazione XR
        XRBaseInteractable xr = GetComponent<XRBaseInteractable>();
        if (xr != null) xr.enabled = isEnabled;
    }

    public void OnHoverEnter(){
       OnHover(true); 
    }

    public void OnHoverExit(){
        OnHover(false); 
    }

    public void DisableKey(){
        meshRenderer.material = disabledMaterial;
    }

    private void OnHover(bool active){
        if (GameManager.Instance.GetInPlay() && isEnabled){
            print(hoverMaterial);
            print("PROVE");
            print(active);
            if (active) meshRenderer.material = hoverMaterial;
            else meshRenderer.material = defaultMaterial;
            // Attiva il testo della nota (figlio)
            Transform noteText = transform.Find("NameNoteText");
            if (noteText != null)
                noteText.gameObject.SetActive(active);
        }
    }

    public int GetOctave() => myOctave;
    public bool IsEnabled() => isEnabled;

    private Coroutine pulseRoutine;

    public void Pulse(float duration = 2f, float frequency = 0.3f)
    {
        if (pulseRoutine != null)
            StopCoroutine(pulseRoutine);

        pulseRoutine = StartCoroutine(PulseRoutine(duration, frequency));
    }

    private IEnumerator PulseRoutine(float duration, float frequency)
    {
        float timer = 0f;
        while (timer < duration)
        {
            if (!isEnabled) yield break;

            meshRenderer.material = hoverMaterial;
            yield return new WaitForSeconds(frequency);

            meshRenderer.material = defaultMaterial;
            yield return new WaitForSeconds(frequency);

            timer += frequency * 2;
        }

        meshRenderer.material = defaultMaterial;
    }

}
