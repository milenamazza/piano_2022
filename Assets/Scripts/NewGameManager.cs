// GameManager.cs
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;


// Il GameManager gestisce lo stato globale del gioco, compresi UI, logica, e parametri di gioco.
public class GameManager : MonoBehaviour
{
    // Riferimenti ai due schermi UI principali: menu iniziale e schermata di gioco
    public GameObject screenMenu, screen1, screen2, screen3, winPopUp, btnReset;

    // Singleton: accesso globale all'istanza del GameManager
    public static GameManager Instance { get; private set; }

    // Manager per note, UI e menu. Istanziati come oggetti "normali" (non MonoBehaviour)
    [SerializeField] public NoteManager noteManager;
    public MenuManager menuManager; // Sarà assegnato da Inspector
    public CelebrationManager celebrationManager; // Sarà assegnato da Inspector

    public GameObject piano;

    //riferimento al nuovo tasto
    public InputActionReference confirmNoteActionLeft;
    public InputActionReference confirmNoteActionRight;


    //suoni feedback
    public AudioClip correctSound;
    public AudioClip wrongSound;
    private AudioSource audioSource;
    //suoni vittoria e sconfitta
    public AudioClip winSound;


    // Stack di note da indovinare
    private Stack<Note> currentSequence = new Stack<Note>();
    private Note currentNote;
    // per il controllo delle note
    private string lastPlayedNote = "";

    //Manager con dinamiche livello corrente
    private ScreenBaseManager level1;
    private ScreenNoteManager level2;
    private ScreenMelodiaManager level3;


    //DEFINIZIONE VARIABILI CON GETTER E SETTER
    private string selectedMode { get; set; }
    public string GetMode() => selectedMode;
    public void SetMode(string mode) => selectedMode = mode;

    private string selectedMelody { get; set; }
    public string GetMelody() => selectedMelody;
    public void SetMelody(string melody) => selectedMelody = melody;

    // Stato di gioco
    private bool inPlay { get; set; }
    public bool GetInPlay() => inPlay;

    // Funzioni per il livello 3
    public Note GetCurrentNoteObj() => currentNote;
    public List<Note> GetCurrentSequenceCopy() => new List<Note>(currentSequence);


