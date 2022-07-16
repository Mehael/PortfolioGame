using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPuzzle
{
    public class Wall : Tile
    {
        public override bool IsPassable => false;
    }
}