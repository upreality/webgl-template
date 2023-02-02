using System;
using UnityEngine;
using Mirror;
using Utils.MirrorUtils;
using Utils.Reactive;

using Multiplayer.Health.domain.repositories;
using System;
public class MirrorPlayerParamsHandler: NetworkBehaviour
,IHealthHandlersRepository
{
[SerializeField] private readonly SyncReactiveDictionary<string,Int32> m_playerIdToHealthHandlers = new();

public Int32 GetHealth(string handlerId)=> m_playerIdToHealthHandlers[handlerId];

public void SetHealth(string handlerId, Int32 health)
{
if(!isServer) return;
m_playerIdToHealthHandlers[handlerId] = health;
}

public IObservable<Int32> GetHealthFlow(string handlerId)=> m_playerIdToHealthHandlers.GetItemFlow(handlerId);


}
