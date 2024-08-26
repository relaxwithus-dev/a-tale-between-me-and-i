using UnityEngine;
using TMPro;
using DanielLochner.Assets.SimpleScrollSnap;
using ATBMI.Inventory;

namespace ATBMI.Interaction
{
    /// <summary>
    /// InteractHandler buat handle interact option
    /// karakter player, tergantung dengan tipe
    /// object yang di-interact.
    /// </summary>
    public class InteractHandler : MonoBehaviour
    {
        #region Fields & Properties

        [Header("UI")]
        [SerializeField] private GameObject optionsPanelUI;
        [SerializeField] private TextMeshProUGUI optionInfoTextUI;

        [Header("Reference")]
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private SimpleScrollSnap scrollSnap;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
        
        }

        private void Update()
        {
        
        }

        #endregion

        #region Methods
        

        #endregion
    }
}
