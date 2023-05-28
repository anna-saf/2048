using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScoreViewModel
{
    public void UpdateScore(int scoreIncrease)
    {
        DeckModel.Instance.score.Value += scoreIncrease;
    }
}
