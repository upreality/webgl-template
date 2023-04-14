using Mirror;
using Multiplayer.PlayerManagement.domain;
using Zenject;

namespace Multiplayer.PlayerManagement
{
    public class CustomNetworkManager : NetworkManager
    {
        [Inject] private IPlayerIdsRepository playerIdsRepository;

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            playerIdsRepository.Remove(conn.connectionId.ToString());
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);
            playerIdsRepository.Add(conn.connectionId.ToString());
        }
    }
}