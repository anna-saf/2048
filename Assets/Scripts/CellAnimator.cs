using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellAnimator : MonoBehaviour
{
    public static CellAnimator Instance { get; private set; }
    //private DeckModel model;
    private Transform spawnTransform;

    private void Awake()
    {
        spawnTransform = transform;
        if (Instance == null)
        {
            Instance = this;
        }
        DOTween.Init();
    }

    /*public CellAnimator(Transform spawnTransform)
    {
        this.spawnTransform = spawnTransform;
        if (Instance != null)
        {
            Instance = this;
        }
        DOTween.Init();
    }*/

    public void CellTransition(CellView startCell, CellView endCell, bool IsMerging)
    {
        CreateAnimationCell().Move(startCell, endCell, IsMerging);
    }

    public void CellCreate(CellView cell)
    {
        CreateAnimationCell().CellGenerate(cell);
    }

    private CellAnimationViewModel CreateAnimationCell()
    {
        CellAnimationView cellView = LoadCellAnimationPrefab();
        CellAnimationViewModel cellAnimationViewModel = BindCellAnimationViewModel(cellView);
        return cellAnimationViewModel;
    }

    //Заменить на фабрику
    private CellAnimationView LoadCellAnimationPrefab()
    {
        GameObject objElement = Instantiate(DeckModel.Instance.CellAnimation.gameObject, spawnTransform, false);
        return objElement.GetComponent<CellAnimationView>();
    }

    private CellAnimationViewModel BindCellAnimationViewModel(CellAnimationView cellView)
    {
        CellAnimationViewModel cellAnimationViewModel = new CellAnimationViewModel();
        cellAnimationViewModel.Init(cellView.CellAnimationRectTransform);
        cellView.BindViewModel(cellAnimationViewModel); 
        return cellView.cellViewModel;
    }
}
