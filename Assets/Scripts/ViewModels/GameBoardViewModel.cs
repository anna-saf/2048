using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class GameBoardViewModel : MonoBehaviour
{

    private GridLayoutGroup deckGridLayoutGroup;
    private int deckElementsCount;
    private int deckSize;
    public ICellViewModel[,] cellViewModels { get; private set; }
    private IGameActionsViewModel gameActions;

    private DeckModel model;

    private void Awake()
    {
        deckGridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
        gameActions = GetComponent<IGameActionsViewModel>();
    }

    private void Start()
    {
        model = DeckModel.Instance;
        deckGridLayoutGroup.constraintCount = model.DeckSize;
        deckSize = model.DeckSize;
        deckElementsCount = deckSize * deckSize;
        cellViewModels = new ICellViewModel[deckSize, deckSize];
        GameInput.Instance.OnSwipeAction += OnSwipeAction;
        model.state.Subscribe(_ => OnStateChanged(_));
        GameManager.Instance.OnGamePaused += OnGamePaused;
        GameManager.Instance.OnGameUnpaused += OnGameUnpaused;

        GenerateStartDeck();
        GenerateStartingNumbers();
    }

    private void GenerateStartDeck()
    {
        for (int i = 0; i < deckElementsCount; i++)
        {
            ICellViewModel element = LoadDeckElement(gameObject.transform);
            element.ChangeElementNumColorSO(model.EmptyElement);
            cellViewModels[i / deckSize, i % deckSize] = element;
        }
    }

    private void GenerateStartingNumbers()
    {
        GenerateNum();
        GenerateNum();
    }

    public void GenerateNum()
    {
        List<ICellViewModel> emptyElements = FindEmptyElements();
        ICellViewModel rndElement = emptyElements[UnityEngine.Random.Range(0, emptyElements.Count - 1)];
        string rndText = model.NumForRandomGenerate[UnityEngine.Random.Range(0, model.NumForRandomGenerate.Length)];
        rndElement.ChangeElementColorNum(model.GetColorByNum(rndText), rndText);
    }

    public ICellViewModel LoadDeckElement(Transform deckTransform)
    {
        GameObject objElement = UnityEngine.Object.Instantiate(model.Cell, deckTransform, false);

        return objElement.GetComponent<ICellViewModel>();

    }

    private void OnGameUnpaused(object sender, EventArgs e)
    {
        GameInput.Instance.OnSwipeAction += OnSwipeAction;
    }

    private void OnGamePaused(object sender, EventArgs e)
    {
        GameInput.Instance.OnSwipeAction -= OnSwipeAction;
    }

    private void OnStateChanged(GameStateEnum state)
    {
        if (GameManager.Instance.IsGameStateEnd())
        {
            GameInput.Instance.OnSwipeAction -= OnSwipeAction;
        }
    }

    private void OnSwipeAction(object sender, SwipeData swipe)
    {
        string[,] deckElementsValuesBeforeSwipe = CopyValuesBeforeSwipe();
        gameActions.MoveCells(swipe, cellViewModels);

        if (ChangesExist(deckElementsValuesBeforeSwipe))
        {
            GenerateNum();
        }
        else
        {
            GameManager.Instance.IsDefeat();
        }
    }

    private string[,] CopyValuesBeforeSwipe()
    {
        string[,] arrayValues = new string[deckSize, deckSize];
        for (int r = 0; r < deckSize; r++)
        {
            for (int c = 0; c < deckSize; c++)
            {
                arrayValues[r, c] = cellViewModels[r, c].Num.Value;
            }
        }

        return arrayValues;
    }

    private List<ICellViewModel> FindEmptyElements()
    {
        List<ICellViewModel> emptyElements = new List<ICellViewModel>();
        foreach (ICellViewModel element in cellViewModels)
        {
            if (element.Num.Value == model.EmptyElement.value)
                emptyElements.Add(element);
        }
        return emptyElements;
    }

    private bool ChangesExist(string[,] deckElementsValuesBeforeSwipe)
    {
        for (int r = 0; r < deckSize; r++)
        {
            for (int c = 0; c < deckSize; c++)
            {
                if (!(cellViewModels[r, c].Num.Value == deckElementsValuesBeforeSwipe[r, c]))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
