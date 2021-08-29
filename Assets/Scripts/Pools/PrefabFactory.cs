using UnityEngine;

namespace Pools
{
    public class PrefabFactory<T> : IFactory<T> where T : MonoBehaviour
    {
        private readonly Transform Parent;
        private readonly T Prefab;
        private readonly string Name;
        private int Index;

        public PrefabFactory(T prefab, Transform parent) : this(prefab, prefab.name)
        {
            Parent = parent;
        }

        private PrefabFactory(T prefab, string name)
        {
            Prefab = prefab;
            Name = name;
        }

        public T Create()
        {
            var tempGameObject = Object.Instantiate(Prefab, Parent);
            tempGameObject.name = Name + Index;
            var objectOfType = tempGameObject.GetComponent<T>();
            Index++;
            return objectOfType;
        }
    }
}