using System.Collections.Generic;
using Rng = UnityEngine.Random;
using UnityEngine;
using Interfaces;

namespace Entities.Puzzle
{
    public class GearsPuzzle : MonoBehaviour, IPuzzle
    {
        [SerializeField] private GameObject _gearPref;
        [SerializeField] private Transform[] _levelsPositions;
        [SerializeField] private int _gearCount;

        [SerializeField] private Gear[] _warmTriggers;
        private List<Gear> _gearsActivated;

        public void TurnOnGear(Gear gear)
        {
            _gearsActivated.Add(gear);
            int ind = _gearsActivated.IndexOf(gear);
            if (_warmTriggers.Length <= ind || gear != _warmTriggers[ind]) Reset();
        }

        public void Start()
        {
            List<Transform> levelsPositions = new (_levelsPositions);
            for (int i = 0; i < _gearCount; i ++)
            {
                int j = Rng.Range(0, levelsPositions.Count);
                Instantiate (_gearPref, levelsPositions[j].position, Quaternion.identity, transform);
                levelsPositions.RemoveAt(j);
            }
        }
        public void Reset()
        {
            _gearsActivated = new();
        }
        public void Solve()
        {
            
        }
    }
}