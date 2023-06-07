using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace WSExample.TicTac.presentation
{
    public class SimpleGameInputController: MonoBehaviour
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        private CompositeDisposable enabledDisposable = new();

        private readonly ReactiveProperty<MovementInput> currentInput = new(MovementInput.None);

        private void OnEnable()
        {
            enabledDisposable = new CompositeDisposable();

            currentInput
                .DistinctUntilChanged()
                .Subscribe(SendInputChange)
                .AddTo(enabledDisposable);
        }

        private void OnDisable() => enabledDisposable.Dispose();

        private void Update()
        {
            var input = MovementInput.None;
            if (Input.GetKey(KeyCode.A))
                input = MovementInput.Backward;
            else if (Input.GetKey(KeyCode.D))
                input = MovementInput.Forward;

            currentInput.Value = input;
        }

        private void SendInputChange(MovementInput input)
        {
            commandsUseCase
                .Request<long, long>(Commands.SetMovementData, (int)input)
                .DistinctUntilChanged()
                .Subscribe()
                .AddTo(enabledDisposable);
        }

        private enum MovementInput
        {
            None = 0,
            Forward = 1,
            Backward = 2
        }
    }
}