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
        //cellAnimator = new CellAnimator(model.SpawnTransform);
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
        for (int c = 0; c < model.DeckSize; c++)
        {
            int rowCounter = rowCounterStart;
            for (int r = 0; r < model.DeckSize; r++)
            {
                if (cellViews[rowCounter, c].cellViewModel.Num.Value != model.EmptyElement.value)
                {
                    OffsetCellVertical(direction, c, rowCounter, rowCounterStart);
                }

                rowCounter += direction;
            }
        }
        //int lastRowForOffset = Math.Abs(rowCounterStart - (model.DeckSize - 1));
        //SkipEmptyCellsVertical(rowCounterStart, direction, lastRowForOffset);
        //MergeEqualsCellsVertical(rowCounterStart, direction, lastRowForOffset);
    }

    private void OffsetCellVertical(int direction, int column, int startRow, int firstRow)
    {
        for(int row = startRow; row != firstRow; row -= direction)
        {
            if (cellViews[row-direction, column].cellViewModel.Num.Value == model.EmptyElement.value)
            {
                cellViews[row-direction, column].cellViewModel.ChangeElementColorNum(
                    model.GetColorByNum(cellViews[row, column].cellViewModel.Num.Value), 
                    cellViews[row, column].cellViewModel.Num.Value
                    );
                cellViews[row, column].cellViewModel.ChangeElementNumColorSO(model.EmptyElement);

                //CellAnimator.Instance.CellTransition(cellViews[row, column], cellViews[row - direction, column], true);

                cellViews[row - direction, column].cellViewModel.VisualUpdate();
                cellViews[row, column].cellViewModel.VisualUpdate();
            }
            else
            {
                TryMergeVertical(row, column, direction);
            }
        }
    }

    private void TryMergeVertical(int row, int column, int direction)
    {
        if ((cellViews[row, column].cellViewModel.Num.Value == cellViews[row-direction, column].cellViewModel.Num.Value) && 
            !cellViews[row - direction, column].cellViewModel.alreadyMerged &&
            !cellViews[row, column].cellViewModel.alreadyMerged)
        {
            cellViews[row - direction, column].cellViewModel.alreadyMerged = true;
            scoreViewModel.UpdateScore(Convert.ToInt32(cellViews[row, column].cellViewModel.Num.Value));
            string newNum = (Convert.ToInt32(cellViews[row, column].cellViewModel.Num.Value) + 
                            Convert.ToInt32(cellViews[row-direction, column].cellViewModel.Num.Value)).ToString();

            cellViews[row-direction, column].cellViewModel.ChangeElementColorNum(model.GetColorByNum(newNum), newNum);
            cellViews[row, column].cellViewModel.ChangeElementNumColorSO(model.EmptyElement);

            //CellAnimator.Instance.CellTransition(cellViews[row, column], cellViews[row-direction, column], true);

            cellViews[row - direction, column].cellViewModel.VisualUpdate();
            cellViews[row, column].cellViewModel.VisualUpdate();

            bool endGame = GameManager.Instance.IsWin(newNum);
            if (endGame)
            {
                return;
            }
        }
    }

    private void MoveHorizontal(int columnCounterStart, int direction)
    {
        for (int r = 0; r < model.DeckSize; r++)
        {
            int colCounter = columnCounterStart;
            for (int c = 0; c < model.DeckSize; c++)
            {
                if (cellViews[r, colCounter].cellViewModel.Num.Value != model.EmptyElement.value)
                {
                    OffsetCellHorizontal(direction, colCounter, r, columnCounterStart);
                }

                colCounter += direction;
            }
        }
    }

    private void OffsetCellHorizontal(int direction, int startColumn, int row, int firstColumn)
    {
        for (int column = startColumn; column != firstColumn; column -= direction)
        {
            if (cellViews[row, column - direction].cellViewModel.Num.Value == model.EmptyElement.value)
            {
                cellViews[row, column - direction].cellViewModel.ChangeElementColorNum(
                    model.GetColorByNum(cellViews[row, column].cellViewModel.Num.Value),
                    cellViews[row, column].cellViewModel.Num.Value
                    );
                cellViews[row, column].cellViewModel.ChangeElementNumColorSO(model.EmptyElement);

                //CellAnimator.Instance.CellTransition(cellViews[row, column], cellViews[row, column - direction], true);

                cellViews[row, column - direction].cellViewModel.VisualUpdate();
                cellViews[row, column].cellViewModel.VisualUpdate();
            }
            else
            {
                TryMergeHorizontal(row, column, direction);
            }
        }
    }
    private void TryMergeHorizontal(int row, int column, int direction)
    {
        if ((cellViews[row, column].cellViewModel.Num.Value == cellViews[row, column - direction].cellViewModel.Num.Value) &&
            !cellViews[row, column - direction].cellViewModel.alreadyMerged &&
            !cellViews[row, column].cellViewModel.alreadyMerged)
        {
            cellViews[row, column - direction].cellViewModel.alreadyMerged = true;
            scoreViewModel.UpdateScore(Convert.ToInt32(cellViews[row, column].cellViewModel.Num.Value));
            string newNum = (Convert.ToInt32(cellViews[row, column].cellViewModel.Num.Value) +
                            Convert.ToInt32(cellViews[row, column - direction].cellViewModel.Num.Value)).ToString();

            cellViews[row, column - direction].cellViewModel.ChangeElementColorNum(model.GetColorByNum(newNum), newNum);
            cellViews[row, column].cellViewModel.ChangeElementNumColorSO(model.EmptyElement);

            //CellAnimator.Instance.CellTransition(cellViews[row, column], cellViews[row-direction, column], true);

            cellViews[row, column - direction].cellViewModel.VisualUpdate();
            cellViews[row, column].cellViewModel.VisualUpdate();

            bool endGame = GameManager.Instance.IsWin(newNum);
            if (endGame)
            {
                return;
            }
        }
    }
}
