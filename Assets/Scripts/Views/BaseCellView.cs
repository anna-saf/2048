using UniRx;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseCellView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deckElementNumTMP;
    [SerializeField] private Image deckElementColorImage;

    public BaseCellViewModel cellViewModel;


    public virtual void BindViewModel(BaseCellViewModel viewModel)
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
    }
}
