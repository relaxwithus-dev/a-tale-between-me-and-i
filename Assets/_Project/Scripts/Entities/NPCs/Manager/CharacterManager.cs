using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Properties")] 
        [SerializeField] private float characterEnergy;
        private float _currentEnergy;
        private bool _canDecreaseEnergy;
        
        private void Start()
        {
            _canDecreaseEnergy = true;
            _currentEnergy = characterEnergy;
        }

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
        
    }
}