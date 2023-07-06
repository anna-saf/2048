
public class GameScoreViewModel
{
    public void UpdateScore(int scoreIncrease)
    {
        DeckModel.Instance.score.Value += scoreIncrease;
    }
}
