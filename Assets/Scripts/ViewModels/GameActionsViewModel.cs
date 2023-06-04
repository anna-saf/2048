using System;
using UnityEngine;
using System.Diagnostics;
using UniRx;

public class GameActionsViewModel
{
    public ReactiveProperty<int> score { get; private set; }
    private CellView[,] cellViews;
    private DeckModel model;
    private GameScoreViewModel scoreViewModel;
    //private CellAnimator cellAnimator;

    public void Init()
    {
        scoreViewModel = new GameScoreViewModel();
        model = DeckModel.Instance;
        score = model.score;
        score.Value = 0;
    }

    public void MoveCells(SwipeData swipeData, CellView[,] cells)
    {
        cellViews = cells;

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
        for (int col = 0; col < model.DeckSize; col++)
        {
            for (int row = rowCounterStart; row != direction * -1 * (rowCounterStart - model.DeckSize); row += direction)
            {
                if (cellViews[row, col].cellViewModel.num != model.EmptyElement.value)
                {
                    OffsetCellVertical(direction, col, row, rowCounterStart);
                }
            }
        }
    }

    private void OffsetCellVertical(int direction, int column, int startRow, int firstRow)
    {
        bool isCellMerged = FindCellToMergeVertical(direction, column, startRow, firstRow);
        if (!isCellMerged)
        {
            FindEmptyCellVertical(direction, column, startRow, firstRow);
        }
    }

    private bool FindCellToMergeVertical(int direction, int column, int startRow, int firstRow)
    {
        for (int row = startRow; row != firstRow; row -= direction)
        {
            if (cellViews[row - direction, column].cellViewModel.num == model.EmptyElement.value)
            {
                //Ќет числа, продолжаем поиск
                continue;
            }
            else
            {
                if(cellViews[row - direction, column].cellViewModel.num == cellViews[startRow, column].cellViewModel.num)
                {
                    if (!cellViews[startRow, column].cellViewModel.alreadyMerged && !cellViews[row - direction, column].cellViewModel.alreadyMerged)
                    {
                        //ћожно сложить 2 числа
                        string sum = MergeCells(startRow, column, row - direction, column);

                        GameManager.Instance.IsWin(sum);

                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }

    private void FindEmptyCellVertical(int direction, int column, int startRow, int firstRow)
    {
        if (startRow != firstRow && (cellViews[startRow - direction, column].cellViewModel.num == model.EmptyElement.value))
        {
            for (int row = startRow; row != firstRow; row -= direction)
            {
                if ((cellViews[row - direction, column].cellViewModel.num != model.EmptyElement.value) && row != startRow)
                {
                    //можно сдвинуть до первого непустого числа
                    SetTransition(startRow, column, row, column);
                    return;
                }
            }
            //можно сдвинуть до самой первой €чейки
            SetTransition(startRow, column, firstRow, column);
        }
    }

    private void MoveHorizontal(int columnCounterStart, int direction)
    {
        for (int row = 0; row < model.DeckSize; row++)
        {
            for (int col = columnCounterStart; col != direction*-1*(columnCounterStart - model.DeckSize); col+=direction)
            {
                if (cellViews[row, col].cellViewModel.num != model.EmptyElement.value)
                {
                    OffsetCellHorizontal(direction, col, row, columnCounterStart);
                }
            }
        }
    }

    private void OffsetCellHorizontal(int direction, int startColumn, int row, int firstColumn)
    {
        bool isCellMerged = FindCellToMergeHorizontal(direction, startColumn, row, firstColumn);
        if (!isCellMerged)
        {
            FindEmptyCellHorizontal(direction, startColumn, row, firstColumn);
        }
    }

    private bool FindCellToMergeHorizontal(int direction, int startColumn, int row, int firstColumn)
    {
        for (int col = startColumn; col != firstColumn; col -= direction)
        {
            if (cellViews[row, col - direction].cellViewModel.num == model.EmptyElement.value)
            {
                //Ќет числа, продолжаем поиск
                continue;
            }
            else
            {
                if (cellViews[row, col - direction].cellViewModel.num == cellViews[row, startColumn].cellViewModel.num)
                {
                    if (!cellViews[row, startColumn].cellViewModel.alreadyMerged && !cellViews[row, col - direction].cellViewModel.alreadyMerged)
                    {
                        //ћожно сложить 2 числа
                        string sum = MergeCells(row, startColumn, row, col - direction);

                        GameManager.Instance.IsWin(sum);

                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }

    private void FindEmptyCellHorizontal(int direction, int startColumn, int row, int firstColumn)
    {
        if (startColumn != firstColumn && (cellViews[row, startColumn - direction].cellViewModel.num == model.EmptyElement.value))
        {
            for (int col = startColumn; col != firstColumn; col -= direction)
            {
                if ((cellViews[row, col - direction].cellViewModel.num != model.EmptyElement.value) && col != startColumn)
                {
                    //можно сдвинуть до первого непустого числа
                    SetTransition(row, startColumn, row, col);
                    return;
                }
            }
            //можно сдвинуть до самой первой €чейки
            SetTransition(row, startColumn, row, firstColumn);
        }
    }

    private void SetTransition(int startCellRow, int startCellCol, int endCellRow, int endCellCol)
    {
        cellViews[endCellRow, endCellCol].cellViewModel.ChangeElementColorNum(
        model.GetColorByNum(cellViews[startCellRow, startCellCol].cellViewModel.num),
                    cellViews[startCellRow, startCellCol].cellViewModel.num
                    );
        cellViews[startCellRow, startCellCol].cellViewModel.ChangeElementNumColorSO(model.EmptyElement);

        CellAnimator.Instance.CellTransition(cellViews[startCellRow, startCellCol], cellViews[endCellRow, endCellCol], false);

        //cellViews[endCellRow, endCellCol].cellViewModel.VisualUpdate();
        //cellViews[startCellRow, startCellCol].cellViewModel.VisualUpdate();
    }

    private string MergeCells(int startCellRow, int startCellCol, int endCellRow, int endCellCol)
    {
        cellViews[endCellRow, endCellCol].cellViewModel.alreadyMerged = true;
        scoreViewModel.UpdateScore(Convert.ToInt32(cellViews[startCellRow, startCellCol].cellViewModel.num));
        string newNum = (Convert.ToInt32(cellViews[startCellRow, startCellCol].cellViewModel.num) +
                        Convert.ToInt32(cellViews[endCellRow, endCellCol].cellViewModel.num)).ToString();
        cellViews[endCellRow, endCellCol].cellViewModel.ChangeElementColorNum(model.GetColorByNum(newNum), newNum);
        cellViews[startCellRow, startCellCol].cellViewModel.ChangeElementNumColorSO(model.EmptyElement);
        CellAnimator.Instance.CellTransition(cellViews[startCellRow, startCellCol], cellViews[endCellRow, endCellCol], true);

        //cellViews[endCellRow, endCellCol].cellViewModel.VisualUpdate();
        //cellViews[startCellRow, startCellCol].cellViewModel.VisualUpdate();

        return newNum;
    }
}
