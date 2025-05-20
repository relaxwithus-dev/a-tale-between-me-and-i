using UnityEngine;

namespace ATBMI.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Fields
        
        [Header("Components")] 
        [SerializeField] private bool isGamePlaying;
        [SerializeField] private GameObject[] gameplayObjects;
        
        public bool IsGamePlaying => isGamePlaying;

        #endregion

        #region Methods
        
        public void StartGame()
        {
            isGamePlaying = true;
            foreach (var gameplay in gameplayObjects)
            {
                gameplay.SetActive(true);
            }
        }
        
        public void EndGame()
        {
            isGamePlaying = false;
            foreach (var gameplay in gameplayObjects)
            {
                gameplay.SetActive(false);
            }
        }
        
        #endregion
    }
}
