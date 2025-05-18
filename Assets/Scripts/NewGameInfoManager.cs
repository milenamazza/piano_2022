using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gestisce le informazioni visive del gioco: punti e vite.
/// I riferimenti sono a due oggetti TextMesh che mostrano i valori nella scena.
/// </summary>
public class GameInfoManager : MonoBehaviour {
    // TextMesh che mostra i punti e le vite.
    public TextMesh point, life;

    /// <summary>
    /// Inizializza i riferimenti ai TextMesh per punti e vite,
    /// cercandoli nella gerarchia del GameObject parent passato come parametro.
    /// </summary>
    public void InitFromParent(GameObject parent){

        Transform counterTransform = parent.transform.FindDeepChild("point");
        if (counterTransform != null)
            point = counterTransform.GetComponent<TextMesh>();

        counterTransform = parent.transform.FindDeepChild("life");
        if (counterTransform != null)
             life = counterTransform.GetComponent<TextMesh>();
    }

    /// <summary>
    /// Inizializza i valori dei punti e delle vite all'inizio del gioco.
    /// </summary>
    public void StartInfo(int n_lives, int n_point){
        SetPoint(n_point);
        SetLife(n_lives);
    }

    /// <summary>
    /// Aggiorna il testo dei punti, con due cifre (es. "03")
    /// </summary>
    public void SetPoint(int n) {
        point.text = n.ToString("D2");
    }

    /// <summary>
    /// Mostra le vite come cuori ("❤❤❤")
    /// </summary>
    public void SetLife(int n){
        life.text = string.Concat(System.Linq.Enumerable.Repeat("❤", n)); // ❌ Sbagliato: sta sovrascrivendo di nuovo point!
    }
}
