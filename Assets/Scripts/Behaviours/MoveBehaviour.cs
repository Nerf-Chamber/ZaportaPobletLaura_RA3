using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    public void MoveX(float directionX, float speed)
    {
        _rb.linearVelocity = new Vector2(directionX * speed, _rb.linearVelocity.y);
    }
    public void MoveY(float directionY, float speed)
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, directionY * speed);
    }
    public void Move(Vector2 direction, float speed)
    {
        _rb.linearVelocity = direction * speed;
    }
}