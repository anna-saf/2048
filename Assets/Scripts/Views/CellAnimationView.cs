using UniRx;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CellAnimationView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deckElementNumTMP;
    [SerializeField] private Image deckElementColorImage;
    [SerializeField] private Transform cellAnimationObject;
    [SerializeField] private RectTransform cellAnimationRectTransform;

    public RectTransform CellAnimationRectTransform { get { return cellAnimationRectTransform; } }
    public CellAnimationViewModel cellViewModel;
    private Sequence sequence;

    

    private void Awake()
    {
        sequence = DOTween.Sequence();
    }

    public void BindViewModel(CellAnimationViewModel viewModel)
    {
        cellViewModel = viewModel;
        SubscribeOnChanges();
    }

    protected void SubscribeOnChanges()
    {
        cellViewModel.Num.Subscribe(_ =>
        {
            deckElementNumTMP.text = _;
        });

        cellViewModel.Color.Subscribe(_ =>
        {
            deckElementColorImage.color = _;
        });
        cellViewModel.cellTransform.Subscribe(_ =>
        {
            cellAnimationRectTransform.position = _.position;
            cellAnimationRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _.rect.width);
            cellAnimationRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _.rect.height);
        });

        cellViewModel.OnCellGenerate += CellViewModel_OnCellGenerate;
        cellViewModel.OnCellMove += CellViewModel_OnCellMove;
    }

    private void CellViewModel_OnCellMove(object sender, CellAnimationViewModel.OnCellMoveEventArgs e)
    {
        CellMove(e.startCell, e.endCell, e.IsMerging, e.sizeChangeTime, e.moveTime);
    }

    private void CellViewModel_OnCellGenerate(object sender, CellAnimationViewModel.OnCellGenerateEventArgs e)
    {
        CellGenerateAnimation(e.moveTime, e.copiedCell);
    }

    private void CellGenerateAnimation(float sizeChangeTime, CellView copiedCell)
    {
        cellAnimationObject.localScale = Vector3.zero;
        sequence = DOTween.Sequence();
        sequence.Append(cellAnimationObject.transform.DOScale(1.2f, CellAnimationModel.Instance.SizeChangeTime));
        sequence.Append(cellAnimationObject.transform.DOScale(1f, CellAnimationModel.Instance.SizeChangeTime));
        sequence.AppendCallback(() =>
        {
            copiedCell.cellViewModel.VisualUpdate();
            Destroy();
        });
    }

    private void CellMove(CellView startCell, CellView endCell, bool IsMerging, float sizeChangeTime, float moveTime)
    {
        startCell.cellViewModel.VisualUpdate();
        sequence.Append(transform.DOMove(endCell.transform.position, moveTime).SetEase(Ease.InOutQuad));
        if (IsMerging)
        {
            sequence.AppendCallback(() =>
            {
                cellViewModel.ChangeElementColorNum(DeckModel.Instance.GetColorByNum(endCell.cellViewModel.num), endCell.cellViewModel.num);
                cellViewModel.VisualUpdate();
            });
            sequence.Append(transform.DOScale(1.2f, sizeChangeTime));
            sequence.Append(transform.DOScale(1f, sizeChangeTime));
        }

        sequence.AppendCallback(() =>
        {
            endCell.cellViewModel.VisualUpdate();
            Destroy();
        });
    }

    private void Destroy()
    {
        sequence.Kill();
        Destroy(gameObject);
    }
}
