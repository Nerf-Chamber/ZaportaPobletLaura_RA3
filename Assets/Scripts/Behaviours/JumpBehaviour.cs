using UnityEngine;

public class JumpBehaviour : MonoBehaviour
{
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    public void InvertGravity()
    {
        _rb.gravityScale *= -1;
    }
    public void Jump(float jumpPower)
    {
        _rb.AddForce(Vector2.up * jumpPower);
    }
}