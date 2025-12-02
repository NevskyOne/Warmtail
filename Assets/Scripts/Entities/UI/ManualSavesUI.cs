using System.IO;
using Data;
using Entities.Core;
using Systems.DataSystems;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Entities.UI
{
    public class ManualSavesUI : MonoBehaviour
    {
	    [SerializeField] private TMP_InputField _saveNameInput;
        [SerializeField] private Button _newSaveButton;
        [SerializeField] private GameObject _savePrefab;
        [SerializeField] private Transform _saveParent;
        [SerializeField] private UnityEvent _saveButtonEvent;
        [SerializeField] private GlobalData _persistentGlobalData;
        private GlobalData _globalData;
        private ManualSaveSystem _manualSaveSystem;
        private SaveSystem _saveSystem;
        private DiContainer _container;
        private SceneLoader _sceneLoader;

        private SaveContainer _emptyContainer;

        [Inject]
		private void Construct(ManualSaveSystem manualSaveSystem, SaveSystem saveSystem,
			DiContainer container, GlobalData globalData, SceneLoader sceneLoader)
        {
			_manualSaveSystem = manualSaveSystem;
			_saveSystem = saveSystem;
			_container = container;
			_globalData = globalData;
			_sceneLoader = sceneLoader;
			if(_newSaveButton)
				_newSaveButton.onClick.AddListener(_saveButtonEvent.Invoke);

			foreach (var meta in _manualSaveSystem.ListManualSaves())
			{
				SpawnNewPrefab(meta);
			}

			CheckIfCanAdd();
        }

        public async void CreateCurrentSave()
        {
	        var meta = await _manualSaveSystem.CreateManualSaveAsync(_saveNameInput.text,_saveSystem.AutoContainer);
			SpawnNewPrefab(meta);
        }
        
        public async void CreateNewSave()
        {
	        if (_emptyContainer == null)
	        {
		        var dataList = _persistentGlobalData.SavableData;
		        var emptyContainer = new SaveContainer();
		        foreach (var d in dataList)
		        {
			        if (d == null) continue;
			        var key = d.GetType().Name;
			        emptyContainer.Blocks[key] = d;
		        }

		        _emptyContainer = emptyContainer;
	        }

	        var meta = await _manualSaveSystem.CreateManualSaveAsync(_saveNameInput.text,_emptyContainer);
	        SpawnNewPrefab(meta);
        }

        private void SpawnNewPrefab(ManualSaveMeta meta)
        {
	        var newObj = _container.InstantiatePrefab(_savePrefab, _saveParent).transform;
	        
	        byte[] bytes = File.ReadAllBytes(meta.ThumbnailPath);
	        Texture2D tex = new Texture2D(2, 2);
	        tex.LoadImage(bytes);
	        tex.Apply();

	        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
	        
	        newObj.GetChild(0).GetComponent<TMP_Text>().text = meta.Name;
	        newObj.GetChild(1).GetComponent<TMP_Text>().text = meta.CreatedAt.ToShortDateString() + "\n" + meta.CreatedAt.ToLongTimeString();
	        newObj.GetChild(2).GetComponent<Image>().sprite = sprite;
	        newObj.GetChild(3).GetComponent<Button>().onClick.AddListener(() => DeleteSave(meta.Id, newObj.gameObject));
	        newObj.GetChild(2).GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
	        {
		        _globalData.UpdateAllData(_manualSaveSystem.LoadManualSave(meta.Id, _globalData.SavableData));
		        _sceneLoader.StartSceneProcess("Gameplay");
	        });
	        CheckIfCanAdd();
        }

        private void DeleteSave(string id, GameObject uiSave)
        {
			_manualSaveSystem.DeleteManualSave(id);
			Destroy(uiSave);
			CheckIfCanAdd();
        }

        private void CheckIfCanAdd()
        {
	        _newSaveButton.interactable = _saveParent.childCount < 3;
        }
    }
}