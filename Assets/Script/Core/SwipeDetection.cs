using UnityEngine;
using VContainer;

public class SwipeDetection : MonoBehaviour
{
    private InputService _inputService;
    private Vector2 startPosition;
    private Tile selectedTile;
    private bool isSwiping;

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

        Vector2Int direction = GetSwipeDirection(delta);

        if (direction != Vector2Int.zero)
        {
            Debug.Log("Swipe: " + direction);
        }

        selectedTile = null;
    }

    private Vector2Int GetSwipeDirection(Vector2 delta)
    {
        // длина вектора. Сделал ли пользователь свайп или просто слегка махнул 
        if (delta.magnitude < 50f)
            return Vector2Int.zero;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            return delta.x > 0 ? Vector2Int.right : Vector2Int.left;
        }
        else
        {
            return delta.y > 0 ? Vector2Int.up : Vector2Int.down;
        }
    }
}
