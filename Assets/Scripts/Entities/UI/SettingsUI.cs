using Cysharp.Threading.Tasks;
using Data;
using Entities.Localization;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Zenject;

namespace Entities.UI
{
    [DeclareFoldoutGroup("Sliders")]
    [DeclareFoldoutGroup("Dropdowns")]
    [DeclareFoldoutGroup("Toggles")]
    public class SettingsUI : MonoBehaviour
    {
        [Title("Assets")]
        [SerializeField] private AudioMixer _mixer;
        
        [GroupNext("Sliders")]
        [SerializeField] private Slider _mainSoundSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;

        [GroupNext("Dropdowns")] 
        [SerializeField] private TMP_Dropdown _graphicsDropdown;
        [SerializeField] private TMP_Dropdown _languageDropdown;
        
        [GroupNext("Toggles")] 
        [SerializeField] private Toggle _fullScreenToggle;

        private GlobalData _globalData;
        private SettingsData _localData;
        private LocalizationManager _localizationManager;
        private bool _isSaving;
        
        [Inject]
        private void Construct(GlobalData globalData, LocalizationManager localization)
        {
            _globalData = globalData;
            _localizationManager = localization;
            //Load Data
            _localData = _globalData.Get<SettingsData>();
            //Apply Data
            Screen.fullScreenMode = _localData.FullscreenMode ? 
                FullScreenMode.FullScreenWindow : FullScreenMode.MaximizedWindow;
            _localizationManager.CurrentLanguage.Value = (Language)_localData.Language;
            _localizationManager.CurrentLanguage.ForceNotify();
            QualitySettings.SetQualityLevel(_localData.QualityLevel);
            ChangeVolume("MainVolume", _localData.MainSoundVolume);
            ChangeVolume("MusicVolume", _localData.MusicVolume);
            ChangeVolume("SfxVolume", _localData.SfxVolume);
            //UpdateUI
            _fullScreenToggle.isOn = _localData.FullscreenMode;
            _graphicsDropdown.value = _localData.QualityLevel;
            _languageDropdown.value = _localData.Language;
            _mainSoundSlider.value = _localData.MainSoundVolume;
            _musicSlider.value = _localData.MusicVolume;
            _sfxSlider.value = _localData.SfxVolume;
            //Add Listeners
            _fullScreenToggle.onValueChanged.AddListener(ChangeFullScreenState);
            _graphicsDropdown.onValueChanged.AddListener(ChangeQuality);
            _languageDropdown.onValueChanged.AddListener(ChangeLanguage);
            _mainSoundSlider.onValueChanged.AddListener(ChangeMainVolume);
            _musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
            _sfxSlider.onValueChanged.AddListener(ChangeSfxVolume);
        }
        
        public void ChangeFullScreenState(bool value)
        {
            Screen.fullScreenMode = value ? FullScreenMode.FullScreenWindow : FullScreenMode.MaximizedWindow;
            _localData.FullscreenMode = value;
            SaveData();
        }
        
        public void ChangeQuality(int value)
        {
            QualitySettings.SetQualityLevel(value);
            _localData.QualityLevel = value;
            SaveData();
        }
        
        public void ChangeLanguage(int value)
        {
            _localizationManager.CurrentLanguage.Value = (Language)value;
            _localizationManager.CurrentLanguage.ForceNotify();
            _localData.Language = value;
            SaveData();
        }
        
        public void ChangeMainVolume(float value)
        {
            ChangeVolume("MainVolume", value);
            _localData.MainSoundVolume = value;
            SaveData();
        }

        public void ChangeMusicVolume(float value)
        {
            ChangeVolume("MusicVolume", value);
            _localData.MusicVolume = value;
            SaveData();
        }
        
        public void ChangeSfxVolume(float value)
        {
            ChangeVolume("SfxVolume", value);
            _localData.SfxVolume = value;
            SaveData();
        }

        private void ChangeVolume(string groupName, float value)
        {
            value = 20 * Mathf.Log10(value);
            _mixer.SetFloat(groupName, value);
        }

        private async void SaveData()
        {
            if (_isSaving) return;
            _isSaving = true;
            await UniTask.Delay(1000);
            _globalData.Edit<SettingsData>(x => x = _localData);
            _isSaving = false;
        }
    }
}