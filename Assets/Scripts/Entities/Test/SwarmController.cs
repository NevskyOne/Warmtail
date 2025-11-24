using System.Collections.Generic;
using UnityEngine;

namespace Systems.Swarm
{
    public class SwarmController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float _speed = 8f;
        [SerializeField] private float _rotationSpeed = 10f;

        [Header("Swarm Settings")]
        [SerializeField] private BoidAgent _boidPrefab;
        [SerializeField] private int _count = 15;
        [SerializeField] private float _spawnRadius = 2f;

        [Header("Boid Rules")]
        public float CohesionWeight = 1f;
        public float AlignmentWeight = 1f;
        public float SeparationWeight = 1.5f;
        public float TargetWeight = 2f;
        public float NeighborRadius = 3f;

        private readonly List<BoidAgent> _agents = new();
        private bool _isAggressive;
        private bool _isHeating;

        public void Initialize()
        {
            for (int i = 0; i < _count; i++)
            {
                var pos = (Vector2)transform.position + Random.insideUnitCircle * _spawnRadius;
                var boid = Instantiate(_boidPrefab, pos, Quaternion.identity, transform);
                boid.Initialize(this);
                _agents.Add(boid);
            }
            gameObject.SetActive(false);
        }

        public void Activate(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
            _agents.ForEach(a => {
                a.transform.position = position + (Vector3)Random.insideUnitCircle;
                a.gameObject.SetActive(true);
            });
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void Move(Vector2 input)
        {
            if (input.sqrMagnitude > 0.01f)
            {
                var targetVelocity = input.normalized * _speed;
                _rb.linearVelocity = Vector2.Lerp(_rb.linearVelocity, targetVelocity, Time.fixedDeltaTime * 5f);
                
                float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
                _rb.rotation = Mathf.LerpAngle(_rb.rotation, angle, Time.fixedDeltaTime * _rotationSpeed);
            }
            else
            {
                _rb.linearVelocity = Vector2.Lerp(_rb.linearVelocity, Vector2.zero, Time.fixedDeltaTime * 2f);
            }
        }

        public void SetState(bool aggressive, bool heating)
        {
            _isAggressive = aggressive;
            _isHeating = heating;
            // Тут можно менять цвет рыбок
        }

        // Вызывается агентами для получения соседей (оптимизация: можно использовать Spatial Hash Grid)
        public List<BoidAgent> GetNeighbors(BoidAgent agent)
        {
            return _agents; 
        }
    }
}