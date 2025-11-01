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
        _rb.linearVelocity = new Vector2(directionX * speed, _rb.linearVelocityY);
    }
}