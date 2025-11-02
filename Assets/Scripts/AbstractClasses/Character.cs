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

    protected virtual void Awake()
    {
        _move = GetComponent<MoveBehaviour>();
        _animation = GetComponent<AnimationBehaviour>();
        _jump = GetComponent<JumpBehaviour>();
    }
}