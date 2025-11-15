using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Data.Player
{
    [CreateAssetMenu(fileName = "Player Config", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeReference] public List<IAbility> Abilities {get; private set;}
    }
}