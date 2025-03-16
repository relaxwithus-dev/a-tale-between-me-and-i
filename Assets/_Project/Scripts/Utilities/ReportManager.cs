using System.IO;
using UnityEngine;
using Directory = System.IO.Directory;

namespace ATBMI.Utilities
{
    public static class ReportManager
    {
        // Fields
        private static string reportDirectory = "Testing";
        private static string reportSeparator = ";";
        private static string[] repotHeader = 
        {
            "Node",
            "Wrisk",
            "Wplan",
            "Wtime",
            "Wtotal"
        };
        
        // Core
        public static void CreateReport(string fileName)
        {
            VerifyDirectoryPath();
            using (StreamWriter sw = File.CreateText(GetFilePath(fileName)))
            {
                var finalString = "";
                for (var i = 0; i < repotHeader.Length; i++)
                {
                    if (finalString != "")
                        finalString += reportSeparator;
                    
                    finalString += repotHeader[i];
                }
                sw.WriteLine(finalString);
                sw.Close();
            }
        }
        
        public static void AppendReport(string fileName, string[] content)
        {
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
        
        static void VerifyDirectoryPath()
        {
            var directory = GetDirectoryPath();
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }
        
        private static string GetDirectoryPath()
        {
            return Application.dataPath + "/_Project" + "/" + reportDirectory;
        }
        
        private static string GetFilePath(string fileName)
        {
            return GetDirectoryPath() + "/" + fileName + ".csv";
        }
        
    }
}
