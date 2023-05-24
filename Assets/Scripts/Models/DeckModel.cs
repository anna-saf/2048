using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static GameModel;

public class DeckModel : MonoBehaviour
{
    [SerializeField] private NumColorSO[] numColorSOArray;
    [SerializeField] private GameObject cell;
    [SerializeField] private string[] numForRandomGenerate;
    [SerializeField] private int deckSize;
    [SerializeField] private NumColorSO emptyElement;

    public NumColorSO[] NumColorSOArray { get { return numColorSOArray; } }
    public GameObject Cell { get { return cell; } }
    public string[] NumForRandomGenerate { get { return numForRandomGenerate; } }
    public int DeckSize { get { return deckSize; } }
    public NumColorSO EmptyElement { get { return emptyElement; } }

    public ReactiveProperty<GameStateEnum> state { get; private set; } = new ReactiveProperty<GameStateEnum> { };

    public ReactiveProperty<int> score { get; private set; } = new ReactiveProperty<int>();

    public static DeckModel Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        state = GameModel.Instance.state;
        score = GameModel.Instance.score;
    }
    public Color GetColorByNum(string num)
    {
        foreach (NumColorSO numColorSO in NumColorSOArray)
        {
            if (numColorSO.value == num)
            {
                return numColorSO.color;
            }
        }

        return default;
    }
}
