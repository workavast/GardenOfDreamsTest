using System;
using GameCode.Saves;
using SomeStorages;
using UnityEngine;
using Zenject;

namespace GameCode.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : EntityBase
    {
        [SerializeField] private float attackDamage;
        [SerializeField] private float attackCooldown;
        [SerializeField] [Range(0, 100)] private int magazineSize;
        [SerializeField] private float reloadTime;
        [SerializeField] private float attackByLookDistance;
        [SerializeField] private LayerMask attackByLookLayers;
        [SerializeField] private TriggerZone attackZone;
        [SerializeField] private TriggerZone itemsPickUpZone;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private GameObject model;
        [SerializeField] private GameObject weapon;
        
        [Inject] private readonly Inventory _inventory;

        private PlayerPickUpProcessor _pickUpProcessor;
        private Rigidbody2D _rigidbody2D; 
        private Vector2 _moveDirection;
        private bool _lookRight = true;

        public PlayerAttackProcessor AttackProcessor { get; private set; }
        public Vector2 LookDirection { get; private set; }
        
        public event Action OnDeath;
        public event Action OnSetMoveInput;
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            _healthPoints = new FloatStorage(healthPoints, healthPoints);
            
            _pickUpProcessor = new PlayerPickUpProcessor(itemsPickUpZone, _inventory);
            AttackProcessor = new PlayerAttackProcessor(this, shootPoint, attackZone, attackDamage, attackCooldown,
                reloadTime, attackByLookDistance, attackByLookLayers, magazineSize);

            AttackProcessor.OnTargetEnterInAttackZone += SetLookByAim;
            AttackProcessor.OnAllTargeExitFromtAttackZone += SetLookByMove;

            SetLookByMove();
        }
        
        private void Update()
        {
            AttackProcessor.ManualUpdate(Time.deltaTime);
        }

        private void FixedUpdate()
            => Move(Time.fixedDeltaTime);

        public void SetMoveInput(Vector2 direction)
        {
            _moveDirection = direction;
            OnSetMoveInput?.Invoke();
        }

        public void ResetMoveInput()
            => _moveDirection = Vector2.zero;

        public void SetAttack(bool setAttack)
            => AttackProcessor.SetAttack(setAttack);
        
        public override void TakeDamage(float damage)
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

        private void SetLookByMove()
        {
            OnSetMoveInput += UpdateLookByMove;
            AttackProcessor.OnTargetStayInAttackZone -= UpdateLookByAim;
        }

        private void SetLookByAim()
        {
            OnSetMoveInput -= UpdateLookByMove;
            AttackProcessor.OnTargetStayInAttackZone += UpdateLookByAim;
        }
        
        private void UpdateLookByMove()
            => LookDirection = _moveDirection;
                
        private void UpdateLookByAim()
            => LookDirection = AttackProcessor.GetLookDirection();

        private void Move(float fixedDeltaTime)
        {
            TryFlipModel();
            RotateWeapon();
            _rigidbody2D.MovePosition((Vector2)transform.position + _moveDirection * (moveSpeed * fixedDeltaTime));
        }

        private void TryFlipModel()
        {
            if (_moveDirection.x < 0 && _lookRight)
            {
                _lookRight = false;
                model.transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                if (_moveDirection.x > 0 && !_lookRight)
                {
                    _lookRight = true;
                    model.transform.localScale = new Vector2(1, 1);
                }
            }
        }

        private void RotateWeapon()
        {
            var angle = 0f;
            
            if(_lookRight)
                angle = Vector2.Angle(Vector2.right, LookDirection);
            else
                angle = Vector2.Angle(Vector2.left, LookDirection);
            
            weapon.transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Sign(LookDirection.y));
        }
    }
}
