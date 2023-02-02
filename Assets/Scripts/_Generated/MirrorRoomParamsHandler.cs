using System;
using UnityEngine;
using UniRx;
using Mirror;

using Multiplayer.MatchState.domain.repositories;
using Multiplayer.MatchState.domain.model;
using System;
public class MirrorRoomParamsHandler: NetworkBehaviour
,IMatchStateRepository
,IMatchStateTimerRepository
{
[SerializeField][SyncVar(hook = nameof(IMatchStateRepositoryHook))] private MatchStates m_MatchStateParam;
private BehaviorSubject<MatchStates> m_MatchStateParamSubject;
[SerializeField][SyncVar(hook = nameof(IMatchStateTimerRepositoryHook))] private Int64 m_MatchStateTimerParam;
private BehaviorSubject<Int64> m_MatchStateTimerParamSubject;

    private void Awake() {
    m_MatchStateParamSubject = new BehaviorSubject<MatchStates>(m_MatchStateParam);
m_MatchStateTimerParamSubject = new BehaviorSubject<Int64>(m_MatchStateTimerParam);

    }
public MatchStates GetMatchState()=> m_MatchStateParam;

public void SetMatchState(MatchStates state)
{
m_MatchStateParam = state;
m_MatchStateParamSubject.OnNext(state);
}

public IObservable<MatchStates> GetMatchStateFlow()=> m_MatchStateParamSubject;
private void IMatchStateRepositoryHook(MatchStates first, MatchStates second)
{
m_MatchStateParamSubject.OnNext(second);
}

public Int64 GetTimer()=> m_MatchStateTimerParam;

public void SetTimer(Int64 timer)
{
m_MatchStateTimerParam = timer;
m_MatchStateTimerParamSubject.OnNext(timer);
}

public IObservable<Int64> GetTimerFlow()=> m_MatchStateTimerParamSubject;
private void IMatchStateTimerRepositoryHook(Int64 first, Int64 second)
{
m_MatchStateTimerParamSubject.OnNext(second);
}


}
