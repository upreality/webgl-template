using System;
using HNS.Model;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace HNS
{
    public class SeekerController : MonoBehaviour
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private GameObject cam;

        public bool isPlayer;
        public Func<bool> isMovable;

        public float speed = 5f;

        private void Start() => Observable
            .Timer(TimeSpan.FromMilliseconds(100))
            .Repeat()
            .Where(_ => isMovable())
            .Select(_ => transform.GetSnapshot())
            .Subscribe(SendSnapshot)
            .AddTo(this);

        private void SendSnapshot(TransformSnapshot snapshot) => commandsUseCase
            .Request<long, TransformSnapshot>(Commands.Movement, snapshot)
            .Subscribe()
            .AddTo(this);

        public void SubmitSnapshot(SeekerSnapshotItem snapshot)
        {
            cam.SetActive(isPlayer);
            if (!isPlayer)
            {
                agent.destination = snapshot.transform.Pos;
                // transform.ApplySnapshot(snapshot.transform);
            }
        }

        private void Update()
        {
            agent.enabled = !isPlayer;
            if (!isPlayer || !isMovable())
                return;

            transform.position += new Vector3
            {
                x = Input.GetAxis("Horizontal"),
                z = Input.GetAxis("Vertical"),
            } * (Time.deltaTime * speed);
            ;
        }
    }
}