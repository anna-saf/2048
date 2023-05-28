using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEditor.XR;
using UnityEngine;

public class CellAnimationViewModel : BaseCellViewModel
{
    public ReactiveProperty<Transform> cellTransform = new ReactiveProperty<Transform>();
    public ReactiveProperty<bool> isDestroy = new ReactiveProperty<bool>();
    private Sequence sequence;
    private float moveTime;
    private float sizeChangeTime;


    public void Init(Transform transform)
    {
        cellTransform.Value = transform;
        isDestroy.Value = false;
        moveTime = DeckModel.Instance.MoveTime;
        sizeChangeTime = DeckModel.Instance.SizeChangeTime;
        isDestroy.Value = false;
    }

    public void Move(CellView startCell, CellView endCell, bool IsMerging)
    {
        ChangeElementColorNum(DeckModel.Instance.GetColorByNum(startCell.cellViewModel.Num.Value), startCell.cellViewModel.Num.Value);
        VisualUpdate();
        cellTransform.Value.position = startCell.transform.position;

        sequence = DOTween.Sequence();
        sequence.Append(cellTransform.Value.DOMove(endCell.transform.position, moveTime).SetEase(Ease.InOutQuad));
        if (IsMerging)
        {
            sequence.AppendCallback(() =>
            {
                ChangeElementColorNum(DeckModel.Instance.GetColorByNum(endCell.cellViewModel.num), endCell.cellViewModel.num);
                VisualUpdate();
            });
            sequence.Append(cellTransform.Value.DOScale(1.2f, sizeChangeTime)); 
            sequence.Append(cellTransform.Value.DOScale(1f, sizeChangeTime));
        }

        sequence.AppendCallback(() =>
        {
            endCell.cellViewModel.VisualUpdate();
            Destroy();
        });
    }

    public void CellGenerate(CellView cell)
    {
        //Перенести анимации во View
        ChangeElementColorNum(DeckModel.Instance.GetColorByNum(cell.cellViewModel.num), cell.cellViewModel.num);
        VisualUpdate();
        cellTransform.Value.transform.position = cell.transform.position;
        cellTransform.Value.transform.localScale = Vector2.zero;

        sequence = DOTween.Sequence();
        sequence.Append(cellTransform.Value.transform.DOScale(1.2f, sizeChangeTime));
        sequence.Append(cellTransform.Value.transform.DOScale(1f, sizeChangeTime));
        sequence.AppendCallback(() =>
        {
            cell.cellViewModel.VisualUpdate();
            //Destroy();
        });
    }

    public void Destroy()
    {
        sequence.Kill();
        isDestroy.Value = true;
    }
}
