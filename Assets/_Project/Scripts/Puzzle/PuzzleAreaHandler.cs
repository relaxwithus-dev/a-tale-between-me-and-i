using System;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Stress;

namespace ATBMI.Puzzle
{
    public class PuzzleAreaHandler : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                StressEventHandler.StressOvertimeEvent(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                StressEventHandler.StressOvertimeEvent(false);
            }
        }
    }
}