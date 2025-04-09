using System.Globalization;
using UnityEngine; 
using ATBMI.Enum;
using ATBMI.Utilities;

namespace ATBMI.Entities.NPCs
{
    public class TestingTraits : MonoBehaviour
    {
        // Fields
        [Header("Debug")]
        [SerializeField] private ReportManager reportManager;
        
        private CharacterTraits _traits;
        private readonly string[] _reportContents = new string[6];
        
        // Methods
        private void Awake()
        {
            _traits = GetComponent<CharacterTraits>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                var actionLenght = System.Enum.GetNames(typeof(InteractAction)).Length;
            
                reportManager.CreateReport(_traits.BaseEmotion.ToString());
            
                for (var i = 0; i < actionLenght; i++)
                {
                    var action = (InteractAction)i;
                    var (emotion, intensity) = _traits.GetDominantEmotion();
                    
                    _traits.InfluenceTraits(action);
                    _reportContents[0] = action.ToString();
                    _reportContents[1] = _traits.Emotions[0].ToString(CultureInfo.InvariantCulture);
                    _reportContents[2] = _traits.Emotions[1].ToString(CultureInfo.InvariantCulture);
                    _reportContents[3] = _traits.Emotions[2].ToString(CultureInfo.InvariantCulture);
                    _reportContents[4] = _traits.Emotions[3].ToString(CultureInfo.InvariantCulture);
                    _reportContents[5] = emotion.ToString();
                    
                    Debug.LogWarning($"Emotion: {_reportContents[0]} | Index: {_traits.Emotions[0]}," +
                                     $"{_traits.Emotions[1]},{_traits.Emotions[2]},{_traits.Emotions[3]}," +
                                     $" | New Emotion: {_reportContents[5]}");
                
                    reportManager.AppendReport(emotion.ToString(), _reportContents);
                    _traits.SetInitEmotions();
                }
            }
        }
    }
}