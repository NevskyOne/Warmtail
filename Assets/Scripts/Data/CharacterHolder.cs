using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Systems.Dialogues.Nodes;
using TriInspector;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Character Holder", menuName = "Configs/Character Holder")]
    public class CharacterHolder : ScriptableObject
    {
        [SerializeField] private List<CharacterConfig> _characters;
        
        public List<CharacterConfig> Characters => _characters;
    }

    [Serializable]
    public struct CharacterConfig
    {
        public Characters Character;
        public SerializedDictionary<CharactersEmotions, Sprite> EmotionSprites;
    }
}