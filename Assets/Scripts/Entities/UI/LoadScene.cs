using Data;
using Entities.Core;
using Systems.DataSystems;
using UnityEngine;
using Zenject;

namespace Entities.UI
{
    public class LoadScene : MonoBehaviour
    {
        private SceneLoader _sceneLoader;
        private SaveSystem _saveSystem;
        
        [Inject]
        private void Construct(SceneLoader sceneLoader, SaveSystem saveSystem)
        {
            _sceneLoader = sceneLoader;
            _saveSystem = saveSystem;
        }

        public void Load(string index)
        {
            _saveSystem.SaveAllToDisk();
            _sceneLoader.StartSceneProcess(index);
        }
    }
}