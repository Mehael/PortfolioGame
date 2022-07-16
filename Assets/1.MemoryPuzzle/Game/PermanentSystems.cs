using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPuzzle
{
    public class PermanentSystems : MonoBehaviour
    {
        public GameObject SystemsPrefab;
        public static PermanentSystems instance;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            Instantiate(SystemsPrefab, transform);

            if (transform.parent != null)
                transform.SetParent(null);

            DontDestroyOnLoad(this);
        }
    }
}