using System.Linq;
using UnityEngine;
using UnityEditor;
using ATBMI.Scene.Chapter;

namespace ATBMI.Cutscene
{
    [CreateAssetMenu(fileName = "CutsceneKeys", menuName = "Data/Cutscene Keys", order = 2)]
    public class CutsceneKeys : ScriptableObject
    {
        [Header("Attribute")] 
        public Chapters chapter;
        public CutsceneLabel id;
        
        public string GetChapterAcronym(Chapters targetChapter)
        {
            return targetChapter switch
            {
                Chapters.Prologue => "C_PRLG",
                Chapters.Chapter_1 => "C_CHP1",
                Chapters.Chapter_2 => "C_CHP2",
                _ => "UNK"
            };
        }
    }
    
    [CustomEditor(typeof(CutsceneKeys))]
    public class CutsceneKeysEditor : Editor 
    {
        public override void OnInspectorGUI() 
        {
            var selector = (CutsceneKeys)target;
            var chapterName = selector.chapter.ToString();
            selector.chapter = (Chapters)EditorGUILayout.EnumPopup("Chapter", selector.chapter);

            if (!string.IsNullOrEmpty(chapterName))
            {
                var allLocations = System.Enum.GetValues(typeof(CutsceneLabel)).Cast<CutsceneLabel>();
                var filtered = allLocations
                    .Where(loc => loc.ToString().StartsWith(selector.GetChapterAcronym(selector.chapter) + "_"))
                    .ToArray();
                
                if (filtered.Length > 0) 
                {
                    var currentIndex = System.Array.IndexOf(filtered, selector.id);
                    if (currentIndex == -1) currentIndex = 0;

                    selector.id = filtered[
                        EditorGUILayout.Popup("Id", currentIndex,
                            filtered.Select(l => l.ToString()).ToArray())
                    ];
                } else 
                {
                    EditorGUILayout.HelpBox("Tidak ada id untuk chapter ini.", MessageType.Info);
                }
            } else 
            {
                EditorGUILayout.HelpBox("Masukkan chapter terlebih dahulu.", MessageType.Warning);
            }
            
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
}