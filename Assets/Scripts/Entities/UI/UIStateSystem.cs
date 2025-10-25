using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Systems;
using Systems.Effects;
using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities.UI
{
    public class UIStateSystem : MonoBehaviour
    {
        [SerializeReference, Range(0.5f, 5f)] private float _crossFadeTime;
        [SerializeField] private SerializedDictionary<UIState, CanvasGroup> _canvasGroups = new();

        public UIState CurrentState { get; private set; }
        
        public async void SwitchCurrentState(UIState state)
        {
            var currentCanvas = _canvasGroups[CurrentState];
            var targetCanvas = _canvasGroups[state];
            if (currentCanvas) currentCanvas.interactable = false;
            if (targetCanvas) targetCanvas.interactable = true;
            CurrentState = state;
            
            await foreach (var (a, b) in CrossfadeEffect.CrossfadeTwins(_crossFadeTime))
            {
                if (currentCanvas) currentCanvas.alpha = a;
                if (targetCanvas) targetCanvas.alpha = b;
            }
        }
    }
    
    public enum UIState
    {
        Normal, Settings, Pause
    }
}