using System;
using Utils.MirrorCodegen;

namespace Multiplayer.Health.domain.repositories
{
    [PlayerParam]
    public interface IHealthHandlersRepository
    {
        [PlayerParamAttribute.PlayerParamGetterAttribute]
        public int GetHealth(string handlerId);
        [PlayerParamAttribute.PlayerParamSetterAttribute]
        public void SetHealth(string handlerId, int health);
        [PlayerParamAttribute.PlayerParamGetterFlowAttribute]
        public IObservable<int> GetHealthFlow(string handlerId);
    }
}