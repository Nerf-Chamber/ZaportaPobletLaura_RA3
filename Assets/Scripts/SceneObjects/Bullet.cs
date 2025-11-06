using UnityEngine;

[RequireComponent(typeof(AnimationBehaviour))]

public class Bullet : MonoBehaviour, ISpawnable
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private AnimationBehaviour _animation;

    private Vector2 collisionTerrainDirection;

    public Spawner spawner { get; set; }

    private void Awake() => _animation = GetComponent<AnimationBehaviour>();
    private void Start() => getBulletDirection();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collisionTerrainDirection);
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

    private void getBulletDirection()
    {
        Quaternion rotation = transform.rotation;

        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;

        collisionTerrainDirection = new Vector2(-Mathf.Sin(angle), Mathf.Cos(angle));
        if (collisionTerrainDirection.x > 0) {_animation.FlipX(); }
    }
    private bool didCollideWithTerrainProperly()
    {
        float raycastDistance = 0.7f;
        return Physics2D.Raycast(transform.position, collisionTerrainDirection, raycastDistance, LayerMask.GetMask("Terrain"));
    }
}