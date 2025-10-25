using Data;
using Entities.UI;
using UnityEngine.InputSystem;
using Zenject;

namespace EditorOnly
{
    public class KeysDebug
    {
        #if UNITY_EDITOR
        [Inject]
        private void Construct(PlayerInput input, PopupSystem popupSystem, UIStateSystem _uiState)
        {
            input.actions["Escape"].performed += _ =>
            {
                if (_uiState.CurrentState == UIState.Normal)
                    _uiState.SwitchCurrentState(UIState.Settings);
                else if (_uiState.CurrentState == UIState.Settings)
                    _uiState.SwitchCurrentState(UIState.Normal);
            };
            foreach (var action in input.actions)
            {
                if (action.actionMap.name == "Player")
                {
                    action.performed += _ => popupSystem.ShowPopup(
                        new NotificationPopup("Action performed", $"Pressed key: {action.name}", 2000));
                }
            }
        }
        #endif
    }
}