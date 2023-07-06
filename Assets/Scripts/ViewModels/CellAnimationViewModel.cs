using System;
using UniRx;
using UnityEngine;

public class CellAnimationViewModel : BaseCellViewModel
{
    public ReactiveProperty<RectTransform> cellTransform = new ReactiveProperty<RectTransform>();
    public ReactiveProperty<bool> isDestroy = new ReactiveProperty<bool>();

    private float moveTime;
    private float sizeChangeTime;


    public event EventHandler<OnCellMoveEventArgs> OnCellMove;
    public class OnCellMoveEventArgs : EventArgs
    {
        public CellView startCell;
        public CellView endCell;
        public bool IsMerging;
        public float sizeChangeTime;
        public float moveTime;
    }

    public event EventHandler<OnCellGenerateEventArgs> OnCellGenerate;
    public class OnCellGenerateEventArgs : EventArgs
    {
        public float moveTime;
        public CellView copiedCell;
    }


    public void Init(RectTransform transform)
    {
        cellTransform.Value = transform;
        isDestroy.Value = false;
        moveTime = CellAnimationModel.Instance.MoveTime;
        sizeChangeTime = CellAnimationModel.Instance.SizeChangeTime;
        isDestroy.Value = false;
    }

    public void Move(CellView startCell, CellView endCell, bool IsMerging)
    {
        RectTransform rectStartCell = startCell.GetComponent<RectTransform>();
        ChangeElementColorNum(DeckModel.Instance.GetColorByNum(startCell.cellViewModel.Num.Value), startCell.cellViewModel.Num.Value);
        VisualUpdate();
        cellTransform.Value.position = startCell.transform.position;
        cellTransform.Value.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectStartCell.rect.width);
        cellTransform.Value.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectStartCell.rect.height);
        OnCellMove?.Invoke(this, new OnCellMoveEventArgs
        {
            startCell = startCell,
            endCell = endCell,
            IsMerging = IsMerging,
            sizeChangeTime = sizeChangeTime,
            moveTime = moveTime
        });
    }

    public void CellGenerate(CellView cell)
    {
        RectTransform rectCopiedCell = cell.GetComponent<RectTransform>();
        ChangeElementColorNum(DeckModel.Instance.GetColorByNum(cell.cellViewModel.num), cell.cellViewModel.num);
        VisualUpdate();
        cellTransform.Value.position = rectCopiedCell.position;
        cellTransform.Value.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectCopiedCell.rect.width);
        cellTransform.Value.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectCopiedCell.rect.height);
        OnCellGenerate?.Invoke(this, new OnCellGenerateEventArgs
        {
            moveTime = moveTime,
            copiedCell = cell
        });        
    }
}
