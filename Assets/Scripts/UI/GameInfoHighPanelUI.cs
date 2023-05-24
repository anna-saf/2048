using UniRx;
using TMPro;
using UnityEngine;

public class GameInfoHighPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTMP;
    [SerializeField] private TextMeshProUGUI recordTMP;

    private void Start()
    {
        scoreTMP.text = "0";
        recordTMP.text = GlobalModel.Instance.record.Value.ToString();
        GameModel.Instance.score.Subscribe(_ => OnChangeScore(_));
    }

    private void OnChangeScore(int score)
    {
        ChangeScoreText(score);
    }

    private void ChangeScoreText(int score)
    {
        scoreTMP.text = score.ToString();
    }
}
