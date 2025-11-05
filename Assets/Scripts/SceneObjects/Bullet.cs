using UnityEngine;

public class Bullet : MonoBehaviour, ISpawnable
{
    [SerializeField] private Rigidbody2D _rb;
    private Vector2 collisionTerrainDirection;

    public Spawner spawner { get; set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Green explosion
            spawner.Push(gameObject);
        }
        else if (didCollideWithTerrainProperly())
        {
            // Red explosion
            spawner.Push(gameObject);
        }
    }
    private bool didCollideWithTerrainProperly()
    {
        float raycastDistance = 0.7f;
        return Physics2D.Raycast(transform.position, collisionTerrainDirection, raycastDistance, LayerMask.GetMask("Terrain"));
    }
}