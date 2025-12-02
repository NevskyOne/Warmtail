using Systems.Effects;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.U2D;

namespace Entities.Triggers
{
    public class RoomTransitionTrigger : MonoBehaviour
    {
        [SerializeField] private Collider2D[] _showCollider;
        [SerializeField] private Collider2D[] _hideCollider;
        [SerializeField] private SpriteShapeRenderer[] _normalSprites;
        [SerializeField] private SpriteShapeRenderer[] _hiddenSprites;

        private void Start()
        {
            for (int i = 0; i < _showCollider.Length; i++)
            {
                _showCollider[i].OnTriggerEnter2DAsObservable().Subscribe(TriggerEnter2D);
                _hideCollider[i].OnTriggerEnter2DAsObservable().Subscribe(HiddenTriggerEnter2D);
            }
        }
        
        private void TriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                for (int i = 0; i < _hiddenSprites.Length; i++)
                {
                    Fade(_normalSprites[i], _hiddenSprites[i]);
                }
                for (int i = 0; i < _showCollider.Length; i++)
                {
                    _showCollider[i].gameObject.SetActive(false);
                    _hideCollider[i].gameObject.SetActive(true);
                }
            }
        }

        private void HiddenTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                for (int i = 0; i < _hiddenSprites.Length; i++)
                {
                    Fade(_hiddenSprites[i], _normalSprites[i]);
                }
                for (int i = 0; i < _showCollider.Length; i++)
                {
                    _showCollider[i].gameObject.SetActive(true);
                    _hideCollider[i].gameObject.SetActive(false);
                }
            }
        }

        private async void Fade(SpriteShapeRenderer normalSprite, SpriteShapeRenderer hiddenSprite)
        {
            var normalColor = normalSprite.color;
            var hiddenColor = hiddenSprite.color;
            await foreach (var (a, b) in  CrossfadeEffect.CrossfadeTwins(1))
            {
                normalColor.a = a;
                hiddenColor.a = b;
                hiddenSprite.color = hiddenColor;
                normalSprite.color = normalColor;
            }
        }
    }
}