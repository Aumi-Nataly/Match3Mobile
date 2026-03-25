using System;
using System.Diagnostics;
using UnityEngine;

public class MaxCountService : IDetectGameOver
{
    public event Action OnGameOver;
    public event Action<int> OnCount;
    private GridSystem _gridSystem;
    private int _maxCount = 100;
    private int _currentCount = 0;

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
                _currentCount += 10;
                break;
            case MatchType.Four:
                _currentCount += 20;
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
