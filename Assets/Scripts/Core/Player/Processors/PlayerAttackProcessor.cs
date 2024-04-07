using System.Collections.Generic;
using CustomTimer;
using SomeStorages;
using UnityEngine;

namespace GameCode.Core
{
    public class PlayerAttackProcessor : IReadOnlyPlayerAttackProcessor
    {
        private readonly Player _player;
        private readonly Transform _shootPoint;
        private readonly TriggerZone _attackZone;
        private readonly List<ITarget> _targetsInAttackZone = new();
        private readonly Timer _attackCooldown;
        private readonly Timer _reloadTimer;
        private readonly LayerMask _attackByLookLayers;
        private readonly IntStorage _magazineCounter;
        
        private float _attackDamage;
        private float _attackByLookDistance;
        private bool _isAttack;

        public IReadOnlySomeStorage<int> MagazineCounter => _magazineCounter;
        
        public PlayerAttackProcessor(Player player, Transform shootPoint, TriggerZone attackZone, float attackDamage, 
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
            
            _attackZone.OnEnter += OnEnterInAttackZone;
            _attackZone.OnExit += OnExitFromAttackZone;
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
            
            if(_targetsInAttackZone.Count > 0)
                AttackNearestEnemy();
            else
                AttackByAimDirection(_player.LookDirection);
            
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
            var aimTarget = _targetsInAttackZone[0];
            float currentDistance = Vector3.Distance(_player.transform.position, aimTarget.AimPoint.transform.position);
            for (int i = 1; i < _targetsInAttackZone.Count; i++)
            {
                var distance = Vector3.Distance(_player.transform.position, _targetsInAttackZone[i].AimPoint.transform.position);
                if (distance < currentDistance)
                {
                    aimTarget = _targetsInAttackZone[i];
                    currentDistance = distance;
                }
            }
            
            AttackByAimDirection((aimTarget.AimPoint.position- _shootPoint.position).normalized);
        }

        private void AttackByAimDirection(Vector2 aimDirection)
        {
            var result = Physics2D.Raycast(_shootPoint.position, aimDirection, _attackByLookDistance, _attackByLookLayers);
            
            Debug.DrawRay(_shootPoint.position, aimDirection * _attackByLookDistance, Color.black, 4);
            if (result && result.transform.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(_attackDamage);
        }
        
        private void OnEnterInAttackZone(Collider2D other)
        {
            if (other.TryGetComponent(out ITarget target))
            {
                if (_targetsInAttackZone.Contains(target))
                {
                    Debug.LogWarning($"Target already enter in zone");
                    return;
                }
                
                _targetsInAttackZone.Add(target);
                
                if(_attackCooldown.TimerIsEnd)
                    TryAttack();
            }
        }

        private void OnExitFromAttackZone(Collider2D other)
        {
            if (other.TryGetComponent(out ITarget target))
                _targetsInAttackZone.Remove(target);
        } 
    }
}