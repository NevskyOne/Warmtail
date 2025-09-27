using UnityEngine;
using UnityEngine.SceneManagement;

namespace Entities.Core
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private int _sceneBetween;

        private AsyncOperation _asyncLoad;
        
        public async void StartSceneProcess()
        {
            _asyncLoad = SceneManager.LoadSceneAsync(_sceneBetween);
            _asyncLoad!.allowSceneActivation = false;

            await _asyncLoad;
            //TODO: ui shit
        }

        public void EnableScene()
        {
            if(_asyncLoad is { isDone: true })
                _asyncLoad.allowSceneActivation = true;
        }
    }
}