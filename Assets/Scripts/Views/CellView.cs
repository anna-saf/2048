using UniRx;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deckElementNumTMP;
    [SerializeField] private Image deckElementColorImage;

    public CellViewModel cellViewModel;


    public void BindViewModel(CellViewModel viewModel)
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
