﻿using UnityEngine;

namespace ATBMI.Singleton
{
    public class MonoDDOL<T> : MonoBehaviour where T: MonoBehaviour
    {
        public static T Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}