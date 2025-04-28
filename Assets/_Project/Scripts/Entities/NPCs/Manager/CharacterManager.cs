using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CharacterManager : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("Properties")] 
        [SerializeField] private float moveEnergy;
        
        private float _currentEnergy;
        private bool _canDecreaseEnergy;
        
        #endregion

        #region Methods

        // Unity Callbacks
        private void Start()
        {
            _canDecreaseEnergy = true;
            _currentEnergy = moveEnergy;
        }

        // Core
        public bool IsEnergyEmpty() => _currentEnergy is 0f;
        public void DecreaseEnergy()
        {
            if(!_canDecreaseEnergy)
                return;
            
            _currentEnergy -= Time.deltaTime;
            if (_currentEnergy <= 0.01f)
            {
                _currentEnergy = 0;
                _canDecreaseEnergy = false;
            }
        }
        
        #endregion
    }
}