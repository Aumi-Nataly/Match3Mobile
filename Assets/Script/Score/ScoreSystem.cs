using TMPro;
using UnityEngine;
using VContainer;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;

    private GridSystem _gridSystem;
    private int Score = 0;

    [Inject]
    public void Construct(GridSystem grid)
    {
        _gridSystem = grid;
    }
    private void OnDestroy()
    {
        _gridSystem.OnScoreAdded -= AddScore;
    }
    void Start()
    {
        _gridSystem.OnScoreAdded += AddScore;
    }

     

    private void AddScore(MatchType matchType)
    {
        switch (matchType)
        {
            case MatchType.Three:
                Score += 10;
                break;
            case MatchType.Four:
                Score += 20;
                break;
            case MatchType.Five:
                Score += 50;
                break;
            case MatchType.LForm:
                Score += 100;
                break;
            case MatchType.TForm:
                Score += 300;
                break;

        }

        scoreText.text = Score.ToString();
    }

}
