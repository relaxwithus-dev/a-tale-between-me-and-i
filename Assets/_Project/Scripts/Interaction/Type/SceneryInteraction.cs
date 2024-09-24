using ATBMI.Enum;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class SceneryInteraction : Interaction
    {
        #region Fields & Properties

        [Header("Scenery")]
        [SerializeField] private TextAsset dialogueAsset;

        #endregion

        #region Methods

        public override void Interact(InteractManager manager, int itemId)
        {
            base.Interact(manager, itemId);
            statusSucces = interactStatus == InteractStatus.Talks;
            if (statusSucces)
            {
                statusSucces = true;
                Debug.Log("hi bro, jangan lupa #timnasday");
            }
            else
            {
                Debug.Log("tidak terjadi apa-apa");
            }
        }

        #endregion
    }
}