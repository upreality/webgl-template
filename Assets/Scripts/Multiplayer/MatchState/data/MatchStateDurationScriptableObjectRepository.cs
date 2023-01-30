using System.Collections.Generic;
using Multiplayer.MatchState.domain.model;
using Multiplayer.MatchState.domain.repositories;
using UnityEngine;

namespace Multiplayer.MatchState.data
{
    [
        CreateAssetMenu
        (
            fileName = "MatchStateDuration SO Repository",
            menuName = "Repositories/MatchStateDuration SO Repository",
            order = 0
        )
    ]
    public class MatchStateDurationScriptableObjectRepository : ScriptableObject, IMatchStateDurationRepository
    {
        [SerializeField] private int matchDurationTimer = 60;
        [SerializeField] private int matchRestartTimer = 10;

        private Dictionary<MatchStates, int> statesToDurations;
        private Dictionary<MatchStates, int> StatesToDurations => statesToDurations ??= new Dictionary<MatchStates, int>
        {
            {MatchStates.Finished, matchRestartTimer},
            {MatchStates.Playing, matchDurationTimer},
        };

        public bool HasStateDuration(MatchStates state) => StatesToDurations.ContainsKey(state);

        public bool GetStateDuration(MatchStates state, out int duration)
        {
            var hasState = HasStateDuration(state);
            duration = hasState? StatesToDurations[state] : 0;
            return hasState;
        }
    }
}