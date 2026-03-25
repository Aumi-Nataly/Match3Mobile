using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileSpriteManager : MonoBehaviour
{
    [SerializeField]
    private Sprite[] SpriteMassive;

    [SerializeField]
    private Sprite SpriteBomb;

    private Dictionary<TileType, Sprite> spriteCache = new Dictionary<TileType, Sprite>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < SpriteMassive.Length; i++)
        {
            if (SpriteMassive[i] != null)
            {
                TileType type = (TileType)i;
                if (type == TileType.Nothing)
                    return;

                spriteCache[(TileType)i] = SpriteMassive[i]; 
            }
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

    public Dictionary<TileType, Sprite> GetDictionaryType()
        => spriteCache;


    public Sprite GetSpriteBomb()
    {
     return  SpriteBomb;
    }
}
