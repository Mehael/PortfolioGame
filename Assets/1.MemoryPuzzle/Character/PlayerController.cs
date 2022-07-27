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
        public Vector2Int Coords { get; private set; }
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
            Coords = Board.WorldToLogicCoords(transform.position);
            if (board.Tiles.ContainsKey(Coords))
                board.Tiles[Coords].Interact();
            
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

        private void MoveTo(Vector2Int newCoords)
        {
            if (isActive == false) return;

            OnMovementStart?.Invoke();
            if (IsInMovement) {
                StopCoroutine(currentMovementCoroutine);
                MoveHeroSprite(Coords);
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
                Coords = newCoords;
                
                currentMovementCoroutine = StartCoroutine(
                    transform.MoveTo(
                        newWorldPosition,
                        MovementSpeed,
                        movementCallback)); 
            }

            IEnumerator WallStuck()
            {
                var positionStarting = Board.LogicToWorldCoords(Coords);
                var positionBetweenTiles = (newWorldPosition + positionStarting) / 2;
                
                yield return StartCoroutine(transform.MoveTo(positionBetweenTiles, MovementSpeed * 2));
                Board.current.Tiles[newCoords].Interact();
                IsInMovement = false;
                yield return StartCoroutine(transform.MoveTo(positionStarting, MovementSpeed * 2));
                Board.current.Tiles[Coords].Interact();
            }
        }

        private void MoveHeroSprite(Vector2Int enterPoint)
        {
            transform.position = Board.LogicToWorldCoords(enterPoint);
            Coords = enterPoint;
        }

        private bool isActive = true;
        public void MoveDown() => MoveTo(Coords + Vector2Int.down);
        public void MoveUp() => MoveTo(Coords + Vector2Int.up);
        public void MoveRight() => MoveTo(Coords + Vector2Int.left);
        public void MoveLeft() => MoveTo(Coords + Vector2Int.right);

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