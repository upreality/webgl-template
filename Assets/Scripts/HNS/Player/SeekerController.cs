using System;
using HNS.data;
using HNS.domain;
using HNS.Model;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace HNS.Player
{
    public class SeekerController: MonoBehaviour
    {
        [Inject] private HNSSnapshotRepository snapshots;
        [Inject] private PlayerViewPrefabProvider playerViewPrefabProvider;
        [Inject] private CatcherHandsRepository catcherHandsRepository;
        
        [SerializeField] private PlayerIdProvider idProvider;
        
        [SerializeField] private Transform viewRoot;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private NavMeshAgent agent;
        
        private Animator characterAnimator;

        private bool initialized;

        private long playerId;

        private void Start() => SnapshotFlow
            .Do(Initialize)
            .DoOnCompleted(Destroy)
            .Subscribe(HandleSnapshot)
            .AddTo(gameObject);

        private void Update()
        {
            rb.isKinematic = agent.isOnNavMesh;
        }

        private void Destroy()
        {
            StopAllCoroutines();
            catcherHandsRepository.Remove(playerId);
            Destroy(gameObject);
        }
        
        private IObservable<SeekerSnapshotItem> SnapshotFlow => idProvider
            .PlayerIdFlow
            .Do(id => playerId = id)
            .Select(GetSnapshotFlow)
            .Switch();

        private IObservable<SeekerSnapshotItem> GetSnapshotFlow(long playerId) => snapshots
            .GetSeeker(playerId, out var flow)
            ? flow
            : Observable.Empty<SeekerSnapshotItem>();

        private void Initialize(SeekerSnapshotItem item)
        {
            if (initialized)
                return;

            var view = playerViewPrefabProvider.GetCharacter(item.character);
            var spawnedCharacter = Instantiate(view, viewRoot);
            var spawnedTransform = spawnedCharacter.transform;
            spawnedTransform.localPosition = Vector3.zero;
            characterAnimator = spawnedTransform.GetComponent<Animator>();
            var hands = spawnedTransform.Find("HandPos");
            catcherHandsRepository.Set(playerId, hands.position);
            transform.ApplySnapshot(item.transform);
            initialized = true;
        }
        
        private void HandleSnapshot(SeekerSnapshotItem item)
        {
            var isHandling = item.catchedHiderId != null;
            characterAnimator.SetBool("Handling", isHandling);
            agent.destination = item.transform.Pos;
            characterAnimator.SetBool("Running", agent.velocity.magnitude > 0.1f);
        }
    }
}