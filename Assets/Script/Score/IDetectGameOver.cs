using System;
using UnityEngine;

public interface IDetectGameOver
{
    public event Action OnGameOver;
    public event Action<int> OnCount;
    public TaskModel Description();

}
