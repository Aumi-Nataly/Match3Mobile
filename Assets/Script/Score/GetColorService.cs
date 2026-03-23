using System;
using System.Collections.Generic;
using UnityEngine;

public class GetColorService : IDetectGameOver
{
    public event Action OnGameOver;
    private GridSystem _gridSystem;
    private int _maxCount = 0;
    private Sprite _sprite;
    private TileType _tileType;

    public GetColorService(GridSystem gridSystem)
    {
        _maxCount = UnityEngine.Random.Range(20, 30);
        _gridSystem = gridSystem;
        SetSprite();
    }

    public TaskModel Description()
    {
        return new TaskModel
        {
            TaskName = "Набрать ячеек",
            TaskValue = _maxCount.ToString(),
            TaskSprite = _sprite
        };
    }

    private void SetSprite()
    { 
        var dic = _gridSystem.GetDictionarySprite();
         GetRandomSprite(dic);
    }


    public void GetRandomSprite(Dictionary<TileType, Sprite> spriteDict)
    {
        List<KeyValuePair<TileType, Sprite>> pairs = new List<KeyValuePair<TileType, Sprite>>(spriteDict);
        int randomIndex = UnityEngine.Random.Range(0, pairs.Count);
        _sprite = pairs[randomIndex].Value;
        _tileType = pairs[randomIndex].Key;

    }

}
