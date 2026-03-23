using System;
using UnityEngine;
using VContainer;

public class SwipeDetection : MonoBehaviour
{
    private InputService _inputService;
    private Vector2 startPosition;
    private Tile selectedTile;
    private bool isSwiping;

    public event Action<SwipeModel> OnSwipe;

    [Inject]
    public void Construct(InputService inputService)
    {
        _inputService = inputService;
    }

    private void Update()
    {
        if (_inputService.IsPressed && !isSwiping)
        {
            StartSwipe();
        }

        if (!_inputService.IsPressed && isSwiping)
        {
            EndSwipe();
        }
    }

    private void StartSwipe()
    {
        isSwiping = true;
        startPosition = _inputService.TouchPosition;

        //Поиск коллайдера на момент касания.точечная проверка
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(startPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null)
        {
            selectedTile = hit.collider.GetComponent<Tile>();
        }
    }

    private void EndSwipe()
    {
        isSwiping = false;

        if (selectedTile == null)
            return;

        Vector2 endPosition = _inputService.TouchPosition;
        Vector2 delta = endPosition - startPosition;

        Vector2 direction = GetSwipeDirection(delta);

        if (direction != Vector2.zero)
        {
            OnSwipe?.Invoke(new SwipeModel {Tile = selectedTile, Vect2 = direction });
        }

        selectedTile = null;
    }

    private Vector2 GetSwipeDirection(Vector2 delta)
    {
        // длина вектора. Сделал ли пользователь свайп или просто слегка махнул 
        if (delta.magnitude < 50f)
            return Vector2.zero;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            return delta.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            return delta.y > 0 ? Vector2.up : Vector2.down;
        }
    }
}
