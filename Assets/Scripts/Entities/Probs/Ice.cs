using System;
using Entities.UI;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Entities.Probs
{
    public class Ice : MonoBehaviour
    {
        [Inject] private FreezeVisuals _freeze;
        public void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
                _freeze.StartDrain().Forget();
        }
        
        public void OnTriggerExit2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
                _freeze.StopDrain().Forget();
        }
    }
}