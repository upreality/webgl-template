using System;
using Multiplayer.MatchState.domain.model;
using Utils.MirrorCodegen;

namespace Multiplayer.MatchState.domain.repositories
{
    [RoomParam]
    public interface IMatchStateRepository
    {
        [RoomParamAttribute.ParamSetter]
        void SetMatchState(MatchStates state);
        [RoomParamAttribute.ParamGetterFlow]
        IObservable<MatchStates> GetMatchStateFlow();
        [RoomParamAttribute.ParamGetter]
        MatchStates GetMatchState();
    }
}