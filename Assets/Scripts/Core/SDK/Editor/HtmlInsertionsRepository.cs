using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.SDK.Editor
{
    public static class HtmlInsertionsRepository
    {
        public static List<string> HeadLines
        {
            get
            {
                var headLines = new List<string>();
#if GOOGLE_ANALYTICS
                headLines = ProjectFileUtils.ReadLinesFromFile("insertHeadJsG").ToList();
#endif
                var platformHeadFilePath = "insertHeadJs";
#if YANDEX_SDK
                platformHeadFilePath = "insertHeadJsY";
#elif VK_SDK
                platformHeadFilePath = "insertHeadJsV";
#else
                return headLines;
#endif
                var platformLines = ProjectFileUtils.ReadLinesFromFile(platformHeadFilePath).ToList();
                return headLines
                    .Concat(platformLines)
                    .ToList();
            }
        }

        public static List<string> BodyLines
        {
            get
            {
                var bodyFilePath = "insertBodyJs";
#if YANDEX_SDK
                bodyFilePath = "insertBodyJsY";
#elif VK_SDK
                bodyFilePath = "insertBodyJsV";
#else
                return new List<string>();
#endif
                return ProjectFileUtils
                    .ReadLinesFromFile(bodyFilePath)
                    .ToList();
            }
        }
    }
}