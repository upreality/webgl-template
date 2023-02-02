using System;
using Features.Cameras.data;
using Features.Cameras.domain;
using ModestTree;
using Multiplayer.MatchState.domain.model;
using Multiplayer.MatchState.domain.repositories;
using UniRx;
using UnityEngine;
using Utils.StringSelector;
using Utils.ZenjectCodegen;
using Zenject;

namespace Features.Cameras.presentation.Sync
{
    // [SceneSubscriptionHandler]
    public class CamSyncHandler : MonoBehaviour
    {
        [Inject] private IMatchStateRepository matchStateRepository;
        [Inject] private IActiveCameraRepository activeCameraRepository;

        [SerializeField, StringSelector(typeof(CameraTypeSORepository))]
        private string overviewCamTypeId;

        [SerializeField, StringSelector(typeof(CameraTypeSORepository))]
        private string finishedCamTypeId;

        [SerializeField, StringSelector(typeof(CameraTypeSORepository))]
        private string playerCamTypeId;

        private void Start() => HandleActiveCam().AddTo(this);

        // [SceneSubscription]
        private IDisposable HandleActiveCam() => matchStateRepository
            .GetMatchStateFlow()
            .Do(_ => Debug.Log(""))
            .Select(GetCamType)
            .Subscribe(activeCameraRepository.SetActiveCamera);

        private string GetCamType(MatchStates state) => state switch
        {
            MatchStates.None => overviewCamTypeId,
            MatchStates.Preparing => overviewCamTypeId,
            MatchStates.Playing => playerCamTypeId,
            MatchStates.Finished => finishedCamTypeId,
            _ => overviewCamTypeId
        };
    }
}