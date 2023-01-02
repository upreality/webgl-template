using UnityEngine;

namespace Utils.WebSocketClient.data
{
    [CreateAssetMenu(menuName = "Settings/Web Socket Settings")]
    public class WSSettings : ScriptableObject
    {
        [SerializeField] private string host = "localhost";
        [SerializeField] private string port = "8080";
        [SerializeField] private string path = "websocket";
        public bool logConnection = true;
        public bool logMessages = true;

        public string URL => $"wss://{host}:{port}/{path}";
    }
}