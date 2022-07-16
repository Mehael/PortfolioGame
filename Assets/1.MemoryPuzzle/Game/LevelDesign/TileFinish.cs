using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryPuzzle
{
    public class TileFinish : Tile
    {
        public GameObject visual;
        
        protected override void Start()
        {
            base.Start();
            Board.current.RegisterFinish(this);
        }

        public override void OnStep()
        {
            visual.SetActive(false);
            AudioSystem.Play("DoorOpen");
            Board.current.UnregisterFinish(this);
        }
    }
}