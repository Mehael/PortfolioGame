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
        public GameObject RunePrefab;
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

        public void Interact()
        {
            if (RunePrefab != null)
                StartCoroutine(ShowRune());
            OnStep();
        }
        
        protected virtual void OnStep()
        {
            AudioSystem.Play("FloorStep");
        }

        protected virtual void OnTurnMade() { }

        private IEnumerator ShowRune()
        {
            var rune = Instantiate(RunePrefab, transform.position, RunePrefab.transform.rotation, transform);
            yield return rune.transform.ScaleFromTo(Vector3.one * .7f,Vector3.one * 1.5f, 1.5f);
            Destroy(rune.gameObject);
        }
    }
}