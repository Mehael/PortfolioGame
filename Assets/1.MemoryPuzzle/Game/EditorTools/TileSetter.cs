using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPuzzle
{
    [ExecuteInEditMode]
    public class TileSetter : MonoBehaviour
    {
        public static Action OnLevelEdited;
        
#if (UNITY_EDITOR)
        private Vector3 lastPosition;
        
        
        void Update()
        {
            if (lastPosition.Equals(transform.localPosition)) return;
            lastPosition = transform.localPosition;

            var coords = Board.WorldToLogicCoords(transform.position);
            var newWorldCoords = Board.LogicToWorldCoords(coords);
            newWorldCoords.z += 0.08f;

            transform.localPosition = newWorldCoords;
            OnLevelEdited?.Invoke();
        }
#endif
    }
}
