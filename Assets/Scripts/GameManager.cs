using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameBoardViewModel deck;

    private ReactiveProperty<int> record; 

    private ReactiveProperty<int> score;

    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        record = GlobalModel.Instance.record;
        GameModel.Instance.state.Value = GameStateEnum.GamePlaying;
        score = GameModel.Instance.score;
    }

    public bool IsGameStateEnd()
    {
        return GameModel.Instance.state.Value == GameStateEnum.GameEnd;
    }

    public bool IsWin(string progress)
    {
        if (progress == GameModel.Instance.WinElement.value)
        {
            GameEnd();
            return true;
        }
        return false;
    }

    public bool IsDefeat()
    {
        for (int r = 0; r < DeckModel.Instance.DeckSize; r++)
        {
            for (int c = 0; c < DeckModel.Instance.DeckSize; c++)
            {
                if (deck.cellViewModels[r, c].Num.Value == DeckModel.Instance.EmptyElement.value)
                {
                    return false;
                }
            }

        }
        GameEnd();
        return true;
    }

    private void GameEnd()
    {
        GameModel.Instance.state.Value = GameStateEnum.GameEnd;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
        CheckRecord();
    }

    private void CheckRecord()
    {
        if (score.Value > record.Value)
        {
            SetRecord();
        }
    }

    private void SetRecord()
    {
        record.Value = score.Value;
    }

    public void GameOnPause()
    {
        OnGamePaused?.Invoke(this, EventArgs.Empty);        
    }

    public void GameUnpaused()
    {
        OnGameUnpaused?.Invoke(this, EventArgs.Empty);
    }
}
