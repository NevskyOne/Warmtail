using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Systems;
using Systems.Effects;
using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Entities.UI
{
    public class UIStateSystem : MonoBehaviour
    {
        [SerializeReference, Range(0.5f, 5f)] private float _crossFadeTime;
        [SerializeField] private SerializedDictionary<UIState, CanvasGroup> _canvasGroups = new();

        public UIState CurrentState { get; private set; }

        [Inject]
        private void Construct(PlayerInput input)
        {
            input.actions["Escape"].performed += _ =>
            {
                if (CurrentState == UIState.Normal)
                    SwitchCurrentStateAsync(UIState.Pause);
                else if (CurrentState == UIState.Pause)
                    SwitchCurrentStateAsync(UIState.Normal);
            };
        }
        
        public void BackToNormal() => SwitchCurrentStateAsync(UIState.Normal);
        public void ToSettings() => SwitchCurrentStateAsync(UIState.Settings);
        public void ToSaves() => SwitchCurrentStateAsync(UIState.Saves);
        public async void SwitchCurrentStateAsync(UIState state)
        {
            var currentCanvas = _canvasGroups[CurrentState];
            var targetCanvas = _canvasGroups[state];
            if (currentCanvas)
            {
                currentCanvas.interactable = false;
                currentCanvas.blocksRaycasts = false;
            }

            if (targetCanvas)
            {
                targetCanvas.interactable = true;
                targetCanvas.blocksRaycasts = true;
            }
            CurrentState = state;
            
            await foreach (var (a, b) in CrossfadeEffect.CrossfadeTwins(_crossFadeTime))
            {
                if (currentCanvas) currentCanvas.alpha = a;
                if (targetCanvas) targetCanvas.alpha = b;
            }
        }
    }
    
    [Serializable]
    public enum UIState
    {
        Normal, Settings, Pause, Saves, Dialogue
    }
}