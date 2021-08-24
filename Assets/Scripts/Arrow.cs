
using UnityEngine;

public class Arrow : Poolable
{
    [SerializeField] private float Speed;
    [SerializeField] private float LifeTime;
    [SerializeField] private float Damage;
    [SerializeField] private LayerMask LayerMask;

    private float TimeAlive;
    
    public void Launch(Vector3 start, Vector3 towards)
    {
        TimeAlive = 0f;
        Enable();
        var t = transform;
        t.position = start;
        var vectorToTarget = t.position - towards;
        var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90f;
        var q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = q;
    }
        
    private void Update()
    {
        if (!gameObject.activeSelf)
            return;
        
        if (TimeAlive >= LifeTime)
            ReturnToPool();

        TimeAlive += Time.deltaTime;
        var moveDistance = Speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        gameObject.transform.Translate(Vector3.up * moveDistance);
    }

    private void CheckCollisions(float moveDistance)
    {
        var t = gameObject.transform;
        var position = t.position;
        var hit = Physics2D.Raycast(position, t.up, moveDistance, LayerMask);
        
        if (hit.collider != null)
            OnHitObject(hit.collider.gameObject);
    }

    private void OnHitObject(GameObject hit)
    {
        var player = hit.GetComponent<Player>();

        if (player != null)
            player.TakeDamage(Damage);

        ReturnToPool();
    }

    protected override void ReturnToPool()
    {
        ArrowsPool.Instance.ReturnObjectToPool(this);
    }
}
