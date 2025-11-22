using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MoveBehaviour))]
[RequireComponent(typeof(AnimationBehaviour))]
[RequireComponent(typeof(JumpBehaviour))]

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected MoveBehaviour _move;
    [SerializeField] protected AnimationBehaviour _animation;
    [SerializeField] protected JumpBehaviour _jump;

    protected bool isFacingRight;
    protected bool isMovingX;
    protected Vector2 onMoveDirection;

    public float speed;

    protected virtual void Awake()
    {
        _move = GetComponent<MoveBehaviour>();
        _animation = GetComponent<AnimationBehaviour>();
        _jump = GetComponent<JumpBehaviour>();
    }

    protected bool GetIsGrounded(float raycastDistance)
    {
        bool hitFloor = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, LayerMask.GetMask("Terrain"));
        bool hitCeiling = Physics2D.Raycast(transform.position, Vector2.up, raycastDistance, LayerMask.GetMask("Terrain"));

        return hitFloor || hitCeiling;
    }
    protected bool GetDidHitHorizontal(float raycastDistance, bool rightCheck)
    {
        return rightCheck 
            ? Physics2D.Raycast(transform.position, Vector2.right, raycastDistance, LayerMask.GetMask("Terrain")) 
            : Physics2D.Raycast(transform.position, Vector2.left, raycastDistance, LayerMask.GetMask("Terrain"));
    }
}