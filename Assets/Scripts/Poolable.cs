using UnityEngine;

public abstract class Poolable : MonoBehaviour
{
    public virtual void Respawn(Vector3 spawnPosition)
    {
        Enable();
        SetPosition(spawnPosition);
    }

    private void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    internal void Enable() => gameObject.SetActive(true);
    public void Disable() => gameObject.SetActive(false);

    protected abstract void ReturnToPool();
}