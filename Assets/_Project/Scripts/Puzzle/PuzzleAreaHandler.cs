using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Puzzle
{
    public class PuzzleAreaHandler : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                
            }
        }
    }
}