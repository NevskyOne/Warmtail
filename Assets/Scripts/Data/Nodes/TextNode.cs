using Entities.UI;
using Systems;
using UnityEngine;
using Zenject;

[NodeWidth(330)]
public class TextNode : BaseNode
{
    [Input, SerializeField] private int _entry;
    [Output, SerializeField] private int _exit;
    [SerializeField] private int _textId;
    [SerializeField] private Characters _character;
    [SerializeField] private CharactersEmotions _emotion;
    [SerializeField] private string _displayName;

    public int Entry => _entry;

    public int Exit => _exit;

    public int TextId => _textId;

    public Characters Character => _character;

    public CharactersEmotions Emotion => _emotion;

    public string DisplayName => _displayName;

    [Inject] private DialogueSystem _dialogueSystem;
    [Inject] private DialogueVisuals _dialogueVisuals;

    public override void Activate()
    {
        _dialogueVisuals.RequestNewLine(this);
        _dialogueSystem.SetNewNode();
    }
}