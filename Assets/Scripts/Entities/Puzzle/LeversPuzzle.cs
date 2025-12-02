using System.Collections.Generic;
using Rng = UnityEngine.Random;
using UnityEngine;
using Interfaces;

namespace Entities.Puzzle
{
    public class LeversPuzzle : MonoBehaviour, IPuzzle
    {
        [SerializeField] private GameObject _leverPref;
        [SerializeField] private Transform[] _leversPositions;
        [SerializeField] private int _leverCount;
        private int _leverActive;

        public void TurnOffLever() ///////// Надо вызывать turn on/off lever из созданных рычагов
        {
            _leverActive--;
        }
        public void TurnOnLever()
        {
            _leverActive++;
        }

        public void Start()
        {
            List<Transform> leversPositions = new (_leversPositions);
            for (int i = 0; i < _leverCount; i ++)
            {
                int j = Rng.Range(0, leversPositions.Count);
                Instantiate (_leverPref, leversPositions[j].position, Quaternion.identity, transform);
                leversPositions.RemoveAt(j);
            }
        }
        public void Reset()
        {

        }
        public void Solve()
        {
            
        }
    }
}