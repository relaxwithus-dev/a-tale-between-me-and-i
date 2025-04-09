using UnityEngine;

namespace ATBMI.Singleton
{
    //Singleton Save Object Reference When Move Scene
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject obj = new()
                        {
                            name = typeof(T).Name
                        };
                        _instance = obj.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }
        
        protected virtual void Awake()
        {
            //Singleton method
            if (_instance == null) 
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            } 
            else if (_instance != this)
            {
                Destroy(_instance.gameObject);
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}