using ATBMI.Enum;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class SceneryInteraction : Interaction
    {
        #region Fields & Properties

        [Header("Scenery")]
        [SerializeField] private TextAsset[] dialogueAssets;

        #endregion

        #region Methods

        // TODO: Drop method call dialogue
        public override void Interact(InteractManager manager, int itemId = 0)
        {
            base.Interact(manager, itemId);
            var isTalks = interactStatus == InteractStatus.Talks;
            
            if (isTalks)
                Debug.Log("hi bro, jangan lupa #timnasday");
            else
                Debug.Log("tidak terjadi apa-apa");
        }

        #endregion
    }
}