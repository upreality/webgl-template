using UnityEditor;
using UnityEditor.Callbacks;

namespace Core.SDK.Editor
{
    public static class FillWgl
    {
        [PostProcessBuild(10)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            ProjectFileUtils.ApplyToIndexHtml(target, pathToBuiltProject, EditJsCode);
        }

        private static void EditJsCode(string filePath)
        {
            ProjectFileUtils.InsertContent(
                filePath,
                HtmlInsertionsRepository.HeadLines,
                HtmlInsertionsRepository.BodyLines
            );
            
            if (!HtmlSdkConfig.FindConfig(out var config))
                return;

            ProjectFileUtils.ReplaceEntries(filePath, config.Definitions);
        }
    }
}