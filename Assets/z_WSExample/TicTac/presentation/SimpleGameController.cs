using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Utils.WebSocketClient.presentation.TicTac.presentation;
using Zenject;

namespace WSExample.TicTac.presentation
{
    public class SimpleGameController : MonoBehaviour
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)] private IWSCommandsUseCase commandsUseCase;

        [SerializeField] private TMP_Text opponentNickText;

        [Header("Cells")] [SerializeField] private CellController cellPrefab;
        [SerializeField] private GridLayoutGroup grid;
        [SerializeField] private Transform cellPoolRoot;
        [SerializeField] private int spacing = 10;

        [Header("Finished")] [SerializeField] private GameObject finishedScreen;
        [SerializeField] private GameObject looseMark;
        [SerializeField] private GameObject winMark;
        [SerializeField] private TMP_Text earned;
        [SerializeField] private GameObject drawMark;

        [SerializeField] private GameObject opponentTurnPlaceholder;
        [SerializeField] private GameObject yourTurnPlaceholder;

        private readonly List<CellController> poolCells = new();
        private readonly List<CellController> cells = new();

        private CompositeDisposable enabledDisposable = new();

        private readonly GameState emptyGameState = new()
        {
            cellStates = Array.Empty<int>(),
            gameState = 1,
            gridSize = 0,
            isPlayerTurn = false,
            isWinner = false,
            isDraw = false,
            opponentNick = string.Empty,
            reward = 0
        };

        private void OnEnable()
        {
            enabledDisposable = new CompositeDisposable();

            HandleGameState(emptyGameState);
            yourTurnPlaceholder.SetActive(false);
            grid.spacing = Vector2.one * spacing;

            commandsUseCase
                .Request<GameState>(Commands.TicTacState)
                .Subscribe(HandleGameState)
                .AddTo(enabledDisposable);
            commandsUseCase
                .Listen<CellUpdate>(Commands.TicTacCellUpdates)
                .Subscribe(HandleCellUpdate)
                .AddTo(enabledDisposable);
            commandsUseCase
                .Listen<bool>(Commands.TicTacTurnUpdates)
                .Subscribe(HandleTurnUpdate)
                .AddTo(enabledDisposable);
            commandsUseCase
                .Listen<FinishedData>(Commands.TicTacFinished)
                .Subscribe(HandleFinish)
                .AddTo(enabledDisposable);
        }

        private void HandleGameState(GameState state)
        {
            var cellStates = state.cellStates;
            UpdateCells(cellStates.Length);
            SetupCells(state.gridSize);
            for (var i = 0; i < cellStates.Length; i++)
                UpdateCellState(i, cellStates[i]);

            finishedScreen.SetActive(state.gameState == 2);
            winMark.SetActive(state.isWinner);
            earned.gameObject.SetActive(false);
            looseMark.SetActive(!state.isWinner);
            drawMark.SetActive(state.isDraw);
            yourTurnPlaceholder.SetActive(state.isPlayerTurn);
            opponentTurnPlaceholder.SetActive(!state.isPlayerTurn);
            opponentNickText.text = state.opponentNick;
        }

        private void HandleCellUpdate(CellUpdate update)
        {
            if (cells.Count <= update.cellPos)
                return;

            cells[update.cellPos].SetMark(update.isMine);
        }

        private void HandleTurnUpdate(bool isMineTurn)
        {
            yourTurnPlaceholder.SetActive(isMineTurn);
            opponentTurnPlaceholder.SetActive(!isMineTurn);
        }

        private void HandleFinish(FinishedData data)
        {
            finishedScreen.SetActive(data.finished);
            winMark.SetActive(data.isWinner && !data.isDraw);
            earned.gameObject.SetActive(data.isWinner);
            earned.text = $"Earned {data.reward} coins!";
            looseMark.SetActive(!data.isWinner && !data.isDraw);
            drawMark.SetActive(data.isDraw);
        }

        private void UpdateCellState(int cellIndex, int state)
        {
            var cell = cells[cellIndex];
            if (state == 0)
            {
                cell.Clear();
                return;
            }

            cell.SetMark(state == 1);
        }

        private void UpdateCells(int cellsCount)
        {
            var extraCellsCount = cells.Count - cellsCount;
            while (extraCellsCount-- > 0)
            {
                var cell = cells[0];
                cells.RemoveAt(0);
                poolCells.Add(cell);
                cell.transform.SetParent(cellPoolRoot);
            }

            var requiredCellsCount = cellsCount - cells.Count;
            while (requiredCellsCount-- > 0)
            {
                CellController cell;
                if (poolCells.Count > 0)
                {
                    cell = poolCells[0];
                    poolCells.RemoveAt(0);
                }
                else
                {
                    cell = Instantiate(cellPrefab);
                }

                cells.Add(cell);
                cell.transform.SetParent(grid.transform);
            }
        }

        private void SetupCells(int size)
        {
            if (size <= 0)
                return;

            var gridTransform = grid.transform as RectTransform;
            var gridCellSpace = gridTransform.rect.size.x - (size - 1) * spacing;
            grid.cellSize = Vector2.one * gridCellSpace / size;

            for (var i = 0; i < cells.Count; i++)
            {
                var cellIndex = i;
                cells[i].ClickListener = () => Mark(cellIndex);
            }
        }

        private void Mark(int cellId) => commandsUseCase
            .Request<bool, int>(Commands.TicTacMakeTurn, cellId)
            .Subscribe()
            .AddTo(this);

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private struct GameState
        {
            public int[] cellStates;
            public int gameState;
            public int gridSize;
            public bool isPlayerTurn;
            public bool isWinner;
            public bool isDraw;
            public long reward;
            public string opponentNick;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private struct CellUpdate
        {
            public int cellPos;
            public bool isMine;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private struct FinishedData
        {
            public bool finished;
            public bool isWinner;
            public bool isDraw;
            public long reward;
        }
    }
}