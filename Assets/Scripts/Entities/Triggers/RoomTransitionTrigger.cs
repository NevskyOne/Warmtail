using Systems.Effects;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.U2D;

namespace Entities.Triggers
{
    public class RoomTransitionTrigger : MonoBehaviour
    {
        [SerializeField] private Collider2D _showCollider;
        [SerializeField] private Collider2D _hideCollider;
        [SerializeField] private SpriteShapeRenderer _normalSprite;
        [SerializeField] private SpriteShapeRenderer _hiddenSprite;
        private bool _isHidden;

        private void Start()
        {
            _showCollider.OnTriggerEnter2DAsObservable().Subscribe(TriggerEnter2D);
            _hideCollider.OnTriggerEnter2DAsObservable().Subscribe(HiddenTriggerEnter2D);
        }
        
        private void TriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !_isHidden)
            {
                Fade(_normalSprite, _hiddenSprite);
                _isHidden = true;
                _showCollider.gameObject.SetActive(false);
                _hideCollider.gameObject.SetActive(true);
            }
        }

        private void HiddenTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && _isHidden)
            {
                Fade(_hiddenSprite, _normalSprite);
                _isHidden = false;
                _hideCollider.gameObject.SetActive(false);
                _showCollider.gameObject.SetActive(true);
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