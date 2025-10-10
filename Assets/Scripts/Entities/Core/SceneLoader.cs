using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Entities.Core
{
    public class SceneLoader : MonoBehaviour
    {
        private static readonly int EndTransition = Animator.StringToHash("EndTransition");
        [SerializeField] private Animator _animPrefab;
        [SerializeField, Unit("Milliseconds")] private int _animDuration;

        private AsyncOperation _asyncLoad;

        private void Start()
        {
            DontDestroyOnLoad(this);
        }
        
        public async void StartSceneProcess(string sceneInd)
        {
            var animator = Instantiate(_animPrefab);
            DontDestroyOnLoad(animator);
            
            await UniTask.Delay(_animDuration);
            _asyncLoad = SceneManager.LoadSceneAsync(sceneInd);
            await UniTask.WhenAll(_asyncLoad.ToUniTask(), UniTask.WaitUntil(() => _asyncLoad.allowSceneActivation));
            
            animator.SetTrigger(EndTransition);
            await UniTask.Delay(_animDuration);
            Destroy(animator);
        }
    }
}