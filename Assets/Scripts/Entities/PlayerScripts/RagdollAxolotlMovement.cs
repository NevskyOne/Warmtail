using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities.PlayerScripts
{
    public class RagdollAxolotlMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _moveForce = 10f;
        [SerializeField] private float _maxSpeed = 5f;
        [SerializeField] private float _drag = 5f;
        
        [Header("Leg Animation")]
        [SerializeField] private float _legSwingSpeed = 10f;
        [SerializeField] private Rigidbody2D[] _legBodies;
        
        [Header("Ragdoll Setup")]
        [SerializeField] private Rigidbody2D _mainRigidbody;
        
        private Vector2 _moveInput;
        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private Rigidbody2D[] _allRagdollBodies;
        private List<LegInfo> _legs = new List<LegInfo>();
        private float _legAnimationTime = 0f;
        
        private class LegInfo
        {
            public Rigidbody2D Body;
            public HingeJoint2D Joint;
            public float MinAngle;
            public float MaxAngle;
            public float CenterAngle;
            
            public LegInfo(Rigidbody2D body, HingeJoint2D joint)
            {
                
                if (joint != null && joint.useLimits)
                {
                    MinAngle = joint.limits.min;
                    MaxAngle = joint.limits.max;
                    CenterAngle = (MinAngle + MaxAngle) / 2f;
                }
                else
                {
                    Debug.Log("1233");
                }
            }
        }

        private void Awake()
        {
            if (_mainRigidbody == null)
            {
                _mainRigidbody = GetComponent<Rigidbody2D>();
                
                if (_mainRigidbody == null)
                {
                    _mainRigidbody = GetComponentInChildren<Rigidbody2D>();
                }
                
                if (_mainRigidbody == null)
                {
                    enabled = false;
                    return;
                }
            }
            _mainRigidbody.linearDamping = _drag;
            _mainRigidbody.angularDamping = _drag;
            
            SetupLegs();
            
        }
        
        private void SetupLegs()
        {
            _legs.Clear();
            
            if (_legBodies != null && _legBodies.Length > 0)
            {
                foreach (var legBody in _legBodies)
                {
                    if (legBody != null)
                    {
                        HingeJoint2D joint = legBody.GetComponent<HingeJoint2D>();
                        if (joint == null)
                        {
                            joint = legBody.transform.parent?.GetComponent<HingeJoint2D>();
                        }
                        
                        LegInfo legInfo = new LegInfo(legBody, joint);
                        _legs.Add(legInfo);
                    }
                }
            }
        }
       
        
        private void FindAllRagdollBodies()
        {
            Rigidbody2D[] allBodies = GetComponentsInChildren<Rigidbody2D>();
            
            List<Rigidbody2D> additionalList = new List<Rigidbody2D>();
            foreach (var body in allBodies)
            {
                if (body != _mainRigidbody && body != null)
                {
                    additionalList.Add(body);
                }
            }
            
            _allRagdollBodies = additionalList.ToArray();
            foreach (var body in _allRagdollBodies)
            {
                if (body != null)
                {
                    body.linearDamping = _drag;
                    body.angularDamping = _drag;
                }
            }
        }

        private void OnEnable()
        {
            _playerInput = FindFirstObjectByType<PlayerInput>();
            
            if (_playerInput != null)
            {
                _moveAction = _playerInput.actions["Move"];
                if (_moveAction != null)
                {
                    _moveAction.performed += OnMove;
                    _moveAction.canceled += OnMoveCanceled;
                }
            }
        }

        private void OnDisable()
        {
            if (_moveAction != null)
            {
                _moveAction.performed -= OnMove;
                _moveAction.canceled -= OnMoveCanceled;
            }
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
            AnimateLegs();
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            _moveInput = Vector2.zero;
        }

        private void FixedUpdate()
        {
            if (_mainRigidbody == null) return;
            
            if (_mainRigidbody.linearVelocity.magnitude > _maxSpeed)
            {
                _mainRigidbody.linearVelocity = _mainRigidbody.linearVelocity.normalized * _maxSpeed;
            }
            if (_moveInput.magnitude > 0.1f)
            {
                Vector2 force = _moveInput.normalized * _moveForce;
                
                _mainRigidbody.AddForce(force, ForceMode2D.Force);

                float targetAngle = Mathf.Atan2(_moveInput.y, _moveInput.x) * Mathf.Rad2Deg ;
                float currentAngle = _mainRigidbody.rotation;
                float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, 5f * Time.fixedDeltaTime);
                _mainRigidbody.MoveRotation(newAngle);
                
                if (_allRagdollBodies != null && _allRagdollBodies.Length > 0)
                {
                    foreach (var body in _allRagdollBodies)
                    {
                        if (body != null && body != _mainRigidbody)
                        {
                            body.AddForce(force * 0.3f, ForceMode2D.Force);
                            
                            float bodyCurrentAngle = body.rotation;
                            float bodyNewAngle = Mathf.LerpAngle(bodyCurrentAngle, targetAngle, 5f * Time.fixedDeltaTime);
                            body.MoveRotation(bodyNewAngle);
                        }
                    }
                }
                
                AnimateLegs();
            }
        }
        
        private void AnimateLegs()
        {
            if (_legs == null || _legs.Count == 0) return;
            
            _legAnimationTime += Time.fixedDeltaTime * _legSwingSpeed;
        }
    }
}

