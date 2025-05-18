using System.Collections.Generic;
using UnityEngine;


// Gestisce la generazione di sequenze di note in base alla modalità scelta (Easy/Hard)
public class NoteManager : MonoBehaviour
{
    // In modalità Easy: solo le note naturali (senza diesis)
    private NoteName[] easyNotes = {
        NoteName.C, NoteName.D, NoteName.E,
        NoteName.F, NoteName.G, NoteName.A, NoteName.B
    };

    // In modalità Hard: tutte le 12 note della scala cromatica
    private NoteName[] hardNotes = {
        NoteName.C, NoteName.CSharp,
        NoteName.D, NoteName.DSharp,
        NoteName.E,
        NoteName.F, NoteName.FSharp,
        NoteName.G, NoteName.GSharp,
        NoteName.A, NoteName.ASharp,
        NoteName.B
    };

    // gestisce l'interazione della selezione delle ottave
    private void HighlightAllowedOctaves(List<int> allowedOctaves)
    {
        GameObject[] allKeys = GameObject.FindGameObjectsWithTag("Key");

        foreach (GameObject key in allKeys)
        {
            Transform parent = key.transform.parent;
            if (parent == null || !parent.name.StartsWith("Octave_"))
                continue;

            int octaveNumber;
            if (!int.TryParse(parent.name.Replace("Octave_", ""), out octaveNumber))
                continue;

            Renderer rend = key.GetComponent<Renderer>();

            if (allowedOctaves.Contains(octaveNumber))
            {
                // ✅ Ottava selezionata: evidenzia (giallo per esempio)
                key.SetActive(true);
                if (rend != null)
                    rend.material.color = Color.yellow;
            }
            else
            {
                // ❌ Ottava non usata: spegni o disattiva
                key.SetActive(false); // o rend.material.color = Color.gray;
            }
        }
    }


    /// <summary>
    /// Genera una lista di Note casuali, a partire dalla modalità e dal numero di ottave
    /// </summary>
    /// <param name="mode">"Easy" o "Hard"</param>
    /// <param name="octaves">Numero di ottave su cui variare le note</param>
    /// <param name="num">Numero di note da generare (default = 10)</param>
    /// <returns>Una lista di oggetti Note</returns>
    public List<Note> GenerateSequence(string mode, int octaves, int num = 10)
    {
        List<Note> currentSequence = new List<Note>();

        NoteName[] notePool = mode == "Easy" ? easyNotes : hardNotes;

        int centerOctave = 4;
        List<int> allowedOctaves = new List<int> { centerOctave };

        int up = 1, down = 1;
        for (int i = 1; allowedOctaves.Count < octaves; i++)
        {
            if (allowedOctaves.Count < octaves)
                allowedOctaves.Add(centerOctave - down++);
            if (allowedOctaves.Count < octaves)
                allowedOctaves.Add(centerOctave + up++);
        }

        // Ordina le ottave per coerenza
        allowedOctaves.Sort();

        for (int i = 0; i < num; i++)
        {
            NoteName randomNote = notePool[Random.Range(0, notePool.Length)];
            int octave = allowedOctaves[Random.Range(0, allowedOctaves.Count)];
            currentSequence.Add(new Note(randomNote, octave));
        }

        //HighlightAllowedOctaves(allowedOctaves);

        return currentSequence;
    }

    

}
