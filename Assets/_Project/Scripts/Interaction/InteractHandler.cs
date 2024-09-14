using UnityEngine;
using TMPro;
using DanielLochner.Assets.SimpleScrollSnap;
using ATBMI.Inventory;
using ATBMI.Player;

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

        // Neccesary
        public IInteractable CurrentTarget { get; private set; }

        [Header("UI")]
        [SerializeField] private GameObject contentPrefabs;
        [SerializeField] private GameObject interactOptionsUI;
        [SerializeField] private TextMeshProUGUI descriptionTextUI;

        [Header("Reference")]
        [SerializeField] private SimpleScrollSnap scrollSnap;
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private PlayerController playerController;

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
        
        // !- Initialize
        private void InitContent()
        {
            // Init Content
            var optionsCount = interactOptionsUI.transform.childCount - 3;
            var itemCount = inventoryManager.CollectibleItem.Count;

            if (itemCount > optionsCount)
            {
                var countGap = Mathf.Abs(itemCount - optionsCount);
                for (var i = 0; i < countGap; i++)
                {
                    Instantiate(contentPrefabs, interactOptionsUI.transform, worldPositionStays: false);
                }
            }
        }

        private void InitScrollSnap()
        {
            // Scroll setup
            scrollSnap.Setup();
            scrollSnap.HandleInfiniteScrolling(true);
        }

        // !- Core
        public void OpenInteractOption(IInteractable interactable)
        {
            // Setup
            InitContent();
            InitScrollSnap();

            // Neccesary
            CurrentTarget = interactable;
            playerController.StopMovement();
            gameObject.SetActive(true);
        }

        public void CloseInteractOption()
        {
            CurrentTarget = null;
            gameObject.SetActive(false);
            playerController.StartMovement();
        }

        #endregion
    }
}
