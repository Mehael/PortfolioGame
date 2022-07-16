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

        public Vector2 Coords { get; private set; }

        protected virtual void Start()
        {
            Coords = Board.WorldToLogicCoords(transform.position);
            Board.current.Register(this);
            Board.current.OnTurnDone += OnTurnMade;
        }

        protected void Die()
        {
            Board.current.Unregister(this);
            Destroy(gameObject);
        }

        public virtual void OnStep()
        {
            AudioSystem.Play("FloorStep");
        }

        public virtual void OnTurnMade() { }
    }
}