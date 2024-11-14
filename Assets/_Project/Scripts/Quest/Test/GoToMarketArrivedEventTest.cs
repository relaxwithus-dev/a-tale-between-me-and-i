using ATBMI.Gameplay.Event;
using UnityEngine;

namespace ATBMI
{
    public class GoToMarketArrivedEventTest : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // arrived
                QuestEvents.ArrivedAtMarketEvent();
            }
        }
    }
}
