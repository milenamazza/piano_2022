using UnityEngine;
using UnityEngine.InputSystem; // Nuovo namespace

public class InputManager : MonoBehaviour
{
    [SerializeField] private NoteHighlighterManager _noteHighlighterManager;

    private Keyboard _keyboard;

    private void Awake()
    {
        _keyboard = Keyboard.current;
    }

    void Update()
    {
        if (_keyboard.spaceKey.wasPressedThisFrame)
        {
            _noteHighlighterManager.SetNextKey();
        }
    }
}
