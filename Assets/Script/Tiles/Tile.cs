using UnityEngine;
using VContainer;

public class Tile : MonoBehaviour
{
    public Vector2 GridPos;
    public TileType Type;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetType(TileType newType, TileSpriteManager _tileSpriteManager)
    {
        Type = newType;
        Sprite newSprite = _tileSpriteManager.GetSprite(Type);
        spriteRenderer.sprite = newSprite;
    }
}
