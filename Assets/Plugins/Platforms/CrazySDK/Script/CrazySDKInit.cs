#if CRAZY_SDK
using UnityEngine;

namespace Plugins.Platforms.CrazySDK.Script
{
    internal class CrazySDKInit
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnRuntimeMethodLoad()
        {
            var sdk = CrazySDK.Instance; // Trigger init by calling instance
        }
    }
}
#endif