using UnityEngine;

[RequireComponent(typeof(AnimationBehaviour))]

public class Bullet : MonoBehaviour, ISpawnable
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private AnimationBehaviour _animation;

    private Vector2 collisionTerrainDirection;

    public Spawner spawner { get; set; }

    private void Awake() => _animation = GetComponent<AnimationBehaviour>();
    private void Start() => GetBulletDirection();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) { _animation.SetCollisionPlayerState(true); }
        else if (DidCollideWithTerrainProperly()) { _animation.SetCollisionTerrainState(true); }
    }

    private void GetBulletDirection()
    {
        Quaternion rotation = transform.rotation;

        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;

        collisionTerrainDirection = new Vector2(-Mathf.Sin(angle), Mathf.Cos(angle));
        if (collisionTerrainDirection.x > 0) {_animation.FlipX(); }
    }
    private bool DidCollideWithTerrainProperly()
    {
        float raycastDistance = 0.7f;
        return Physics2D.Raycast(transform.position, collisionTerrainDirection, raycastDistance, LayerMask.GetMask("Terrain"));
    }
    private void WhenExplosionAnimationStarts() { GetComponent<BoxCollider2D>().enabled = false; }
    private void WhenBulletAnimationStarts() { GetComponent<BoxCollider2D>().enabled = true; }
    private void WhenExplosionAnimationEnds()
    {
        _animation.SetCollisionTerrainState(false);
        _animation.SetCollisionPlayerState(false);
        spawner.Push(gameObject);
    }
}