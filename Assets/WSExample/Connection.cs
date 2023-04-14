using System;
using System.Collections.Generic;
using System.Text;
using ModestTree;
using NativeWebSocket;
using UnityEngine;

namespace WSExample
{
    public class Connection : MonoBehaviour
    {
        [Header("Url")] [SerializeField] private string host = "localhost";
        [SerializeField] private string port = "8080";
        [SerializeField] private string path = "websocket";
        [Header("Auth")] [SerializeField] private string username;
        [SerializeField] private string password;

        private WebSocket websocket;

        private string URL
        {
            get
            {
                var root = port.IsEmpty() ? host : $"{host}:{port}";
                return $"ws://{root}/{path}";
            }
        }

        private byte[] AuthData => Encoding.UTF8.GetBytes($"{username}:{password}");
        private string AuthHeader => "Basic " + Convert.ToBase64String(AuthData);

        private async void Start()
        {
            var headers = CreateHeaders();

            websocket = new WebSocket(URL, headers);

            websocket.OnOpen += () => { Debug.Log("Connection open!"); };

            websocket.OnError += (e) => { Debug.LogError("Error! " + e); };

            websocket.OnClose += (closeCode) => { Debug.LogWarning("Connection closed! Reason: " + closeCode); };

            websocket.OnMessage += (bytes) =>
            {
                // Reading a plain text message
                var message = Encoding.UTF8.GetString(bytes);
                Debug.Log("Received OnMessage! (" + bytes.Length + " bytes) " + message);
            };

            await websocket.Connect();
        }

        private Dictionary<string, string> CreateHeaders()
        {
            var headers = new Dictionary<string, string>
            {
                { "Authorization", AuthHeader }
            };
            return headers;
        }

#if !UNITY_WEBGL || UNITY_EDITOR
        private void Update()
        {
            websocket.DispatchMessageQueue();
        }
#endif

        private async void OnApplicationQuit()
        {
            await websocket.Close();
        }
    }
}