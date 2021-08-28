using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MovingLivingEntity
{
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Idle = Animator.StringToHash("Idle");
    
    [SerializeField] private PlayerSwordAttack SwordAttack;
    [SerializeField] private PlayerDashAttack DashAttack;
    [SerializeField] private PlayerRangedAttack RangedAttack;
    [SerializeField] private LayerMask EnemyLayerMask;
    
    private bool IsDashing;
    
    private Vector3 Direction;
    private Vector3 DashDirection;
    private Vector3 LastAnimationDirection;

    private ContactFilter2D ContactFilter2D;
    private PlayerAttack CurrentAttack;

    public Vector3 LookDirection => LastAnimationDirection;

    protected override void Start()
    {
        base.Start();
        CurrentAttack = SwordAttack;
        
        SwordAttack.Init(Animator);
        DashAttack.Init(Animator);
        RangedAttack.Init(Animator);

        ContactFilter2D = new ContactFilter2D
        {
            layerMask = EnemyLayerMask,
            useLayerMask = true
        };
    }

    public void TrySwordAttack()
    {
        if (!SwordAttack.CanAttack)
            return;

        CurrentAttack = SwordAttack;
        SwordAttack.Attack();
        StartCoroutine(DealDamageWithDelay());
    }

    private IEnumerator DealDamageWithDelay()
    {
        yield return new WaitForSeconds(SwordAttack.AttackDelay);
        
        var collidersList = new List<Collider2D>();
        Physics2D.OverlapCircle((Vector2)transform.position + (Vector2)LastAnimationDirection/2f,
            SwordAttack.AttackRadius, ContactFilter2D, collidersList);

        foreach (var livingEntity in collidersList
            .Select(collider2d => collider2d.GetComponent<LivingEntity>())
            .Where(livingEntity => livingEntity != null))
        {
            livingEntity.TakeDamage(SwordAttack.Damage);
        }
    }
    
    public void TryDashAttack()
    {
        if (!DashAttack.CanAttack)
            return;

        CurrentAttack = DashAttack;
        DashAttack.Attack();
    }
    
    public void OnDashStart()
    {
        DashDirection = LastAnimationDirection;
        StartCoroutine(nameof(DashCoroutine));
        StartCoroutine(nameof(DealDashDamage));
    }
    
    private IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(DashAttack.AttackDelay);
        IsDashing = true;
        yield return new WaitForSeconds(DashAttack.AttackTime);
        IsDashing = false;
    }
    
    private IEnumerator DealDashDamage()
    {
        yield return new WaitForSeconds(DashAttack.AttackDelay);
        var t = 0f;
        var nextDamageTickTime = 0f;
        for (; t < DashAttack.AttackTime; t += Time.deltaTime)
        {
            if (Time.time < nextDamageTickTime)
                continue;
            
            nextDamageTickTime = Time.time + DashAttack.DashTickTime;
            var collidersList = new List<Collider2D>();
            Physics2D.OverlapCircle((Vector2) transform.position + (Vector2) LastAnimationDirection,
                DashAttack.AttackRadius, ContactFilter2D, collidersList);

            foreach (var livingEntity in collidersList
                .Select(collider2d => collider2d.GetComponent<LivingEntity>())
                .Where(livingEntity => livingEntity != null))
            {
                livingEntity.TakeDamage(DashAttack.Damage);
            }

            yield return null;
        }
    }
    
    public void TryRangedAttack()
    {
        if (!RangedAttack.CanAttack)
            return;

        CurrentAttack = RangedAttack;
        RangedAttack.Attack();

        for (var i = 0; i < RangedAttack.PuddlesAmount; i++)
            SpawnRangedAttack();
    }

    private void SpawnRangedAttack()
    {
        var dz = ProjectilesManager.Instance.GetDamageZone();
        var randomSpawnPoint = (Vector2)transform.position + Random.insideUnitCircle * 3f;
        dz.Spawn(randomSpawnPoint);

        dz.OnAnimationEnd += OnAnimationEnd;
        void OnAnimationEnd()
        {
            dz.OnAnimationEnd -= OnAnimationEnd;
            dz.Reset();
            ProjectilesManager.Instance.Return(dz);
        }

        dz.OnDamageActivated += OnDamageActivated;
        void OnDamageActivated(DamageZone damageZone)
        {
            damageZone.OnDamageActivated -= OnDamageActivated;
            StartCoroutine(DealRangedDamage(damageZone));
        }
    }

    private IEnumerator DealRangedDamage(DamageZone damageZone)
    {
        var collidersList = new List<Collider2D>();
        Physics2D.OverlapCircle(damageZone.transform.position,
            RangedAttack.AttackRadius, ContactFilter2D, collidersList);

        foreach (var livingEntity in collidersList
            .Select(collider2d => collider2d.GetComponent<LivingEntity>())
            .Where(livingEntity => livingEntity != null))
        {
            livingEntity.TakeDamage(RangedAttack.Damage);
            yield return null;
        }
    }

    protected override void Update()
    {
        base.Update();
        var moveX = InputManager.Instance.HorizontalInput;
        var moveY = InputManager.Instance.VerticalInput;

        Direction = new Vector2(moveX, moveY).normalized;
        
        if (moveX != 0 || moveY != 0)
            LastAnimationDirection = Direction;

        if (IsAttacking())
            return;

        Animator.SetBool(Idle, Direction == Vector3.zero);
        Animator.SetFloat(Horizontal, LastAnimationDirection.x);
        Animator.SetFloat(Vertical, LastAnimationDirection.y);
    }

    private bool IsAttacking()
    {
        return SwordAttack.IsAttacking || DashAttack.IsAttacking || RangedAttack.IsAttacking;
    }

    protected override Vector2 GetDirection()
    {
        return IsDashing ? DashDirection : Direction;
    }

    protected override float GetMovementModifier()
    {
        if (IsDashing)
            return DashAttack.DashSpeed;

        return IsAttacking() ? 0.35f : base.GetMovementModifier();
    }
}