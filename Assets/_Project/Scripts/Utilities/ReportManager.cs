using System.IO;
using UnityEngine;
using Directory = System.IO.Directory;

namespace ATBMI.Utilities
{
    public class ReportManager : MonoBehaviour
    {
        // Fields
        [Header("Property")]
        [SerializeField] private string reportDirectory;
        [SerializeField] private string[] reportHeader;
        
        private readonly string defaultDirectory = "/_Project/Testing/";
        private readonly string reportSeparator = ";";
        
        // Core
        public void CreateReport(string fileName)
        {
            VerifyDirectoryPath();
            using (StreamWriter sw = File.CreateText(GetFilePath(fileName)))
            {
                var finalString = "";
                for (var i = 0; i < reportHeader.Length; i++)
                {
                    if (finalString != "")
                        finalString += reportSeparator;
                    
                    finalString += reportHeader[i];
                }
                sw.WriteLine(finalString);
                sw.Close();
            }
        }
        
        public void AppendReport(string fileName, string[] content)
        {
            if (content.Length != reportHeader.Length)
            {
                Debug.LogError("content isn't same with header!");
                return;
            }
            
            using (StreamWriter sw = File.AppendText(GetFilePath(fileName)))
            {
                var finalString = "";
                for (var i = 0; i < content.Length; i++)
                {
                    if (finalString != "")
                        finalString += reportSeparator;
                    
                    finalString += content[i];
                }
                sw.WriteLine(finalString);
                sw.Close();
            }
        }
        
        private void VerifyDirectoryPath()
        {
            var directory = GetDirectoryPath();
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }
        
        private string GetDirectoryPath()
        {
            return Application.dataPath + defaultDirectory + reportDirectory;
        }
        
        private string GetFilePath(string fileName)
        {
            return GetDirectoryPath() + "/" + fileName + ".csv";
        }
    }
}
