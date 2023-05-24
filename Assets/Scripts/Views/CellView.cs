using System.Collections;
using System.Collections.Generic;
using UniRx;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deckElementNumTMP;
    [SerializeField] private Image deckElementColorImage;
    
    private CellViewModel cellViewModel;

    private void Awake()
    {
        cellViewModel = gameObject.GetComponent<CellViewModel>();
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
