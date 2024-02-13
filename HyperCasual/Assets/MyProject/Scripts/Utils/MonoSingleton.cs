namespace Project.ui
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                    Debug.Log("Instance not initialized");

                return instance;
            }
        }

        private void Awake()
        {
            if (instance != null)
                Destroy(gameObject);
            else
            {
                instance = this as T;
                ExecuteOnAwake();
            }
        }

        private void OnDestroy()
        {
            if (instance == this as T)
            {
                instance = null;
            }

            ExecuteOnDestroy();
        }

        protected virtual void ExecuteOnAwake() { }
        protected virtual void ExecuteOnDestroy() { }
    }
}