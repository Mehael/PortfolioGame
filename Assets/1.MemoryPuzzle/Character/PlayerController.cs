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
        public Action OnDeath;
        public Action OnVictory;
        private void Awake()
        {
            instance = this;
        }
        
        private void Start()
        {
            board = Board.current;
            coords = Board.WorldToLogicCoords(transform.position);
            if (board.Tiles.ContainsKey(coords))
                board.Tiles[coords].Interact();
            
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
            if (IsInMovement) {
                StopCoroutine(currentMovementCoroutine);
                MoveHeroSprite(coords);
                movementCallback.Invoke();
            }

            Board.current.TurnDone();

            var isWallStuck = board.Tiles.ContainsKey(newCoords) && !board.Tiles[newCoords].IsPassable;
            var newWorldPosition = Board.LogicToWorldCoords(newCoords);
            
            IsInMovement = true;
            if (isWallStuck) {
                currentMovementCoroutine = StartCoroutine(WallStuck());
            } else {
                movementCallback = () => IsInMovement = false;
                movementCallback += Board.current.Interact(newCoords);
                coords = newCoords;
                
                currentMovementCoroutine = StartCoroutine(
                    transform.MoveTo(
                        newWorldPosition,
                        MovementSpeed,
                        movementCallback)); 
            }

            IEnumerator WallStuck()
            {
                var positionStarting = Board.LogicToWorldCoords(coords);
                var positionBetweenTiles = (newWorldPosition + positionStarting) / 2;
                
                yield return StartCoroutine(transform.MoveTo(positionBetweenTiles, MovementSpeed * 2));
                Board.current.Tiles[newCoords].Interact();
                IsInMovement = false;
                yield return StartCoroutine(transform.MoveTo(positionStarting, MovementSpeed * 2));
                Board.current.Tiles[coords].Interact();
            }
        }

        private void MoveHeroSprite(Vector2 enterPoint)
        {
            transform.position = Board.LogicToWorldCoords(enterPoint);
            coords = enterPoint;
        }

        private bool isActive = true;
        public void MoveDown() => MoveTo(coords + Vector2.down);
        public void MoveUp() => MoveTo(coords + Vector2.up);
        public void MoveRight() => MoveTo(coords + Vector2.left);
        public void MoveLeft() => MoveTo(coords + Vector2.right);

        public static void Fall()
        {
            instance.isActive = false;
            instance.OnDeath?.Invoke();
        }
        
        public static void Victory()
        {
            instance.isActive = false;
            instance.OnVictory?.Invoke();
        }
    }
}