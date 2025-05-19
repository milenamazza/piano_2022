using UnityEngine;

// Il GameManager gestisce lo stato globale del gioco, compresi UI, logica, e parametri di gioco.
public class ScreenBaseManager : MonoBehaviour {
    private GameObject piano;
    private GameObject screen;
    private TextMesh textNote;
    private Transform greenSelect;

    //pseudocostruttore
    public void InitManager(GameObject p, GameObject s){
        piano = p;
        screen = s;
        Transform textTransform = screen.transform.FindDeepChild("txNote");
        if (textTransform != null) 
            textNote = textTransform.GetComponent<TextMesh>();
    }

    public void selectCurrentNote(Note currentNote){
        textNote.text = currentNote.ToString();
        //deseleziona quello precedente
        if (greenSelect != null)
            greenSelect.gameObject.SetActive(false);
        //seleziona quello corrente
        GameObject key = FindKeyFromNote(currentNote);
        greenSelect = key.transform.Find("Key_select");
        if (greenSelect != null)
            greenSelect.gameObject.SetActive(true);
    }

    public void Win(){
        greenSelect.gameObject.SetActive(false);
        screen.SetActive(false);
    }

    private GameObject FindKeyFromNote(Note note)
    {

        string octaveName = "Octave_" + note.octave;
        Transform octave = piano.transform.Find(octaveName);
        if (octave == null)
        {
            Debug.LogWarning("Octave non trovata: " + octaveName);
            return null;
        }

        string keyName;
        if (note.name.ToString().EndsWith("Sharp"))
        {
            string baseNote = note.name.ToString().Replace("Sharp", "");
            keyName = "Key_" + baseNote + "_Black";
        }
        else
        {
            keyName = "Key_" + note.name;
        }

        Transform key = octave.Find(keyName);
        return key != null ? key.gameObject : null;
    }
}