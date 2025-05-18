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
        defaultMaterial = Resources.Load<Material>("Materials/btnDefault");
        hoverMaterial = Resources.Load<Material>("Materials/btnHover");
        selectedMaterial = Resources.Load<Material>("Materials/btnSelect");

        // ✅ Applica il materiale selezionato visivamente al QuadBackground
        Transform quad = transform.Find("QuadBackground");
        if (quad != null)
        {
            MeshRenderer renderer = quad.GetComponent<MeshRenderer>();
            if (renderer != null && selectedMaterial != null) {
                if (gameObject.name.Contains("Easy") || gameObject.name.Contains("Hard")){
                    renderer.material = GetMaterialMode(gameObject.name);
                }
                else
                    renderer.material = defaultMaterial;  
            }
        }
    }

    /// <summary>
    /// Quando il cursore entra sul pulsante, cambia materiale in hover
    /// </summary>
    public void ButtonMaterialToHoverEnter(){
            // Cerca il GameObject "QuadBackground" dentro il bottone
            Transform quad = gameObject.transform.Find("QuadBackground");
            if (quad == null) return;

            MeshRenderer renderer = quad.GetComponent<MeshRenderer>();
            if (renderer != null) renderer.material = hoverMaterial;
    }

    /// <summary>
    /// Quando il cursore esce dal pulsante, ripristina il materiale
    /// </summary>
    public void ButtonMaterialToHoverExit(){
            Transform quad = gameObject.transform.Find("QuadBackground");
            if (quad == null) return;

            MeshRenderer renderer = quad.GetComponent<MeshRenderer>();
            if (renderer != null){
                // Se è "Easy" o "Hard", ripristina il materiale salvato (default o selezionato)
                if (gameObject.name.Contains("Easy") || gameObject.name.Contains("Hard"))
                    renderer.material = GetMaterialMode(gameObject.name);
                else
                    renderer.material = selectedMaterial;  
        }
    }

    /// <summary>
    /// Aggiorna il materiale visivo di tutti i pulsanti figli,
    /// impostando quello selezionato su "selectedMaterial" e gli altri su "defaultMaterial"
    /// </summary>
    public void UpdateButtonMaterialFromSelection(){
        if (gameObject == null) return;

        Transform parent = gameObject.transform.parent;
        if (parent == null) return;

        foreach (Transform child in parent){

            Transform quad = child.Find("QuadBackground");
            if (quad == null) continue;

            MeshRenderer renderer = quad.GetComponent<MeshRenderer>();
            if (renderer == null) continue;

            renderer.material = GetMaterialMode(child.gameObject.name);
        }
    }

    public void HideButton(){
        VisibilityButton(false);
    }

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

    private Material GetMaterialMode(string name){
         string selectMode = GameManager.Instance.GetMode();
         return name.Contains(selectMode) ? selectedMaterial : defaultMaterial;
     }
}
