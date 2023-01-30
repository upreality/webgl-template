using UnityEngine;

namespace Utils.Misc
{
    public static class NumericInput
    {
        const int UndefinedKeyNumber = -1;

        private static readonly KeyCode[] KeyCodes =
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
        };

        public static bool GetNumericInput(out int keyNumber)
        {
            for (var i = 0; i < KeyCodes.Length; i++)
            {
                if (!Input.GetKeyDown(KeyCodes[i])) continue;
                keyNumber = i + 1;
                return true;
            }

            keyNumber = UndefinedKeyNumber;
            return false;
        }
    }
}