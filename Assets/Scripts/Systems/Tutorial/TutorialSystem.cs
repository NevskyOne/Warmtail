using System;
using System.Collections.Generic;
using Data;
using Data.Player;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Systems.Tutorial
{
    public class TutorialSystem : MonoBehaviour
    {
        [SerializeField] private List<TutorStep> _steps;
        public List<TutorStep> Steps => _steps;
        private int _currentIndex;

        [Inject] private GlobalData _globalData;
        
        private void Start()
        {
            _currentIndex = _globalData.Get<SavablePlayerData>().TutorState;
            foreach (var step in _steps)
            {
                foreach (var stepEvent in step.Events)
                {
                    step.Trigger.Event += stepEvent.Invoke;
                }
                step.Trigger.Event += IterateNewStep;
            }
            if(_steps.Count > 0)
                _steps[_currentIndex].Trigger.Activate();
        }

        private void IterateNewStep()
        {
            _steps[_currentIndex].Trigger.Deactivate();
            if (_currentIndex < _steps.Count - 1)
            {
                _currentIndex++;
                _steps[_currentIndex].Trigger.Activate();
                _globalData.Edit<SavablePlayerData>(data => data.TutorState = _currentIndex);
            }
        }
    }
    
    [Serializable]
    public struct TutorStep
    { 
        [SerializeReference] public ITutorTrigger Trigger;
        [SerializeField] public List<UnityEvent> Events; 
    }
}
