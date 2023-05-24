using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameActionsViewModel : MonoBehaviour, IGameActionsViewModel
{
    public ReactiveProperty<int> score { get; private set; }
    private ICellViewModel[,] cellViewModels;
    private DeckModel model;

    private void Start()
    {
        model = DeckModel.Instance;
        score = model.score;
        score.Value = 0;
    }

    public void MoveCells(SwipeData swipeData, ICellViewModel[,] cells)
    {
        cellViewModels = cells;

        switch (swipeData.direction)
        {
            case SwipeDirection.Up:
                MoveVertical(0, 1);
                break;
            case SwipeDirection.Down:
                MoveVertical(model.DeckSize - 1, -1);
                break;
            case SwipeDirection.Left:
                MoveHorizontal(0, 1);
                break;
            case SwipeDirection.Right:
                MoveHorizontal(model.DeckSize - 1, -1);
                break;
            default:
                break;
        }
        
    }

    private void MoveVertical(int rowCounterStart, int direction)
    {
        int lastRowForOffset = Math.Abs(rowCounterStart - (model.DeckSize - 1));
        SkipEmptyCellsVertical(rowCounterStart, direction, lastRowForOffset);
        MergeEqualsCellsVertical(rowCounterStart, direction, lastRowForOffset);
    }
    
    private void SkipEmptyCellsVertical(int rowCounterStart, int direction, int lastRowForOffset)
    {
        for (int c = 0; c < model.DeckSize; c++)
        {
            int rowCounter = rowCounterStart;
            for (int r = 0; r < model.DeckSize; r++)
            {
                if (cellViewModels[rowCounter, c].Num.Value == model.EmptyElement.value)
                {

                    OffsetElementVertical(rowCounter, c, lastRowForOffset, direction);
                    rowCounter -= direction;
                }
                rowCounter += direction;
            }
        }
    }

    private void MergeEqualsCellsVertical(int rowCounterStart, int direction, int lastRowForOffset)
    {
        for (int c = 0; c < model.DeckSize; c++)
        {
            int rowCounter = rowCounterStart;
            for (int r = 0; r < model.DeckSize - 1; r++)
            {
                if (cellViewModels[rowCounter, c].Num.Value != model.EmptyElement.value)
                {
                    if (cellViewModels[rowCounter, c].Num.Value == cellViewModels[rowCounter + direction, c].Num.Value)
                    {
                        cellViewModels[rowCounter + direction, c].ChangeElementNumColorSO(model.EmptyElement);
                        OffsetElementVertical(rowCounter + direction, c, lastRowForOffset, direction);
                        string sum = SumDeckElements(cellViewModels[rowCounter, c]);
                        bool endGame = GameManager.Instance.IsWin(sum);
                        if (endGame)
                        {
                            return;
                        }

                    }
                    rowCounter += direction;
                }
                else break;
            }
        }
    }

    private void OffsetElementVertical(int startIdx, int c, int lastRowForChange, int direction)
    {
        for (int r = startIdx; r != lastRowForChange; r += direction)
        {
            cellViewModels[r, c].ChangeElementColorNum(model.GetColorByNum(cellViewModels[r + direction, c].Num.Value), cellViewModels[r + direction, c].Num.Value);
        }

        cellViewModels[lastRowForChange, c].ChangeElementNumColorSO(model.EmptyElement);
    }

    private void MoveHorizontal(int columnCounterStart, int direction)
    {
        int lastColumnForOffset = Math.Abs(columnCounterStart - (model.DeckSize - 1));
        SkipEmptyCellsHorizontal(columnCounterStart, direction, lastColumnForOffset);
        MergeEqualsCellsHorizontal(columnCounterStart, direction, lastColumnForOffset);
    }

    private void SkipEmptyCellsHorizontal(int columnCounterStart, int direction, int lastColumnForOffset)
    {
        for (int r = 0; r < model.DeckSize; r++)
        {
            int colCounter = columnCounterStart;
            for (int c = 0; c < model.DeckSize; c++)
            {
                if (cellViewModels[r, colCounter].Num.Value == model.EmptyElement.value)
                {
                    OffsetElementHorizontal(colCounter, r, lastColumnForOffset, direction);
                    colCounter -= direction;
                }
                colCounter += direction;
            }
        }
    }

    private void MergeEqualsCellsHorizontal(int columnCounterStart, int direction, int lastColumnForOffset)
    {
        for (int r = 0; r < model.DeckSize; r++)
        {
            int colCounter = columnCounterStart;
            for (int c = 0; c < model.DeckSize - 1; c++)
            {
                if (cellViewModels[r, colCounter].Num.Value != model.EmptyElement.value)
                {
                    if (cellViewModels[r, colCounter].Num.Value == cellViewModels[r, colCounter + direction].Num.Value)
                    {
                        cellViewModels[r, colCounter + direction].ChangeElementNumColorSO(model.EmptyElement);
                        OffsetElementHorizontal(colCounter + direction, r, lastColumnForOffset, direction);
                        string sum = SumDeckElements(cellViewModels[r, colCounter]);
                        bool endGame = GameManager.Instance.IsWin(sum);
                        if (endGame)
                        {
                            return;
                        }
                    }
                    colCounter += direction;
                }
                else break;
            }
        }
    }

    private void OffsetElementHorizontal(int startIdx, int r, int lastColumnForChange, int direction)
    {
        for (int c = startIdx; c != lastColumnForChange; c += direction)
        {
            cellViewModels[r, c].ChangeElementColorNum(model.GetColorByNum(cellViewModels[r, c + direction].Num.Value), cellViewModels[r, c + direction].Num.Value);
        }
        cellViewModels[r, lastColumnForChange].ChangeElementNumColorSO(model.EmptyElement);
    }

    private string SumDeckElements(ICellViewModel deckElement)
    {
        model.score.Value += Convert.ToInt32(deckElement.Num.Value);
        string text = (Convert.ToInt32(deckElement.Num.Value) * 2).ToString();
        deckElement.ChangeElementColorNum(model.GetColorByNum(text), text);

        return text;
    }

}
