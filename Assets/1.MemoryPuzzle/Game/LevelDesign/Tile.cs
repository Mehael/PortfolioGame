using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace MemoryPuzzle
{
    public class Tile : MonoBehaviour
    {
        public virtual bool IsPassable => true;
        public RuneAnimator RunePrefab;
        private RuneAnimator runeInstance;
        public Vector2Int Coords { get; private set; }

        protected virtual void Start()
        {
            Coords = Board.WorldToLogicCoords(transform.position);
            Board.current.Register(this);
            Board.current.OnTurnDone += OnTurnMade;
            runeInstance = Instantiate(RunePrefab, transform.position, RunePrefab.transform.rotation, transform);
        }

        protected void Die()
        {
            Board.current.Unregister(this);
            Destroy(gameObject);
        }

        public void Interact()
        {
            runeInstance.Interact();
            OnStep();
        }

        public void Highlight(int radius, float time)
        {
            runeInstance.Highlight(radius, time);
        }
        
        protected virtual void OnStep()
        {
            AudioSystem.Play("FloorStep");
        }

        protected virtual void OnTurnMade() { }
    }
}