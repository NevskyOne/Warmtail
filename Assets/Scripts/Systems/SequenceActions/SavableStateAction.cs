using System.Collections.Generic;
using Entities.Probs;
using Interfaces;
using UnityEngine;

namespace Systems.SequenceActions
{
    public class SavableStateAction : ISequenceAction
    {
        [SerializeField] private bool _active;
        [SerializeField] private List<SavableStateObject> _objects;
        
        public void Invoke()
        {
            _objects.ForEach(x => x.ChangeState(_active));
        }
    }
}