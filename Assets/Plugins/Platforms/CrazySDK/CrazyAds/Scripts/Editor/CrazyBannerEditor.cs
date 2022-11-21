using UnityEditor;

namespace Plugins.Platforms.CrazySDK.CrazyAds.Scripts.Editor
{
    [CustomEditor(typeof(CrazyBanner))]
    public class CrazyBannerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var script = (CrazyBanner)target;
            var newValue = EditorGUILayout.EnumPopup("Banner size", script.Size);
            script.Size = (CrazyBanner.BannerSize)newValue;
            EditorUtility.SetDirty(target);
        }
    }
}