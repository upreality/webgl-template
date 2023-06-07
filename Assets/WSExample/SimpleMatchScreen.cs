using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ModestTree;
using TMPro;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace WSExample
{
    public class SimpleMatchScreen : MonoBehaviour
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)] private IWSCommandsUseCase commandsUseCase;
        [SerializeField] private RectTransform matchRoot;
        [SerializeField] private RectTransform lobbyRoot;

        private void Start()
        {
            commandsUseCase
                .Subscribe<MatchData>(Commands.MatchState)
                .Subscribe(HandleMatch)
                .AddTo(this);
        }

        private void HandleMatch(MatchData matchData)
        {
            var isRealMatch = !matchData.id.IsEmpty();
            matchRoot.gameObject.SetActive(isRealMatch);
            lobbyRoot.gameObject.SetActive(!isRealMatch);
            Debug.Log("Match state = " + isRealMatch);
            if (!isRealMatch)
                return;

            var userIds = "";
            matchData
                .participantIds
                .Select(id => id.ToString())
                .ToList()
                .ForEach(id => userIds += id + " ");
            // userIdsText.text = userIds;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private struct MatchData
        {
            public string id;
            public long createdTime;
            public long[] participantIds;
        }
    }
}