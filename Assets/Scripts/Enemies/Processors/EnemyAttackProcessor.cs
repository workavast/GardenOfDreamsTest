using CustomTimer;
using GameCode.Core;
using UnityEngine;

namespace GameCode.Enemies.Processors
{
    public class EnemyAttackProcessor
    {
        private readonly EnemyBase _enemyBase;
        private readonly Timer _attackCooldown;
        private readonly TriggerZone _attackZone;
        
        private float _attackDamage;
        private Player _target;
        
        public bool TargetInAttackZone { get; private set; }

        public EnemyAttackProcessor(EnemyBase enemyBase, TriggerZone attackZone, float attackDamage, float attackCooldown )
        {
            _enemyBase = enemyBase;
            _attackZone = attackZone;

            _attackDamage = attackDamage;
            _attackCooldown = new Timer(attackCooldown);

            _attackCooldown.OnTimerEnd += TryAttack;
            
            attackZone.OnEnter += CheckPlayerEnterInAttackZone;
            attackZone.OnExit += CheckPlayerExitAttackZone;
        }
        
        public void ManualUpdate(float time)
        {
            _attackCooldown.Tick(time);
        }
        
        private void TryAttack()
        {
            if(!TargetInAttackZone)
                return;
            
            _target.TakeDamage(_attackDamage);
            _attackCooldown.Reset();
        }
        
        private void CheckPlayerEnterInAttackZone(Collider2D collider)
        {
            if (collider.TryGetComponent(out Player player))
            {
                _target = player;
                TargetInAttackZone = true;
                if(_attackCooldown.TimerIsEnd)
                    TryAttack();
            }
        }

        private void CheckPlayerExitAttackZone(Collider2D collider)
        {
            if (collider.TryGetComponent(out Player player))
            {
                _target = null;
                TargetInAttackZone = false;
            }
        }  
    }
}