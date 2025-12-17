using System.Collections.Generic;
using Entities.Core;
using Interfaces;
using UnityEngine;

namespace Systems.SequenceActions
{
    public class ActivateObjectsAction : ISequenceAction
    {
        [SerializeField] private bool _active;
        [SerializeField] private List<string> _objectIds;
            
        public void Invoke()
        {
            _objectIds.ForEach(x => SavableObjectsResolver.FindObjectById(x).SetActive(_active));
        }
    }
}