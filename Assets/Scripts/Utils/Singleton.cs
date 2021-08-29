using UnityEngine;

namespace Utils
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = (T) this;
            }
            else
            {
                Destroy(this);
            }

            DontDestroyOnLoad(this);
        }
    }
}