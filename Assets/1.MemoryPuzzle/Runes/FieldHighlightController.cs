using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPuzzle
{
    public class FieldHighlightController : MonoBehaviour
    {
        public float EffectDelay = 1f;
        public float ShiningWavesDelay = .1f;
        public float ShiningWaveTime = .5f;
        public int ShiningRadius = 2;

        public IEnumerator PlayShine(Vector2Int centerCoords)
        {
            yield return new WaitForSeconds(EffectDelay);
            var tiles = Board.current.Tiles;
            for (var radius = 0; radius < ShiningRadius; radius++) {
                foreach (var tile in tiles) {
                    var coords = tile.Key - centerCoords;
                    if (Math.Abs(coords.x) + Math.Abs(coords.y) == radius) {
                        tile.Value.Highlight(radius, ShiningWaveTime);
                    }
                }
                yield return new WaitForSeconds(ShiningWavesDelay);
            }

        }
    }
}

