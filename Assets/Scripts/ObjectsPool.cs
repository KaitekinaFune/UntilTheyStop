using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectsPool<T> : Singleton<ObjectsPool<T>> where T: Poolable
{
    [SerializeField] private int ObjectsToInstantiate;
    [SerializeField] private T ObjectPrefab;
    
    private readonly Stack<T> Objects = new Stack<T>();

    protected override void Awake()
    {
        base.Awake();
        for (var i = 0; i < ObjectsToInstantiate; i++)
            AddObject();
    }

    public IEnumerable<T> Get(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            if (Objects.Count == 0)
                AddObject();

            yield return Objects.Pop();
        }
    }

    private void AddObject()
    {
        var obstacleObject = Instantiate(ObjectPrefab, transform);
        ReturnObjectToPool(obstacleObject);
    }
        
    public void ReturnObjectToPool(T obj)
    {
        obj.Disable();
        Objects.Push(obj);
    }
}