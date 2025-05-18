using System.Collections.Generic;
using UnityEngine;


// Gestisce la generazione di sequenze di note in base alla modalità scelta (Easy/Hard)
public class NoteManager : MonoBehaviour
{

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


    public List<Note> GenerateSequence(string songName)
    {
        List<Note> currentSequence = new List<Note>();
        int baseOctave = 4;

        List<NoteName> melody;

        switch (songName.ToLower())
        {
            case "fra martino": //?? non doveva essere little lamb??
                melody = new List<NoteName> {
                    NoteName.C, NoteName.D, NoteName.E, NoteName.C,
                    NoteName.C, NoteName.D, NoteName.E, NoteName.C,
                    NoteName.E, NoteName.F, NoteName.G,
                    NoteName.E, NoteName.F, NoteName.G
                };
                break;

            case "little lamb": 
                melody = new List<NoteName> {
                    NoteName.E, NoteName.D, NoteName.C, NoteName.D,
                    NoteName.E, NoteName.E, NoteName.E, NoteName.D,
                    NoteName.D, NoteName.D, NoteName.E, NoteName.G,
                    NoteName.G,
                };
                break;

            case "ode to joy":
                melody = new List<NoteName> {
                    NoteName.E, NoteName.E, NoteName.F, NoteName.G,
                    NoteName.G, NoteName.F, NoteName.E, NoteName.D,
                    NoteName.C, NoteName.C, NoteName.D, NoteName.E,
                    NoteName.E, NoteName.D, NoteName.D
                };
                break;

            case "happy birthday":
                melody = new List<NoteName> {
                    NoteName.C, NoteName.C, NoteName.D, NoteName.C,
                    NoteName.F, NoteName.E,
                    NoteName.C, NoteName.C, NoteName.D, NoteName.C,
                    NoteName.G, NoteName.F
                };
                break;

            default:
                Debug.LogWarning("Canzone non riconosciuta. Restituita sequenza vuota.");
                return currentSequence;
        }

        // Inverti la sequenza
        melody.Reverse();

        // Converte la melodia in oggetti Note
        foreach (NoteName noteName in melody)
        {
            currentSequence.Add(new Note
            {
                name = noteName,
                octave = baseOctave
            });
        }

        return currentSequence;
    }


    public List<TimedNote> GenerateTimedMelody(string songName)
{
    List<TimedNote> sequence = new List<TimedNote>();
    int baseOctave = 4;

    switch (songName.ToLower())
    {
        case "happy birthday":
            sequence = new List<TimedNote>
            {
                new TimedNote(new Note(NoteName.C, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.C, baseOctave), 0.25f),
                new TimedNote(new Note(NoteName.D, baseOctave), 0.75f),
                new TimedNote(new Note(NoteName.C, baseOctave), 0.75f),
                new TimedNote(new Note(NoteName.F, baseOctave), 0.75f),
                new TimedNote(new Note(NoteName.E, baseOctave), 1f),

                new TimedNote(new Note(NoteName.C, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.C, baseOctave), 0.25f),
                new TimedNote(new Note(NoteName.D, baseOctave), 0.75f),
                new TimedNote(new Note(NoteName.C, baseOctave), 0.75f),
                new TimedNote(new Note(NoteName.G, baseOctave), 0.75f),
                new TimedNote(new Note(NoteName.F, baseOctave), 1f)
            };
            break;

        case "fra martino":
            sequence = new List<TimedNote>
            {
                new TimedNote(new Note(NoteName.C, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.D, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.E, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.C, baseOctave), 0.5f),

                new TimedNote(new Note(NoteName.C, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.D, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.E, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.C, baseOctave), 0.5f),

                new TimedNote(new Note(NoteName.E, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.F, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.G, baseOctave), 1f),

                new TimedNote(new Note(NoteName.E, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.F, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.G, baseOctave), 1f)
            };
            break;

        case "little lamb":
            sequence = new List<TimedNote>
            {
                new TimedNote(new Note(NoteName.E, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.D, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.C, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.D, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.E, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.E, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.E, baseOctave), 1f),

                new TimedNote(new Note(NoteName.D, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.D, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.D, baseOctave), 1f),

                new TimedNote(new Note(NoteName.E, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.G, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.G, baseOctave), 1f)
            };
            break;

        case "ode to joy":
            sequence = new List<TimedNote>
            {
                new TimedNote(new Note(NoteName.E, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.E, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.F, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.G, baseOctave), 0.5f),

                new TimedNote(new Note(NoteName.G, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.F, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.E, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.D, baseOctave), 0.5f),

                new TimedNote(new Note(NoteName.C, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.C, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.D, baseOctave), 0.5f),
                new TimedNote(new Note(NoteName.E, baseOctave), 0.5f),

                new TimedNote(new Note(NoteName.E, baseOctave), 0.75f),
                new TimedNote(new Note(NoteName.D, baseOctave), 0.25f),
                new TimedNote(new Note(NoteName.D, baseOctave), 1.0f)
            };
            break;


        default:
            Debug.LogWarning("Melodia non trovata. Restituita sequenza vuota.");
            break;
    }

    return sequence;
}


}
