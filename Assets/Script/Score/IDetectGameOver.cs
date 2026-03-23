using System;
using UnityEngine;

public interface IDetectGameOver
{
    public event Action OnGameOver;
    public TaskModel Description();

}
