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
    public CellView[,] cellViews { get; private set; }
    private GameActionsViewModel gameActions;

    private DeckModel model;

    private void Awake()
    {
        deckGridLayoutGroup = GetComponent<GridLayoutGroup>();

    }

    private void Start()
    {
        model = DeckModel.Instance;
        deckGridLayoutGroup.constraintCount = model.DeckSize;
        deckSize = model.DeckSize;
        deckElementsCount = deckSize * deckSize;
        cellViews = new CellView[deckSize, deckSize];
        GameInput.Instance.OnSwipeAction += OnSwipeAction;
        model.state.Subscribe(_ => OnStateChanged(_));
        GameManager.Instance.OnGamePaused += OnGamePaused;
        GameManager.Instance.OnGameUnpaused += OnGameUnpaused;


        gameActions = new GameActionsViewModel();
        gameActions.Init();

        GenerateStartDeck();
        GenerateStartingNumbers();
    }

    private void GenerateStartDeck()
    {
        for (int i = 0; i < deckElementsCount; i++)
        {
            CellView cellView = LoadDeckElement(gameObject.transform);
            cellView.BindViewModel(new CellViewModel());
            cellView.cellViewModel.ChangeElementNumColorSO(model.EmptyElement);
            cellView.cellViewModel.VisualUpdate();
            cellViews[i / deckSize, i % deckSize] = cellView;
        }
    }

    private void GenerateStartingNumbers()
    {
        GenerateNum();
        GenerateNum();
    }

    public void GenerateNum()
    {
        List<CellView> emptyElements = FindEmptyElements();
        CellView rndElement = emptyElements[UnityEngine.Random.Range(0, emptyElements.Count - 1)];
        string rndText = model.NumForRandomGenerate[UnityEngine.Random.Range(0, model.NumForRandomGenerate.Length)];
        rndElement.cellViewModel.ChangeElementColorNum(model.GetColorByNum(rndText), rndText);

        //CellAnimator.Instance.CellCreate(rndElement);
        rndElement.cellViewModel.VisualUpdate();
    }

    public CellView LoadDeckElement(Transform deckTransform)
    {
        GameObject objElement = Instantiate(model.Cell.gameObject, deckTransform, false);

        return objElement.GetComponent<CellView>();

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
        gameActions.MoveCells(swipe, cellViews);

        if (ChangesExist(deckElementsValuesBeforeSwipe))
        {
            GenerateNum();
        }
        else
        {
            GameManager.Instance.IsDefeat();
        }

        ResetAlreadyMerged();
    }

    private void ResetAlreadyMerged()
    {
        for (int r = 0; r < model.DeckSize; r++)
        {
            for (int c = 0; c < model.DeckSize; c++)
            {
                cellViews[r, c].cellViewModel.alreadyMerged = false;
            }
        }
    }

    private string[,] CopyValuesBeforeSwipe()
    {
        string[,] arrayValues = new string[deckSize, deckSize];
        for (int r = 0; r < deckSize; r++)
        {
            for (int c = 0; c < deckSize; c++)
            {
                arrayValues[r, c] = cellViews[r, c].cellViewModel.Num.Value;
            }
        }

        return arrayValues;
    }

    private List<CellView> FindEmptyElements()
    {
        List<CellView> emptyElements = new List<CellView>();
        foreach (CellView element in cellViews)
        {
            if (element.cellViewModel.Num.Value == model.EmptyElement.value)
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
                if (!(cellViews[r, c].cellViewModel.Num.Value == deckElementsValuesBeforeSwipe[r, c]))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
