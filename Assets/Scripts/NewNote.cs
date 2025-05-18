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