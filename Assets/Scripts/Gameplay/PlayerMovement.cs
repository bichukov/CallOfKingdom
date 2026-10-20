using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody2D _rb;
    private Vector2 _moveDirection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            _moveDirection = Vector2.zero;
            return;
        }

        var keyboard = Keyboard.current;

        float h =
            (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed ? 1f : 0f)
            - (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed ? 1f : 0f);

        float v =
            (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed ? 1f : 0f)
            - (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed ? 1f : 0f);

        _moveDirection = new Vector2(h, v).normalized;
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(
            _rb.position + _moveDirection * speed * Time.fixedDeltaTime
        );
    }
}
