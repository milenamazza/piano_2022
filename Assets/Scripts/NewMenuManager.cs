// Questo è progettato al contrario... ops SI PUÒ CAMBIARE SE ABBIAMO TEMPO
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    private string selectedMode = ""; // Attualmente non utilizzato (solo Debug)
    
    // Riferimenti ai due GameObject che rappresentano le due schermate del menu
    private GameObject screenMenu, screen1, screen2, screen3;

    // Pulsanti per selezionare le modalità di gioco
    private GameObject btnEasy, btnHard;

    // Riferimento al contatore visivo delle ottave (usato solo in UI, con TextMesh)
    private TextMesh counter;

    private GameObject winPopup, losePopup;


    /// <summary>
    /// Inizializza il menu a partire dai due oggetti UI principali
    /// </summary>
    /// <param name="parent">GameObject che contiene i pulsanti e il contatore</param>
    /// <param name="otherScreen">GameObject della schermata successiva (es. schermo gioco)</param>
    public void InitFromParent(GameObject sMenu, GameObject s1,  GameObject s2, GameObject s3){
        screenMenu = sMenu;
        screen1 = s1;
        screen2 = s2;
        screen3 = s3;

        // Trova il contatore (es. "num") e salva il componente TextMesh
        //Transform counterTransform = parent.transform.FindDeepChild("num");
        //if (counterTransform != null) 
        //    counter = counterTransform.GetComponent<TextMesh>();

        //winPopup = startScreen2.transform.FindDeepChild("WinPopup")?.gameObject;
        //losePopup = startScreen2.transform.FindDeepChild("LosePopup")?.gameObject;

        if (winPopup != null) winPopup.SetActive(false);
        if (losePopup != null) losePopup.SetActive(false);

    }

    /// <summary>
    /// Passa dalla schermata iniziale a quella di gioco
    /// </summary>
    public void SwitchScreen(string mode) {
        screenMenu.SetActive(false);
        screen1.SetActive(false);
        screen2.SetActive(false);
        screen3.SetActive(false);
        switch(mode.ToLower()){
            case "menu": screenMenu.SetActive(true); break;
            case "base": screen1.SetActive(true); break;
            case "note": screen2.SetActive(true); break;
            case "melodia": screen3.SetActive(true); break;
            default: break;
        }
    }

    /// <summary>
    /// Imposta la modalità di gioco (attualmente solo stampa a console)
    /// </summary>
    public void SetMode(string mode){
        Debug.Log("Modalità selezionata: " + mode);
        selectedMode = mode; // Potrebbe servire in futuro
    }

    /// <summary>
    /// Gestisce l'aumento o la diminuzione delle ottave, in base all'indice del dropdown
    /// </summary>
    public int SetOctaves(int selectedOctaves, int dropdownIndex){
        // dropdownIndex > 0 = aumenta, altrimenti diminuisci
        if(dropdownIndex > 0)
            selectedOctaves = SetOctavePlus(selectedOctaves);
        else
            selectedOctaves = SetOctaveMenos(selectedOctaves);

        // Aggiorna la UI del contatore
        counter.text = selectedOctaves.ToString();

        return selectedOctaves;
    }

    /// <summary>
    /// Aumenta le ottave fino a un massimo di 8
    /// </summary>
    private int SetOctavePlus(int selectedOctaves){
        if(selectedOctaves < 8) 
            selectedOctaves++;
        return selectedOctaves;
    }

    /// <summary>
    /// Diminuisce le ottave fino a un minimo di 1
    /// </summary>
    private int SetOctaveMenos(int selectedOctaves){
        if(selectedOctaves > 1) 
            selectedOctaves--;
        return selectedOctaves;
    }


    public void ShowEndPopup(bool win)
{
    if (winPopup == null || losePopup == null) return;

    if (win)
    {
        winPopup.SetActive(true);
        losePopup.SetActive(false);
    }
    else
    {
        winPopup.SetActive(false);
        losePopup.SetActive(true);
    }
}

public void HideAllPopups()
{
    if (winPopup != null) winPopup.SetActive(false);
    if (losePopup != null) losePopup.SetActive(false);
}


}
