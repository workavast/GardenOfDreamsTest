using System;
using CustomTimer;
using GameCode.Core;
using SomeStorages;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameCode.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class EnemyBase : EntityBase, IDamageable
    {
        [SerializeField] private float attackDamage;
        [SerializeField] private float attackCooldown;
        [SerializeField] private TriggerZone followZone;
        [SerializeField] private TriggerZone attackZone;
        [SerializeField] private Transform aimPoint;

        [SerializeField] private float dropChance;
        [SerializeField] private ItemId[] dropList;
        
        private Rigidbody2D _rigidbody2D;
        private bool _targetInAttackZone;
        private Timer _attackCooldown;
        private Player _target;
        private bool _isTargetNull;

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
            _attackCooldown = new Timer(attackCooldown);

            _attackCooldown.OnTimerEnd += TryAttack;
            
            followZone.OnColliderEnter += CheckPlayerEnterInFollowZone;
            followZone.OnColliderExit += CheckPlayerExitFollowZone;

            attackZone.OnColliderEnter += CheckPlayerEnterInAttackZone;
            attackZone.OnColliderExit += CheckPlayerExitAttackZone;
            
            _isTargetNull = _target == null;
        }
        
        public void Update()
        {
            _attackCooldown.Tick(Time.deltaTime);
        }

        public void FixedUpdate()
        {
            TryMove(Time.fixedDeltaTime);
        }
        
        private void TryMove(float time)
        {
            if(_isTargetNull || _targetInAttackZone)
                return;
            
            Move((_target.transform.position - transform.position).normalized, time);
        }
        
        private void Move(Vector2 direction, float time)
        {
            _rigidbody2D.MovePosition((Vector2)transform.position + direction * moveSpeed * time);
        }
        
        private void TryAttack()
        {
            if(!_targetInAttackZone || _target == null)
                return;
            
            _target.TakeDamage(attackDamage);
            _attackCooldown.Reset();
        }
        
        public void TakeDamage(float damage)
        {
            _healthPoints.ChangeCurrentValue(-damage);

            if (_healthPoints.IsEmpty)
            {
                var randomDropChance = Random.value;

                if (randomDropChance <= dropChance)
                {
                    Debug.Log("item drop");
                    var itemDropId = Random.Range(0, dropList.Length);
                    OnDropItem?.Invoke(dropList[itemDropId], transform.position);
                }
                
                Destroy(gameObject);
            }
        }
        
        private void CheckPlayerEnterInFollowZone(Collider2D collider)
        {
            if (collider.TryGetComponent(out Player player))
            {
                _target = player;
                _isTargetNull = false;
                if(_attackCooldown.TimerIsEnd)
                    TryAttack();
            }
        }

        private void CheckPlayerExitFollowZone(Collider2D collider)
        {
            if (collider.TryGetComponent(out Player player))
            {
                _target = null;
                _isTargetNull = true;
            }
        }
        
        private void CheckPlayerEnterInAttackZone(Collider2D collider)
        {
            if (collider.TryGetComponent(out Player player))
            {
                _targetInAttackZone = true;
                if(_attackCooldown.TimerIsEnd)
                    TryAttack();
            }
        }

        private void CheckPlayerExitAttackZone(Collider2D collider)
        {
            if (collider.TryGetComponent(out Player player))
            {
                _targetInAttackZone = false;
            }
        }    
    }
}