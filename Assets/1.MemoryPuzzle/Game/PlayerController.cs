using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MemoryPuzzle
{
    public class PlayerController : MonoBehaviour
    {
        private Board board;
        private Coroutine currentMovementCoroutine;
        private Action movementCallback;

        public float MovementSpeed = 5f;
        public Vector2 coords { get; private set; }
        public static PlayerController instance;
        public Action OnMovementStart;
        public bool IsInMovement { get; private set; }      
        
        private void Awake()
        {
            instance = this;
        }
        
        private void Start()
        {
            board = Board.current;
            coords = Board.WorldToLogicCoords(transform.position);
                
            InputController.OnAction += Fall;
            InputController.OnLeft += MoveLeft;
            InputController.OnRight += MoveRight;
            InputController.OnUp += MoveUp;
            InputController.OnDown += MoveDown;
        }

        private void OnDestroy()
        {
            InputController.OnAction -= Fall;
            InputController.OnLeft -= MoveLeft;
            InputController.OnRight -= MoveRight;
            InputController.OnUp -= MoveUp;
            InputController.OnDown -= MoveDown;
        }

        private void MoveTo(Vector2 newCoords)
        {
            if (isActive == false) return;

            OnMovementStart?.Invoke();
            if (IsInMovement)
            {
                StopCoroutine(currentMovementCoroutine);
                MoveHeroSprite(coords);
                movementCallback.Invoke();
            }

            Board.current.TurnDone();
            if (board.Tiles.ContainsKey(newCoords))
            {
                var nextTile = board.Tiles[newCoords];
                if (!nextTile.IsPassable)
                {
                    newCoords = coords;
                    AudioSystem.Play("WallStuck");
                }
            }

            movementCallback = Board.current.OnStep(newCoords);
            movementCallback += () => IsInMovement = false;
            
            coords = newCoords;

            IsInMovement = true;
            currentMovementCoroutine = StartCoroutine(
                transform.MoveTo(
                    Board.LogicToWorldCoords(newCoords),
                    MovementSpeed,
                    movementCallback));
        }

        private void MoveHeroSprite(Vector2 enterPoint)
        {
            transform.position = Board.LogicToWorldCoords(enterPoint);
            coords = enterPoint;
        }

        private bool isActive = true;

        private IEnumerator Die()
        {
            isActive = false;
            
            AudioSystem.Play("FallToHole");

            StartCoroutine(transform.ScaleHide(1f));
            yield return new WaitForSeconds(1.5f);
            LevelLoader.RestartLevel();
        }

        public void MoveDown() => MoveTo(coords + Vector2.down);
        public void MoveUp() => MoveTo(coords + Vector2.up);
        public void MoveRight() => MoveTo(coords + Vector2.right);
        public void MoveLeft() => MoveTo(coords + Vector2.left);

        public static void Fall()
        {
            instance.StartCoroutine(instance.Die());
        }
    }
}