using UnityEngine;

[System.Serializable]
public struct Note
{
    public NoteName name;
    public int octave;

    public Note(NoteName name, int octave)
    {
        this.name = name;
        this.octave = octave;
    }

    public override string ToString()
    {
        return name.ToString() + octave;
    }

    public bool Equals(Note other)
    {
        return name == other.name && octave == other.octave;
    }
}

public enum NoteName
{
    C, D, E, F, G, A, B, // note base
    CSharp, DSharp, FSharp, GSharp, ASharp // diesis (per modalità difficile)
}

[System.Serializable]
public struct TimedNote
{
    public Note note;
    public float duration; // in secondi

    public TimedNote(Note note, float duration)
    {
        this.note = note;
        this.duration = duration;
    }

    public override string ToString()
    {
        return $"{note} ({duration}s)";
    }
}
