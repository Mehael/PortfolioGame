using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPuzzle
{
    [ExecuteInEditMode]
    public class HeroSetter : MonoBehaviour
    {
#if (UNITY_EDITOR)
        private void Start()
        {
            TileSetter.OnLevelEdited += UpdateHeroPosition;
            UpdateHeroPosition();
        }

        private void OnDestroy()
        {
            TileSetter.OnLevelEdited -= UpdateHeroPosition;
        }

        void UpdateHeroPosition()
        {
            if (this == null)
                return;
            
            var start = FindObjectsOfType<TileStart>();
            if (start.Length != 1)
            {
                Debug.LogError("Put 1 TileStart on level!");
                return;
            }

            transform.position = start[0].transform.position;
        }
#endif
    }
}