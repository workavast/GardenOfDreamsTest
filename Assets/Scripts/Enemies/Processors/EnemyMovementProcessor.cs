using GameCode.Core;
using UnityEngine;

namespace GameCode.Enemies.Processors
{
    public class EnemyMovementProcessor
    {
        private readonly EnemyBase _enemyBase;
        private readonly TriggerZone _followZone;
        private readonly EnemyAttackProcessor _enemyAttackProcessor;
        private readonly Rigidbody2D _rigidbody2D;
        private readonly GameObject _model;
        
        private float _moveSpeed;
        private bool _isTargetNull;
        private Player _target;
        private bool _lookRight = true;

        public EnemyMovementProcessor(EnemyBase enemyBase, GameObject model, Rigidbody2D rigidbody2D, TriggerZone followZone, EnemyAttackProcessor enemyAttackProcessor, float moveSpeed)
        {
            _enemyBase = enemyBase;
            _model = model;
            _rigidbody2D = rigidbody2D;
            _followZone = followZone;
            _enemyAttackProcessor = enemyAttackProcessor;
            _moveSpeed = moveSpeed;
            
            followZone.OnEnter += CheckEnterInFollowZone;
            followZone.OnExit += CheckExitFromFollowZone;
            
            _isTargetNull = true;
        }
        
        public void ManualFixedUpdate(float time)
        {
            TryMove(time);
        }
        
        private void TryMove(float time)
        {
            if(_isTargetNull || _enemyAttackProcessor.TargetInAttackZone)
                return;
            
            Move((_target.transform.position - _enemyBase.transform.position).normalized, time);
        }
        
        private void Move(Vector2 direction, float time)
        {
            TryFlipModel(direction);
            _rigidbody2D.MovePosition((Vector2)_enemyBase.transform.position + direction * (_moveSpeed * time));
        }
        
        private void CheckEnterInFollowZone(Collider2D collider)
        {
            if (collider.TryGetComponent(out Player player))
            {
                _target = player;
                _isTargetNull = false;
            }
        }

        private void CheckExitFromFollowZone(Collider2D collider)
        {
            if (collider.TryGetComponent(out Player player))
            {
                _target = null;
                _isTargetNull = true;
            }
        }
        
        private void TryFlipModel(Vector2 moveDirection)
        {
            if (moveDirection.x < 0 && _lookRight)
            {
                _lookRight = false;
                _model.transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                if (moveDirection.x > 0 && !_lookRight)
                {
                    _lookRight = true;
                    _model.transform.localScale = new Vector2(1, 1);
                }
            }
        }

    }
}