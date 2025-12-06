using UnityEngine;

namespace Systems.Swarm
{
    public class BoidAgent : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float _maxSpeed = 10f;
        [SerializeField] private float _steerForce = 5f;

        private SwarmController _controller;

        public void Initialize(SwarmController controller)
        {
            _controller = controller;
        }

        private void FixedUpdate()
        {
            if (_controller == null) return;

            var neighbors = _controller.GetNeighbors(this);
            
            Vector2 cohesion = Vector2.zero;
            Vector2 alignment = Vector2.zero;
            Vector2 separation = Vector2.zero;
            int count = 0;

            foreach (var boid in neighbors)
            {
                if (boid == this) continue;
                float dist = Vector2.Distance(transform.position, boid.transform.position);
                
                if (dist < _controller.NeighborRadius)
                {
                    cohesion += (Vector2)boid.transform.position;
                    alignment += boid._rb.linearVelocity;
                    separation += (Vector2)(transform.position - boid.transform.position) / (dist * dist);
                    count++;
                }
            }

            Vector2 acceleration = Vector2.zero;
            if (count > 0)
            {
                cohesion = ((cohesion / count) - (Vector2)transform.position).normalized;
                alignment = (alignment / count).normalized;
                separation = separation.normalized;

                acceleration += cohesion * _controller.CohesionWeight;
                acceleration += alignment * _controller.AlignmentWeight;
                acceleration += separation * _controller.SeparationWeight;
            }

            Vector2 targetDir = (_controller.transform.position - transform.position).normalized;
            acceleration += targetDir * _controller.TargetWeight;

            _rb.linearVelocity += acceleration * Time.fixedDeltaTime * _steerForce;
            _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, _maxSpeed);
            
            if (_rb.linearVelocity != Vector2.zero)
                transform.up = _rb.linearVelocity;
        }
    }
}
