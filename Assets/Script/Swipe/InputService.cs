using UnityEngine;

public class InputService : MonoBehaviour
{
    private MobileInputAction input;

    public Vector2 TouchPosition => input.Player.Position.ReadValue<Vector2>();

    public Vector2 TouchDelta => input.Player.Swipe.ReadValue<Vector2>();

    public bool IsPressed => input.Player.Press.IsPressed();

    private void Awake()
    {
        input = new MobileInputAction();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
