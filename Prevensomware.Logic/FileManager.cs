using System.Collections.Generic;
using System.IO;

namespace Prevensomware.Logic
{
    public static class FileManager
    {
        public static void RenameAllFilesWithNewExtension(IEnumerable<ExtensionReplacement> extensionReplacementList, string directoryPath)
        {
            foreach (var extensionReplacement in extensionReplacementList)
            {
                var allFilesArray = Directory.GetFiles(directoryPath, "*" + extensionReplacement.Name, SearchOption.AllDirectories);
                RenameFileList(allFilesArray, extensionReplacement.Replacement);
            }
           
        }

        private static void RenameFileList(IEnumerable<string> filePathList, string newExtension)
        {
            foreach (var filePath in filePathList)
            {
                var newPath = Path.ChangeExtension(filePath, newExtension);
                File.Move(filePath, newPath);
            }
        }
    }
}
