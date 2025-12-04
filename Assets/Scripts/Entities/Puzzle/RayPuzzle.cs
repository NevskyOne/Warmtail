using UnityEngine.Events;
using UnityEngine;

namespace Entities.Puzzle
{
    public class RayPuzzle : MonoBehaviour
    {
        public UnityEvent OnSolved = new();

        public void Start()
        {
        }
        public void Reset()
        {
        }
        public void Solve()
        {
            OnSolved.Invoke();
            Debug.Log("RayPuzzle выполнено");
            Invoke("DestroyPuzzle", 0.5f);
        }

        private void DestroyPuzzle()
        {
            Destroy(gameObject);
        }
    }
}
