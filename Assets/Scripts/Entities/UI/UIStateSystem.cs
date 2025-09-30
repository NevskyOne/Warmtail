using System.Collections.Generic;
using Systems;
using TriInspector;
using UnityEngine;

namespace Entities.UI
{
    public class UIStateSystem : MonoBehaviour
    {
        [SerializeReference, Range(0.5f, 3f)] private float _crossFadeTime;
        [ShowInInspector] private readonly Dictionary<UIState, CanvasGroup> _canvasGroups = new();

        private UIState _currentState;
        
        public async void SwitchCurrentState(UIState state)
        {
            var currentCanvas = _canvasGroups[_currentState];
            var targetCanvas = _canvasGroups[state];

            _currentState = state;
            
            await foreach (var (a, b) in CrossfadeSystem.CrossfadeTwins(_crossFadeTime))
            {
                currentCanvas.alpha = a;
                targetCanvas.alpha = b;
            }
        }
    }
    
    public enum UIState
    {
        Normal, Battle
    }
}