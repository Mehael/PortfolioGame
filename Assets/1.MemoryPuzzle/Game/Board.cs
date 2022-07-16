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
        public Dictionary<Vector2, Tile> Tiles = new Dictionary<Vector2, Tile>();
        private List<TileFinish> finishTiles = new List<TileFinish>();
        
        public static Board current;
        public Action OnTurnDone;

        void Awake()
        {
            current = this;
        }

        public Action OnStep(Vector2 newCoords)
        {
            return () =>
            {
                if (!Tiles.ContainsKey(newCoords))
                    PlayerController.Fall();
                else
                    Tiles[newCoords].OnStep();
            };
        }

        public void TurnDone()
        {
            OnTurnDone?.Invoke();
        }
        
        public static Vector3 LogicToWorldCoords(Vector2 boardCoords)
        {
            return new Vector3(
                boardCoords.x + 0.5f,
                boardCoords.y + 0.5f,
                (boardCoords.y / 10) - 0.08f
            );
        }

        public static Vector2 WorldToLogicCoords(Vector2 worldCoords)
        {
            return new Vector2(
                Mathf.Round(worldCoords.x - 0.5f),
                Mathf.Round(worldCoords.y - 0.5f));
        }

        public void Register(Tile tile)
        {
            if (Tiles.ContainsKey(tile.Coords))
            {
                Debug.LogError("Two nodes in one coords: " + tile.name + ", " + Tiles[tile.Coords].name);
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
                LevelLoader.LoadNextLevel();
        }
    }
}