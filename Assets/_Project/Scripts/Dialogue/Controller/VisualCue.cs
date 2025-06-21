using UnityEngine;

namespace ATBMI.Dialogue
{
    public class VisualCue : MonoBehaviour
    {
        public void ActivateVisualCue()
        {
            gameObject.SetActive(true);
        }

        public void DeactivateVisualCue()
        {
            gameObject.SetActive(false);
        }
    }
}
