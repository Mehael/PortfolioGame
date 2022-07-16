using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace MemoryPuzzle
{
    public class WeakTile : Tile
    {
        public override void OnStep()
        {
            AudioSystem.Play("Weak");
            Die();   
        }
    }
}