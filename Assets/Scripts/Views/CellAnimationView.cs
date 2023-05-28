using UniRx;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellAnimationView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deckElementNumTMP;
    [SerializeField] private Image deckElementColorImage;

    public CellAnimationViewModel cellViewModel;


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
            transform.position = _.position;
        });
        cellViewModel.isDestroy.Subscribe(_ =>
        {
            if (_)
            {
                Destroy(gameObject);
            }
        });
    }
}
