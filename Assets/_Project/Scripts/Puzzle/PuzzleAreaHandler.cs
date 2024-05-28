using System;
using System.Collections.Generic;
using ATBMI.Stress;
using UnityEngine;

namespace ATBMI.Puzzle
{
    public class PuzzleAreaHandler : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("increased stress");
                StressEventHandler.StressOvertimeEvent();
            }
        }
    }
}