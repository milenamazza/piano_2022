using UnityEngine;

// QUI SI PUÒ GESTIRE MEGLIO PERCHÉ SE I DUE CURSORI SI SOVRAPPONGONO SI ROMPE
// Gestisce il cambio di materiale (colore) visivo dei pulsanti in base allo stato:
// - Hover (quando il cursore li sfiora)
// - Selezionato
// - Default (normale)
public class Btn : MonoBehaviour
{
    // Materiali assegnabili da Inspector per i vari stati del bottone
    private Material defaultMaterial, hoverMaterial, selectedMaterial;
    private Color gray = new Color(	0.13f, 0.18f, 0.18f);

    void Awake()
    {
        // Carica i materiali dalla cartella Resources/Materials
        defaultMaterial = Resources.Load<Material>("Materials/btnDeselect");
        hoverMaterial = Resources.Load<Material>("Materials/btnHover");
        selectedMaterial = Resources.Load<Material>("Materials/btnSelect");
        
        //NB attiva i bordi degli elementi di default (ora impostati in UX)
    }

    /// <summary>
    /// Quando il cursore entra/esce dal pulsante, cambia materiale in hover/default
    /// </summary>
    public void ButtonHover(bool enter)
    {
        Transform quad = gameObject.transform.Find("QuadBackground");
        if (quad == null) return;

        MeshRenderer renderer = quad.GetComponent<MeshRenderer>();
        if (renderer != null)
         renderer.material = enter ? hoverMaterial : defaultMaterial;
    }

    /// <summary>
    /// Evidenzia questo bottone come selezionato e rimuove la selezione dagli altri fratelli
    /// </summary>
    public void ButtonSelect(){
        Transform parent = gameObject.transform.parent;
        if (parent == null) return;

        foreach (Transform sibling in parent){
            Transform border = sibling.Find("border");
            if (border != null)
                border.gameObject.SetActive(false);
        }

        Transform myBorder = transform.Find("border");
        if (myBorder != null)
            myBorder.gameObject.SetActive(true);
    }

    //colora il bottone con SELECTMATERIAL (usare per hover)
    public void ButtonColorHover(bool enter)
    {
        Transform quad = gameObject.transform.Find("QuadBackground");
        if (quad == null) return;

        MeshRenderer renderer = quad.GetComponent<MeshRenderer>();
        if (renderer != null)
         renderer.material = enter ? selectedMaterial : defaultMaterial;
    }

    public void HideButton(){
        VisibilityButton(false);
    }

    //per ascolta nota
    public void ShowButton(){
        VisibilityButton(true);
    }

    private void VisibilityButton(bool visible){
        // Renderer
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>(visible);
        foreach (var r in renderers) r.enabled = visible;

        // Testi
        TextMesh[] texts = GetComponentsInChildren<TextMesh>(visible);
        foreach (var t in texts) t.gameObject.SetActive(visible);

        // Interazioni
        Collider[] colliders = GetComponentsInChildren<Collider>(visible);
        foreach (var c in colliders) c.enabled = visible;
    }
}
