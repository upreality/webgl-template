using System;
using Utils.MirrorCodegen;

namespace Multiplayer.MatchState.domain.repositories
{
    [RoomParam]
    public interface IMatchStateTimerRepository
    {
        [RoomParamAttribute.ParamGetter]
        public long GetTimer();
        [RoomParamAttribute.ParamSetter]
        public void SetTimer(long timer);
        [RoomParamAttribute.ParamGetterFlow]
        public IObservable<long> GetTimerFlow();
    }
}