using System;


public class MaxCountService : IDetectGameOver
{
    public event Action OnGameOver;

    private GridSystem _gridSystem;
    private int _maxCount;
    private int _currentCount;

    public MaxCountService(GridSystem gridSystem)
    {
        _maxCount = UnityEngine.Random.Range(100, 200) * 10;
        _gridSystem = gridSystem;

        _gridSystem.OnScoreAdded += AddScore;
    }

    private void AddScore(MatchType matchType)
    {
        switch (matchType)
        {
            case MatchType.Three:
                _maxCount += 10;
                break;
            case MatchType.Four:
                _maxCount += 20;
                break;
            case MatchType.Five:
                _currentCount += 50;
                break;
            case MatchType.LForm:
                _currentCount += 100;
                break;
            case MatchType.TForm:
                _currentCount += 300;
                break;
        }

        if (_currentCount >= _maxCount)
        {
            OnGameOver?.Invoke();
            _gridSystem.OnScoreAdded -= AddScore;
        }
     
    }

    public TaskModel Description()
    {
        return new TaskModel { 
            TaskName = "Набрать указанное число очков",
            TaskValue = _maxCount.ToString()
        };
    }
}
