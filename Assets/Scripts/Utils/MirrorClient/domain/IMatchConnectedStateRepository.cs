using System;

namespace Utils.MirrorClient.domain
{
    public interface IMatchConnectedStateRepository
    {
        public IObservable<MatchConnectedState> GetConnectedState();
    }
    public enum MatchConnectedState
    {
        Disconnected,
        Connected
    }
}