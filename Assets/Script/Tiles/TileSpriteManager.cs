using System.Collections.Generic;
using UnityEngine;

public class TileSpriteManager : MonoBehaviour
{
    [SerializeField]
    private Sprite[] SpriteMassive;

    private Dictionary<TileType, Sprite> spriteCache = new Dictionary<TileType, Sprite>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < SpriteMassive.Length; i++)
        {
            if (SpriteMassive[i] != null)
                spriteCache[(TileType)i] = SpriteMassive[i];
        }
    }

    public Sprite GetSprite(TileType type)
    {
        if (spriteCache.TryGetValue(type, out Sprite sprite))
        {
            return sprite;
        }
        return null;
    }
}
