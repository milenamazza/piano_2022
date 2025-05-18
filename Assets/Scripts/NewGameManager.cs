// GameManager.cs
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;


// Il GameManager gestisce lo stato globale del gioco, compresi UI, logica, e parametri di gioco.
public class GameManager : MonoBehaviour
{
    // Riferimenti ai due schermi UI principali: menu iniziale e schermata di gioco
    public GameObject screen1, screen2;

    // Singleton: accesso globale all'istanza del GameManager
    public static GameManager Instance { get; private set; }

    // Manager per note, UI e menu. Istanziati come oggetti "normali" (non MonoBehaviour)
    [SerializeField] private NoteManager noteManager;
    //[SerializeField] private GameInfoManager gameInfoManager ;

    public MenuManager menuManager; // Sarà assegnato da Inspector
    public CelebrationManager celebrationManager; // Sarà assegnato da Inspector

    public GameObject piano;

    // Parametri di gioco selezionabili dall’utente
    private string selectedMode = "Easy";
    private int selectedOctaves = 1, n_note = 10;

    // Stato di gioco
    private int lives = 0, maxLives = 3, point = 0;
    private bool inPlay = false;
    private bool isVisibleNameNote = false;

    // Stack di note da indovinare
    private Stack<Note> currentSequence = new Stack<Note>();
    private Note currentNote;

    // per il controllo delle note
    private string lastPlayedNote = "";

    //riferimento al nuovo tasto
    public InputActionReference confirmNoteActionLeft;
    public InputActionReference confirmNoteActionRight;


    //suoni feedback
    public AudioClip correctSound;
    public AudioClip wrongSound;
    private AudioSource audioSource;

    //suoni vittoria e sconfitta
    public AudioClip winSound;
    public AudioClip loseSound;




    // Unity callback: chiamato appena l’oggetto viene inizializzato
    private void Awake()
    {
        // Inizializza i manager UI con i riferimenti ai GameObject UI passati via Inspector
        menuManager.InitFromParent(screen1, screen2);
        //gameInfoManager.InitFromParent(screen2);
        selectedMode = "Easy";

        // Singleton pattern: impedisce la creazione di più GameManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Doppione trovato → distrugge questa istanza
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persisti il GameManager anche se si cambia scena

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

    }

    // Avvia una nuova partita
    public void StartGame()
    {
        // Passa dallo schermo di menu a quello di gioco
        menuManager.SwitchScreen();
        menuManager.HideAllPopups();

        // Abilita lo stato di gioco
        inPlay = true;

        // Reset di vite e punteggio
        lives = maxLives;
        point = 0;
        //gameInfoManager.StartInfo(lives, point); // Aggiorna l'UI

        // Genera la sequenza di note da indovinare
        List<Note> noteList = noteManager.GenerateSequence(selectedMode, selectedOctaves, n_note);
        currentSequence = new Stack<Note>(noteList);

        // 🔥 Abilita/disabilita tasti visivamente
        // 🔥 Imposta le ottave attive per tutti i tasti
        List<int> allowedOctaves = CalculateAllowedOctaves(selectedOctaves);
        HoverHighlight.AllowedOctaves = allowedOctaves;

        foreach (HoverHighlight key in FindObjectsOfType<HoverHighlight>())
        {
            key.UpdateKeyState();
        }


        currentNote = currentSequence.Pop();

        Debug.Log(currentNote);

        if (confirmNoteActionLeft != null)
            confirmNoteActionLeft.action.Enable();

        if (confirmNoteActionRight != null)
            confirmNoteActionRight.action.Enable();

    }

    // Imposta la modalità selezionata (es. Easy o Hard)
    public void SetMode(string mode){
        selectedMode = mode;
        menuManager.SetMode(mode); // Aggiorna la UI
    }

    public string GetMode(){
        return selectedMode;
    }

    // Imposta il numero di ottave selezionato dal dropdown
    public void SetOctaves(int dropdownIndex){
        Debug.Log(selectedOctaves+" "+dropdownIndex);
        selectedOctaves = menuManager.SetOctaves(selectedOctaves, dropdownIndex);
    }

    // Restituisce il numero di ottave attualmente selezionato
    public int GetOctaves(int dropdownIndex){
        return selectedOctaves;
    }

    // Restituisce se siamo in stato di gioco o meno
    public bool GetInPlay(){
        return inPlay;
    }

    // Scala le vite di uno e aggiorna l'UI
    public void DescremenLive(){
        lives--;
        //gameInfoManager.SetLife(lives);
    }

    // Aumenta il punteggio e aggiorna l'UI
    public void IncrementPoint(){
        point++;
        //gameInfoManager.SetPoint(point);
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

    private void Update()
    {

    }

    private void CheckLastNote()
    {
        if (lastPlayedNote == ""){
            Debug.Log("Nessuna nota registrata.");
            return;
        }
        GameObject obj = GameObject.Find("ButtonNote");
        obj.GetComponent<Btn>()?.ShowButton();

        if (lastPlayedNote == currentNote.ToString())
        {
            IncrementPoint();
            PlayFeedbackSound(true);
            if (currentSequence.Count > 0){
                currentNote = currentSequence.Pop();
            }
            else{
                Debug.Log("Hai finito tutte le note!");
                inPlay = false;
                menuManager.ShowEndPopup(true); // ✅ Mostra popup vittoria
                PlayEndSound(true);
                celebrationManager.PlayConfetti(); // o direttamente: confetti.Play();
                StartCoroutine(ReturnToMenuAfterDelay(6f)); // ✅ Ritorna al menu dopo 3 secondi
            }
        }
        else{
            DescremenLive();
            PlayFeedbackSound(false);

            if (lives <= 0)
            {
                inPlay = false;
                menuManager.ShowEndPopup(false); // ✅ Mostra popup sconfitta
                PlayEndSound(false);
                celebrationManager.PlayConfetti(); // o direttamente: confetti.Play();
                StartCoroutine(ReturnToMenuAfterDelay(6f)); // ✅ Ritorna al menu dopo 3 secondi
            }
        }

        lastPlayedNote = ""; // Reset!
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

    //mostra i nomi delle note sul piano (button MOSTA NOTE)
    public void VisibilityNameNote()
    {
        isVisibleNameNote = !isVisibleNameNote;
        // Trova tutti i figli chiamati "NameNoteText"
        List<Transform> noteTexts = piano.transform.FindAllChildrenByName("NameNoteText");

        // Attiva ciascuno
        foreach (Transform noteText in noteTexts)
            noteText.gameObject.SetActive(isVisibleNameNote);

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

    private IEnumerator ReturnToMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Disattiva popup
        screen2.SetActive(false);
        screen1.SetActive(true);

        // Reset stato se vuoi
        inPlay = false;
        point = 0;
        lives = 3;
    }

    private void PlayFeedbackSound(bool success)
    {
        if (audioSource == null) return;

        audioSource.clip = success ? correctSound : wrongSound;
        audioSource.Play();
    }

    private void PlayEndSound(bool win)
    {
        if (audioSource == null) return;

        audioSource.clip = win ? winSound : loseSound;
        audioSource.Play();
    }






}

