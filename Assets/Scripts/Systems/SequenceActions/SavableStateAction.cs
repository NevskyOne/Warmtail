using System.Collections.Generic;
using Entities.Core;
using Entities.Probs;
using Interfaces;
using UnityEngine;

namespace Systems.SequenceActions
{
    public class SavableStateAction : ISequenceAction
    {
        [SerializeField] private bool _active;
        [SerializeField] private List<string> _objectIds;
        
        public void Invoke()
        {
            _objectIds.ForEach(x => 
                SavableObjectsResolver.FindObjectById<SavableStateObject>(x).ChangeState(_active));
        }
    }
}