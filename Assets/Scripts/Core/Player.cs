using System;
using System.Collections.Generic;
using CustomTimer;
using GameCode.CollectableItems;
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
        [SerializeField] private float attackByLookDistance;
        [SerializeField] private LayerMask attackByLookLayers;
        [SerializeField] private TriggerZone attackZone;
        [SerializeField] private TriggerZone itemsPickUpZone;
        [SerializeField] private Transform shootPoint;
        
        [Inject] private readonly Inventory _inventory;
        
        private readonly List<EnemyBase> _enemiesInAttackZone = new();
        
        private Rigidbody2D _rigidbody2D;
        private Timer _attackCooldown;
        private Vector2 _lookDirection;
        private Vector2 _moveDirection;
        private bool _attack;

        public event Action OnDeath;
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            _healthPoints = new FloatStorage(healthPoints, healthPoints);
            _attackCooldown = new Timer(attackCooldown, attackCooldown);
            
            _attackCooldown.OnTimerEnd += TryAttack;

            attackZone.OnColliderEnter += OnEnterInAttackZone;
            attackZone.OnColliderExit += OnExitFromAttackZone;

            itemsPickUpZone.OnColliderEnter += OnEnterInItemsPickUpZone;
        }
        
        private void Update()
        {
            _attackCooldown.Tick(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            Move(Time.fixedDeltaTime);
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

        public void SetInput(Vector2 direction)
        {
            _lookDirection = _moveDirection = direction;
        }

        public void ResetInput()
        {
            _moveDirection = Vector2.zero;
        }

        public void SetAttack(bool setAttack)
        {
            if (setAttack)
            {
                _attack = true;
                if(_attackCooldown.TimerIsEnd)
                    TryAttack();
            }
            else
            {
                _attack = false;
            }
        }
        
        public void TakeDamage(float damage)
        {
            _healthPoints.ChangeCurrentValue(-damage);

            if (_healthPoints.IsEmpty)
            {
                OnDeath?.Invoke();
            }
        }
        
        private void TryAttack()
        {
            if(!_attack)
                return;
            
            if(_enemiesInAttackZone.Count > 0)
                AttackNearestEnemy();
            else
                AttackByLookDirection();
            
            _attackCooldown.Reset();
        }
        
        private void AttackNearestEnemy()
        {
            EnemyBase targetEnemy = _enemiesInAttackZone[0];
            float currentDistance = Vector3.Distance(transform.position, targetEnemy.transform.position);
            for (int i = 1; i < _enemiesInAttackZone.Count; i++)
            {
                var distance = Vector3.Distance(transform.position, _enemiesInAttackZone[i].transform.position);
                if (distance < currentDistance)
                {
                    targetEnemy = _enemiesInAttackZone[i];
                    currentDistance = distance;
                }
            }
            
            targetEnemy.TakeDamage(attackDamage);
        }

        private void AttackByLookDirection()
        {
            var result = Physics2D.Raycast(shootPoint.position, _lookDirection, attackByLookDistance, attackByLookLayers);
            
            Debug.DrawRay(shootPoint.position, _lookDirection * attackByLookDistance, Color.black, 4);
            if (result && result.transform.TryGetComponent(out EnemyBase targetEnemy))
                targetEnemy.TakeDamage(attackDamage);
        }
        
        private void OnEnterInAttackZone(Collider2D other)
        {
            if (other.TryGetComponent(out EnemyBase enemy))
            {
                if (_enemiesInAttackZone.Contains(enemy))
                {
                    Debug.LogWarning($"Enemy already enter in zone");
                    return;
                }
                
                Debug.LogWarning($"Enemy enter in zone");
                _enemiesInAttackZone.Add(enemy);
                
                if(_attackCooldown.TimerIsEnd)
                    TryAttack();
            }
        }

        private void OnExitFromAttackZone(Collider2D other)
        {
            if (other.TryGetComponent(out EnemyBase enemy))
                _enemiesInAttackZone.Remove(enemy);
        }

        private void OnEnterInItemsPickUpZone(Collider2D other)
        {
            if (other.TryGetComponent(out CollectableItem item))
            {
                var freeStack = _inventory.ItemFreeStackValue(item.ItemConfig.ItemId);
                Debug.Log($"item count: {item.ItemCount}");
                
                if (freeStack.Item1 <= -1)
                {
                    if (_inventory.HasEmptySlot())
                    {
                        Debug.Log("PickUp new item");
                        var itemCount = item.ItemCount;
                        var itemConfig = item.PickUp(item.ItemCount);
                        _inventory.AddItem(itemConfig, itemCount); 
                    }
                }
                else
                {
                    Debug.Log("PickUp extract item");
                    var pickUpCount = Mathf.Clamp(item.ItemCount, 0, freeStack.Item2);
                    var itemConfig = item.PickUp(pickUpCount);
                    _inventory.ChangeItemCount(freeStack.Item1, pickUpCount);

                    if (item.ItemCount > 0 && _inventory.HasEmptySlot())
                    {
                        Debug.Log($"PickUp new item after extract {item.ItemCount}");
                        var itemCount = item.ItemCount;
                        item.PickUp(itemCount);
                        _inventory.AddItem(itemConfig, itemCount);
                    }
                }
            }
        }
    }
}
