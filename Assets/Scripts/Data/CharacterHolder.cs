using System;
using System.Collections.Generic;
using Systems.Dialogues.Nodes;
using TriInspector;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Character Holder", menuName = "Configs/Character Holder")]
    public class CharacterHolder : ScriptableObject
    {
        [SerializeField, TableList(Draggable = true, AlwaysExpanded = true)]
        private List<CharacterConfig> _characters;
        
        public List<CharacterConfig> Characters => _characters;
    }

    [Serializable]
    public struct CharacterConfig
    {
        public Characters Character;
        public List<CharactersEmotions> Emotions;
        public List<Sprite> Sprites;
    }
}