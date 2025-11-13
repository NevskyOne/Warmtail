using System.IO;
using Data;
using Systems.DataSystems;
using TMPro;
using UnityEngine;
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
        private ManualSaveSystem _manualSaveSystem;
        private SaveSystem _saveSystem;
        private GlobalData _globalData;

        [Inject]
		private void Construct(ManualSaveSystem manualSaveSystem, SaveSystem saveSystem, GlobalData globalData)
        {
			_manualSaveSystem = manualSaveSystem;
			_saveSystem = saveSystem;
			_globalData = globalData;
			_newSaveButton.onClick.AddListener(CreateNewSave);

			foreach (var meta in _manualSaveSystem.ListManualSaves())
			{
				SpawnNewPrefab(meta);
			}
        }

        private async void CreateNewSave()
        {
	        var meta = await _manualSaveSystem.CreateManualSaveAsync(_saveNameInput.text,_saveSystem.Container);
			SpawnNewPrefab(meta);
        }

        private void SpawnNewPrefab(ManualSaveMeta meta)
        {
	        var newObj = Instantiate(_savePrefab, _saveParent).transform;
	        
	        byte[] bytes = File.ReadAllBytes(meta.ThumbnailPath);
	        Texture2D tex = new Texture2D(2, 2);
	        tex.LoadImage(bytes);
	        tex.Apply();

	        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
	        
	        newObj.GetChild(0).GetComponent<TMP_Text>().text = meta.Name;
	        newObj.GetChild(1).GetComponent<TMP_Text>().text = meta.CreatedAt.ToShortDateString() + "\n" + meta.CreatedAt.ToLongTimeString();
	        newObj.GetChild(2).GetComponent<Image>().sprite = sprite;
	        newObj.GetChild(3).GetComponent<Button>().onClick.AddListener(() => DeleteSave(meta.Id, newObj.gameObject));
	        newObj.GetChild(4).GetComponent<Button>().onClick.AddListener(() =>
		        _globalData.UpdateAllData(_manualSaveSystem.LoadManualSave(meta.Id, _globalData.SavableData)));
        }

        private void DeleteSave(string id, GameObject uiSave)
        {
			_manualSaveSystem.DeleteManualSave(id);
			Destroy(uiSave);
        }
    }
}