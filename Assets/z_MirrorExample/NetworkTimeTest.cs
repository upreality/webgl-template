using Mirror;
using UnityEngine;
using Utils.Reactive;

public class NetworkTimeTest : NetworkBehaviour
{
    private NetworkTimeData CurrentTimeData => new NetworkTimeData()
    {
        networkTime = NetworkTime.time,
        localTime = NetworkTime.localTime,
        offset = NetworkTime.offset,
        rtt = NetworkTime.rtt,
    };

    public override void OnStartAuthority() => this.CreateTimer(3000, () => LogClientTime(CurrentTimeData));

    [Command]
    private void LogClientTime(NetworkTimeData data)
    {
        Debug.Log("Client time: " + JsonUtility.ToJson(data));
        Debug.Log("Server time: " + JsonUtility.ToJson(CurrentTimeData));
    }

    private struct NetworkTimeData
    {
        public double networkTime;
        public double localTime;
        public double offset;
        public double rtt;
    }
}