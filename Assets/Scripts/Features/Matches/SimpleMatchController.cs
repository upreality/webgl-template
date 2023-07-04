using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Core.Auth.domain;
using ModestTree;
using TMPro;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Features.Matches
{
    public class SimpleMatchController : MonoBehaviour
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)] private IWSCommandsUseCase commandsUseCase;
        [SerializeField] private RectTransform matchRootUI;
        [SerializeField] private RectTransform lobbyRootUI;
        [SerializeField] private GameObject matchRoot;
        [SerializeField] private TMP_Text userIdsText;

        private void Start()
        {
            matchRoot.SetActive(false);
            matchRootUI.gameObject.SetActive(false);
            lobbyRootUI.gameObject.SetActive(true);
            commandsUseCase
                .Subscribe<MatchData>(Commands.MatchState)
                .Subscribe(HandleMatch)
                .AddTo(this);
        }

        private void HandleMatch(MatchData matchData)
        {
            var isRealMatch = !matchData.id.IsEmpty();
            matchRoot.SetActive(isRealMatch);
            matchRootUI.gameObject.SetActive(isRealMatch);
            lobbyRootUI.gameObject.SetActive(!isRealMatch);
            Debug.Log("Match state = " + isRealMatch);
            if (!isRealMatch)
                return;

            var userIds = "";
            matchData
                .participantIds
                .Select(id => id.ToString())
                .ToList()
                .ForEach(id => userIds += id + " ");
            userIdsText.text = userIds;
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