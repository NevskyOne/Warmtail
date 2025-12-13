using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Systems.SequenceActions
{
    public class ActivateObjectsAction : ISequenceAction
    {
        [SerializeField] private bool _active;
        [SerializeField] private List<GameObject> _objects;
            
        public void Invoke()
        {
            _objects.ForEach(x => x.SetActive(_active));
        }
    }
}