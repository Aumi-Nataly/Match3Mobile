using DG.Tweening;
using UnityEngine;
using VContainer;

public class Tile : MonoBehaviour
{
    public TileType Type;
    public TileKind TileKind;
    public SpriteRenderer spriteRenderer;

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
        }
    }

    public void SetType(TileSpriteManager _tileSpriteManager)
    {
        EnsureComponent();
        TileType newType = (TileType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(TileType)).Length - 1);

        Type = newType;
        TileKind = TileKind.Normal;
        Sprite newSprite = _tileSpriteManager.GetSprite(Type);
        spriteRenderer.sprite = newSprite;
    }

    public void MoveTo(Vector3 tatgetPos, float duration = 0.1f)
    {
        transform.DOLocalMove(tatgetPos, duration).SetEase(Ease.Linear);
    }

    public void SetKind(TileKind kind)
    {
        TileKind = kind;
    }

    public void SetColorBomb(TileSpriteManager _tileSpriteManager)
    {
        spriteRenderer.sprite = _tileSpriteManager.GetSpriteBomb();
        Type = TileType.Nothing;
    }

  
}
