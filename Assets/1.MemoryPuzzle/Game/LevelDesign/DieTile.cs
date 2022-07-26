using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPuzzle
{
    public class DieTile : Tile
    {
        protected override void OnStep()
        {
            PlayerController.Fall();
        }
    }
}
