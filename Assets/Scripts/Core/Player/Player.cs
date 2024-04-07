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

        [Inject] private readonly Inventory _inventory;

        private PlayerPickUpProcessor _pickUpProcessor;
        public PlayerAttackProcessor AttackProcessor { get; private set; }
        
        private Rigidbody2D _rigidbody2D; 
        public Vector2 LookDirection { get; private set; }
        private Vector2 _moveDirection;
        private bool _lookRight = true;
        
        public event Action OnDeath;
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            _healthPoints = new FloatStorage(healthPoints, healthPoints);
            
            _pickUpProcessor = new PlayerPickUpProcessor(itemsPickUpZone, _inventory);
            AttackProcessor = new PlayerAttackProcessor(this, shootPoint, attackZone, attackDamage, attackCooldown,
                reloadTime, attackByLookDistance, attackByLookLayers, magazineSize);
        }
        
        private void Update()
            => AttackProcessor.ManualUpdate(Time.deltaTime);

        private void FixedUpdate()
            => Move(Time.fixedDeltaTime);

        public void SetInput(Vector2 direction)
            => LookDirection = _moveDirection = direction;

        public void ResetInput()
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
        
        private void Move(float fixedDeltaTime)
        {
            TryFlipModel();
            
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
    }
}
