using UnityEngine;

public class KeyPrefab : MonoBehaviour
{
    void Awake()
    {
        // Trova il figlio chiamato "NameNoteText"
        Transform nameTextTransform = transform.Find("NameNoteText");

        if (nameTextTransform != null) {
            TextMesh textMesh = nameTextTransform.GetComponent<TextMesh>();
            
            // Imposta il testo con il nome del padre corretto
            if (textMesh != null)
                textMesh.text = gameObject.name.Replace("Key_", "").Replace("_Black", "#");
            nameTextTransform.gameObject.SetActive(false);
        }
    }
}
