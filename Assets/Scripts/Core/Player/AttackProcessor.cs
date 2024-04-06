using System.Collections.Generic;
using CustomTimer;
using GameCode.Enemies;
using SomeStorages;
using UnityEngine;

namespace GameCode.Core
{
    public class AttackProcessor : IReadOnlyAttackProcessor
    {
        private readonly Player _player;
        private readonly Transform _shootPoint;
        private readonly TriggerZone _attackZone;
        private readonly List<EnemyBase> _enemiesInAttackZone = new();
        private readonly Timer _attackCooldown;
        private readonly Timer _reloadTimer;
        private readonly LayerMask _attackByLookLayers;
        private readonly IntStorage _magazineCounter;
        
        private float _attackDamage;
        private float _attackByLookDistance;
        private bool _isAttack;

        public IReadOnlySomeStorage<int> MagazineCounter => _magazineCounter;
        
        public AttackProcessor(Player player, Transform shootPoint, TriggerZone attackZone, float attackDamage, 
            float attackCooldown, float reloadTime, float attackByLookDistance, LayerMask attackByLookLayers, int magazineSize)
        {
            _player = player;
            _shootPoint = shootPoint;
            _attackZone = attackZone;
            _attackDamage = attackDamage;
            _attackCooldown = new Timer(attackCooldown, attackCooldown);
            _reloadTimer = new Timer(reloadTime, reloadTime);
            _attackByLookDistance = attackByLookDistance;
            _attackByLookLayers = attackByLookLayers;
            _magazineCounter = new IntStorage(magazineSize, magazineSize);
                
            _attackCooldown.OnTimerEnd += TryAttack;
            _reloadTimer.OnTimerEnd += OnReloadEnd;
            
            _attackZone.OnColliderEnter += OnEnterInAttackZone;
            _attackZone.OnColliderExit += OnExitFromAttackZone;
        }

        public void ManualUpdate(float time)
        {
            _attackCooldown.Tick(time);
            _reloadTimer.Tick(time);
        }
        
        public void SetAttack(bool setAttack)
        {
            if (setAttack)
            {
                _isAttack = true;
                if(_attackCooldown.TimerIsEnd)
                    TryAttack();
            }
            else
            {
                _isAttack = false;
            }
        }
        
        private void TryAttack()
        {
            if(!_isAttack)
                return;
            
            if(_enemiesInAttackZone.Count > 0)
                AttackNearestEnemy();
            else
                AttackByLookDirection();
            
            _magazineCounter.ChangeCurrentValue(-1);

            if (_magazineCounter.IsEmpty)
                _reloadTimer.Reset();
            else
                _attackCooldown.Reset();
        }

        private void OnReloadEnd()
        {
            _magazineCounter.SetCurrentValue(_magazineCounter.MaxValue);
            _attackCooldown.Reset();
        }
        
        private void AttackNearestEnemy()
        {
            EnemyBase targetEnemy = _enemiesInAttackZone[0];
            float currentDistance = Vector3.Distance(_player.transform.position, targetEnemy.transform.position);
            for (int i = 1; i < _enemiesInAttackZone.Count; i++)
            {
                var distance = Vector3.Distance(_player.transform.position, _enemiesInAttackZone[i].transform.position);
                if (distance < currentDistance)
                {
                    targetEnemy = _enemiesInAttackZone[i];
                    currentDistance = distance;
                }
            }
            
            targetEnemy.TakeDamage(_attackDamage);
        }

        private void AttackByLookDirection()
        {
            var result = Physics2D.Raycast(_shootPoint.position, _player.LookDirection, _attackByLookDistance, _attackByLookLayers);
            
            Debug.DrawRay(_shootPoint.position, _player.LookDirection * _attackByLookDistance, Color.black, 4);
            if (result && result.transform.TryGetComponent(out EnemyBase targetEnemy))
                targetEnemy.TakeDamage(_attackDamage);
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
    }
}