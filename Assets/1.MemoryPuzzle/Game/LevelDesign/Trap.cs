using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPuzzle
{
    public class Trap : Tile
    {
        public int TurnsToBecomeSafe = 0;
        private bool wasPlayerOnPrevTurn;
        private bool isDeadly;
        protected override void OnTurnMade()
        {
            if (wasPlayerOnPrevTurn)
            {
                AudioSystem.Play("TrapUp");
                isDeadly = true;
                TurnsToBecomeSafe = 2;
            }
            else if (isDeadly)
            {
                TurnsToBecomeSafe--;
                if (TurnsToBecomeSafe != 0)
                    return;

                AudioSystem.Play("TrapDown");
                isDeadly = false;
            }

            wasPlayerOnPrevTurn = false;
        }

        protected override void OnStep()
        {
            if (isDeadly)
                PlayerController.Fall();
            else 
                AudioSystem.Play("TrapStep");

            wasPlayerOnPrevTurn = true;
        }
    }
}
