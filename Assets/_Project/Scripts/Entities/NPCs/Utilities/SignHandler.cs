using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class SignHandler : MonoBehaviour
    {
        // Fields
        [SerializeField] private Transform rootSign;
        private Transform _sign;
        
        // Methods
        private void Update()
        {
            if (transform.childCount < 1)
            {
                if (_sign)
                    _sign = null;
                return;
            }
            
            _sign ??= transform.GetChild(0);
            
            if (!_sign.gameObject.activeSelf) return;
            if (_sign.localEulerAngles != rootSign.localEulerAngles)
            {
                _sign.localEulerAngles = rootSign.localEulerAngles;
            }
        }
    }
}