    // Unity callback: chiamato appena l’oggetto viene inizializzato
    private void Awake()
    {
        // Inizializza i manager UI con i riferimenti ai GameObject UI passati via Inspector
        menuManager.InitFromParent(screenMenu, screen1, screen2, screen3);
        Transform parent = screenMenu.transform.parent;
        winPopUp = parent.FindDeepChild("WinPopup")?.gameObject;
        btnReset = parent.FindDeepChild("btnReset")?.gameObject;

        //imposta parametri di default
        selectedMode = "Note";
        selectedMelody = "Happy Birthday";
        inPlay = false;

        // Singleton pattern: impedisce la creazione di più GameManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Doppione trovato → distrugge questa istanza
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persisti il GameManager anche se si cambia scena

        //Boooo
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // Avvia una nuova partita
    public void StartGame()
    {
        // UI Passa dallo schermo di menu a quello di gioco
        menuManager.SwitchScreen(selectedMode); 
        menuManager.HideAllPopups();

        // Abilita lo stato di gioco
        inPlay = true;
        btnReset.SetActive(true);

        //GESTIONE SEQUENZA NOTE E NOTA CORRENTE
        // Genera la sequenza di note da indovinare
        List<Note> noteList = noteManager.GenerateSequence(selectedMelody);
        currentSequence = new Stack<Note>(noteList);
        currentNote = currentSequence.Pop();
        Debug.Log(currentNote);
        //UI KEY PIANO
        // 🔥 Abilita/disabilita tasti visivamente
        // 🔥 Imposta le ottave attive per tutti i tasti
        List<int> allowedOctaves = CalculateAllowedOctaves(1);
        HoverHighlight.AllowedOctaves = allowedOctaves;
        foreach (HoverHighlight key in FindObjectsOfType<HoverHighlight>()){
            key.UpdateKeyState();
        }

        //avvio effettivo della modalità di gioco
        StartMode();

        //attiva i bottoni per confermare
        if (confirmNoteActionLeft != null)
            confirmNoteActionLeft.action.Enable();

        if (confirmNoteActionRight != null)
            confirmNoteActionRight.action.Enable();

    }

    // ?
    public void StartMode(){
        switch(selectedMode){
            case "Base":
                level1 = new ScreenBaseManager();
                level1.InitManager(piano, screen1);
                level1.selectCurrentNote(currentNote);
                break;
            case "Note":
                level2 = new ScreenNoteManager();
                level2.InitManager(screen2);
                level2.selectCurrentNote(currentNote);
                break;
            case "Melodia":
                level3 = new ScreenMelodiaManager();
                level3.InitManager(screen3);
                break;

        }
    }

    // Restituisce la nota attualmente da indovinare
    public string GetCurrentNote(){
        return currentNote.ToString();
    }

    // parte per controllare se la nota è giusta
   public void RegisterPlayedNote(string playedNote)
    {
        if (!inPlay) return;
        lastPlayedNote = playedNote;
        Debug.Log("Nota registrata: " + playedNote);
    }

    public void AdvanceNote(){
        //VITTORIAAAA
        if (currentSequence.Count == 0){
            Debug.Log("Sequenza finita!");

            inPlay = false;
            PlayEndSound(true);
            winPopUp.SetActive(true);winPopUp.SetActive(true);
            celebrationManager.PlayConfetti();
            piano.DeselectPiano();

            switch(selectedMode){
                case "Base": level1.Win(); break;
                case "Note":level2.Win();break;
                case "Melodia":level3.Win();break;
            }

            //StartCoroutine(ReturnToMenuAfterDelay(4f));
            return;
        }
        else{
            currentNote = currentSequence.Pop();

            switch(selectedMode){
                case "Base":
                    level1.selectCurrentNote(currentNote);
                    break;
                case "Note":
                    level2.selectCurrentNote(currentNote);
                    break;    
                // altri casi...
            }
        }
    }


    private void CheckLastNote()
    {
        if (lastPlayedNote == "")
        {
            Debug.Log("Nessuna nota registrata.");
            return;
        }

        if (selectedMode == "Melodia")
        {
            level3.OnConfirm(lastPlayedNote);
        }
        else
        {
            if (lastPlayedNote == currentNote.ToString())
            {
                PlayFeedbackSound(true);
                AdvanceNote();
            }
            else
            {
                PlayFeedbackSound(false);
            }
        }

        lastPlayedNote = "";
    }




    private void OnEnable()
    {
        if (confirmNoteActionLeft != null)
            confirmNoteActionLeft.action.performed += OnConfirmNote;

        if (confirmNoteActionRight != null)
            confirmNoteActionRight.action.performed += OnConfirmNote;
    }

    private void OnDisable()
    {
        if (confirmNoteActionLeft != null)
            confirmNoteActionLeft.action.performed -= OnConfirmNote;

        if (confirmNoteActionRight != null)
            confirmNoteActionRight.action.performed -= OnConfirmNote;
    }


    private void OnConfirmNote(InputAction.CallbackContext ctx)
    {
        CheckLastNote();
    }


    // per la gestione dei tasti

    private List<int> CalculateAllowedOctaves(int selectedOctaves)
    {
        int center = 4;
        List<int> allowedOctaves = new List<int> { center };

        int up = 1, down = 1;
        while (allowedOctaves.Count < selectedOctaves)
        {
            if (allowedOctaves.Count < selectedOctaves)
                allowedOctaves.Add(center - down++);
            if (allowedOctaves.Count < selectedOctaves)
                allowedOctaves.Add(center + up++);
        }

        allowedOctaves.Sort();
        return allowedOctaves;
    }


    //tasto hint
    public void ShowHint()
    {
        if (!inPlay )
            return;

        int targetOctave = currentNote.octave;

        foreach (HoverHighlight key in FindObjectsOfType<HoverHighlight>())
        {
            if (key.GetOctave() == targetOctave && key.IsEnabled())
            {
                key.Pulse(); // Lampeggia i tasti dell’ottava corretta
            }
        }
    }

    private IEnumerator ReturnToMenuAfterDelay(float delay){
        yield return new WaitForSeconds(delay);
        
        Reset();
    }

    public void Reset(){
        //selectedMode="Note";
        //selectedMelody="Happy Birthday";
        piano.DeselectPiano();

        inPlay = false;

        // Disattiva popup
        screenMenu.SetActive(true);
        screen1.SetActive(false);
        screen2.SetActive(false);
        screen3.SetActive(false);
        winPopUp.SetActive(false);
        btnReset.SetActive(false);
    }

    public void PlayFeedbackSound(bool success)
    {
        if (audioSource == null) return;

        audioSource.clip = success ? correctSound : wrongSound;
        audioSource.Play();
    }

    private void PlayEndSound(bool win)
    {
        if (audioSource == null) return;

        audioSource.clip = winSound;
        audioSource.Play();
    }


    // metodi dei bottoni del livello 3 
    public void PlayMelodiaOriginale()
    {
        level3?.PlayOriginalMelody();
    }

    public void PlayMelodiaUtente()
    {
        level3?.PlayPlayerMelody();
    }

    public void ResetMelodia()
    {
        level3?.ResetMelody();
    }


}

