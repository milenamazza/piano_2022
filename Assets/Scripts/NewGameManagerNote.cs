// GameManager.cs
using UnityEngine;

// Il GameManager gestisce lo stato globale del gioco, compresi UI, logica, e parametri di gioco.
public class ScreenNoteManager : MonoBehaviour {
    private GameObject screen;
    private GameObject buttonNote;
    private GameObject greenSelect;

    //pseudocostruttore
    public void InitManager(GameObject s){
        screen = s;
        print("mi");
        Transform buttonTransform = screen.transform.FindDeepChild("btnPlayNote");
        if (buttonTransform != null) 
            print("enter");
            buttonNote = buttonTransform.gameObject;
            print(buttonNote);
    }

    public void selectCurrentNote(Note currentNote){
        var script = buttonNote.GetComponent<Btn>();
        if (script != null)
            script.ShowButton();
    }

    public void Win()
    {
        screen.SetActive(false);
    }


}

