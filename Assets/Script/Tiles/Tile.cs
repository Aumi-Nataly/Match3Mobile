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


    private void EnsureComponent() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

            Debug.Log($"Компонент SpriteRenderer добавлен на {gameObject.name}");
        }
    }

    public void SetType(TileType newType, TileSpriteManager _tileSpriteManager)
    {
        EnsureComponent();
        Type = newType;
        Sprite newSprite = _tileSpriteManager.GetSprite(Type);
        spriteRenderer.sprite = newSprite;
    }
}
