using UnityEngine;

public class MonoBehaviourFactory<T> : IFactory<T> where T : MonoBehaviour
{
    private readonly string Name;
    private int Index;

    public MonoBehaviourFactory() : this("MonoBehaviour")
    {
    }

    public MonoBehaviourFactory(string name)
    {
        Name = name;
    }

    public T Create()
    {
        var tempGameObject = Object.Instantiate(new GameObject());

        tempGameObject.name = Name + Index;
        tempGameObject.AddComponent<T>();
        var objectOfType = tempGameObject.GetComponent<T>();
        Index++;
        return objectOfType;
    }
}