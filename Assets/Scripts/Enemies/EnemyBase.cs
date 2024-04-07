using System;
using GameCode.Core;
using GameCode.Enemies.Processors;
using SomeStorages;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameCode.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class EnemyBase : EntityBase, ITarget
    {
        [SerializeField] private float attackDamage;
        [SerializeField] private float attackCooldown;
        [SerializeField] private TriggerZone followZone;
        [SerializeField] private TriggerZone attackZone;
        [SerializeField] private Transform aimPoint;
        [SerializeField] private GameObject model;
        [Space]
        [SerializeField] private float dropChance;
        [SerializeField] private ItemId[] dropList;
        
        private EnemyMovementProcessor _movementProcessor;
        private EnemyAttackProcessor _attackProcessor;
        private Rigidbody2D _rigidbody2D;
        
        public abstract EnemyType EnemyType { get; }
        public Transform AimPoint => aimPoint;
        
        /// <summary>
        /// return item id and item drop position
        /// </summary>
        public event Action<ItemId, Vector2> OnDropItem; 
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            _healthPoints = new FloatStorage(healthPoints, healthPoints);
            
            //need create final state machine
            _attackProcessor = new EnemyAttackProcessor(this, attackZone, attackDamage, attackCooldown);
            _movementProcessor = new EnemyMovementProcessor(this, model, _rigidbody2D, followZone, _attackProcessor, moveSpeed);
        }
        
        public void Update()
            => _attackProcessor.ManualUpdate(Time.deltaTime);
            
        public void FixedUpdate()
            => _movementProcessor.ManualFixedUpdate(Time.fixedDeltaTime);
        
        public override void TakeDamage(float damage)
        {
            _healthPoints.ChangeCurrentValue(-damage);

            if (_healthPoints.IsEmpty)
            {
                var randomDropChance = Random.value;

                if (randomDropChance <= dropChance)
                {
                    var itemDropId = Random.Range(0, dropList.Length);
                    OnDropItem?.Invoke(dropList[itemDropId], transform.position);
                }
                
                Destroy(gameObject);
            }
        }
    }
}