using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

namespace MemoryPuzzle
{
    public class Board : MonoBehaviour
    {
        public Dictionary<Vector2Int, Tile> Tiles = new Dictionary<Vector2Int, Tile>();
        private List<TileFinish> finishTiles = new List<TileFinish>();
        
        public static Board current;
        public Action OnTurnDone;

        void Awake()
        {
            current = this;
        }

        public Action Interact(Vector2Int newCoords)
        {
            return () =>
            {
                if (!Tiles.ContainsKey(newCoords))
                    PlayerController.Fall();
                else
                    Tiles[newCoords].Interact();
            };
        }

        public void TurnDone()
        {
            OnTurnDone?.Invoke();
        }
        
        public static Vector3 LogicToWorldCoords(Vector2Int boardCoords)
        {
            return new Vector3(
                boardCoords.x + 0.5f,
                boardCoords.y + 0.5f,
                0
            );
        }

        public static Vector2Int WorldToLogicCoords(Vector2 worldCoords)
        {
            return new Vector2Int(
                Mathf.RoundToInt(worldCoords.x - 0.5f),
                Mathf.RoundToInt(worldCoords.y - 0.5f));
        }

        public void Register(Tile tile)
        {
            if (Tiles.ContainsKey(tile.Coords))
            {
                Debug.LogError("Two nodes in one Coords: " + tile.name + ", " + Tiles[tile.Coords].name);
                return;
            }
            Tiles.Add(tile.Coords, tile);
        }
        
        public void Unregister(Tile tile)
        {
            if (Tiles[tile.Coords] == tile)
                Tiles.Remove(tile.Coords);
        }

        public void RegisterFinish(TileFinish tile)
        {
            finishTiles.Add(tile);
        }

        public void UnregisterFinish(TileFinish tile)
        {
            finishTiles.Remove(tile);
            if (finishTiles.Count == 0)
                PlayerController.Victory();
        }
    }
}