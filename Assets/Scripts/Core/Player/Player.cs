using System;
using System.Collections.Generic;
using CustomTimer;
using GameCode.Enemies;
using GameCode.Saves;
using SomeStorages;
using UnityEngine;
using Zenject;

namespace GameCode.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : EntityBase, IDamageable
    {
        [SerializeField] private float attackDamage;
        [SerializeField] private float attackCooldown;
        [SerializeField] private float reloadTime;
        [SerializeField] private float attackByLookDistance;
        [SerializeField] private LayerMask attackByLookLayers;
        [SerializeField] private TriggerZone attackZone;
        [SerializeField] private TriggerZone itemsPickUpZone;
        [SerializeField] private Transform shootPoint;
        
        [Inject] private readonly Inventory _inventory;

        private PickUpProcessor _pickUpProcessor;
        private AttackProcessor _attackProcessor;
        
        private Rigidbody2D _rigidbody2D; 
        public Vector2 LookDirection { get; private set; }
        private Vector2 _moveDirection;
        private bool _attack;

        public event Action OnDeath;
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            _healthPoints = new FloatStorage(healthPoints, healthPoints);
            
            _pickUpProcessor = new PickUpProcessor(itemsPickUpZone, _inventory);
            _attackProcessor = new AttackProcessor(this, shootPoint, attackZone, attackDamage, attackCooldown,
                reloadTime, attackByLookDistance, attackByLookLayers);
        }
        
        private void Update()
            => _attackProcessor.ManualUpdate(Time.deltaTime);

        private void FixedUpdate()
            => Move(Time.fixedDeltaTime);

        public void SetInput(Vector2 direction)
            => LookDirection = _moveDirection = direction;

        public void ResetInput()
            => _moveDirection = Vector2.zero;

        public void SetAttack(bool setAttack)
            => _attackProcessor.SetAttack(setAttack);
        
        public void TakeDamage(float damage)
        {
            _healthPoints.ChangeCurrentValue(-damage);

            if (_healthPoints.IsEmpty)
            {
                OnDeath?.Invoke();
            }
        }
        
        public void LoadSave(PlayerSave playerSave)
        {
            var healthPointsPercentage = playerSave.HealthPointsFillingPercentage;
            if (healthPointsPercentage <= 0)
                healthPointsPercentage = 1;
            _healthPoints.SetCurrentValue(_healthPoints.MaxValue * healthPointsPercentage);
        }
        
        private void Move(float fixedDeltaTime)
        {
            _rigidbody2D.MovePosition((Vector2)transform.position + _moveDirection * (moveSpeed * fixedDeltaTime));
        }
    }
}
