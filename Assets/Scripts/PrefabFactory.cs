using UnityEngine;

public class PrefabFactory<T> : IFactory<T> where T : MonoBehaviour
{
    private readonly T Prefab;
    private readonly string Name;
    private int Index;

    public PrefabFactory(T prefab) : this(prefab, prefab.name)
    {
    }

    private PrefabFactory(T prefab, string name)
    {
        Prefab = prefab;
        Name = name;
    }

    public T Create()
    {
        var tempGameObject = Object.Instantiate(Prefab);
        tempGameObject.name = Name + Index;
        var objectOfType = tempGameObject.GetComponent<T>();
        Index++;
        return objectOfType;
    }
}