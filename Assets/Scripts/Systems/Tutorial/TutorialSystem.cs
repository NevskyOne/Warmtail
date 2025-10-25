using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Systems.Tutorial
{
    public class TutorialSystem : MonoBehaviour
    {
        [SerializeField] private List<TutorStep> _steps;
        public List<TutorStep> Steps => _steps;
        private int _currentIndex;

        private void Start()
        {
            foreach (var step in _steps)
            {
                foreach (var stepEvent in step.Events)
                {
                    step.Trigger.Event += stepEvent.Activate;
                }
                step.Trigger.Event += IterateNewStep;
            }
            
            _steps[_currentIndex].Trigger.Activate();
        }

        private void IterateNewStep()
        {
            if (_currentIndex > 0)
            {
                foreach (var stepEvent in _steps[_currentIndex-1].Events)
                {
                    stepEvent.Deactivate();
                }
            }
            
            _steps[_currentIndex].Trigger.Deactivate();
            if (_currentIndex < _steps.Count - 1)
            {
                _currentIndex++;
                _steps[_currentIndex].Trigger.Activate();
            }

            
        }
    }
    
    [Serializable]
    public struct TutorStep
    { 
        [SerializeReference] public ITutorTrigger Trigger;
        [SerializeReference] public List<ITutorEvent> Events; 
    }
}
