using UniRx;
using UnityEngine;

public class GameModel : MonoBehaviour
{
    [SerializeField] private NumColorSO winElement;      

    public ReactiveProperty<GameStateEnum> state { get; private set; } = new ReactiveProperty<GameStateEnum>();

    public NumColorSO WinElement { get { return winElement; } }

    public ReactiveProperty<int> score { get; private set; } = new ReactiveProperty<int>();

    public static GameModel Instance;
    private void Awake()
    {
        Instance = this;
    }
}

public enum GameStateEnum
{
    GamePlaying,
    GameEnd,
}
