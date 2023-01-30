using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils.WebSocketClient.presentation.TicTac.presentation
{
    public class CellController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private GameObject cover;
        [SerializeField] private GameObject mineMark;
        [SerializeField] private GameObject opponentMark;

        private readonly BehaviorSubject<CellState> state = new(CellState.Empty);

        [CanBeNull] private IDisposable stateHandler;

        [CanBeNull] public Action ClickListener;

        private void OnEnable() => stateHandler = state.DistinctUntilChanged().Subscribe(UpdateState);
        private void OnDisable() => stateHandler?.Dispose();

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (state.Value != CellState.Empty) return;
            state.OnNext(CellState.Covered);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (state.Value != CellState.Covered) return;
            state.OnNext(CellState.Empty);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ClickListener?.Invoke();
        }

        public void SetMark(bool isMine)
        {
            var mark = isMine ? CellState.MineMark : CellState.OpponentMark;
            state.OnNext(mark);
        }

        public void Clear()
        {
            if (state.Value == CellState.Covered) return;
            state.OnNext(CellState.Empty);
        }

        private void UpdateState(CellState newState)
        {
            cover.SetActive(newState == CellState.Covered);
            mineMark.SetActive(newState == CellState.MineMark);
            opponentMark.SetActive(newState == CellState.OpponentMark);
        }

        private enum CellState
        {
            Empty,
            Covered,
            MineMark,
            OpponentMark
        }
    }
}