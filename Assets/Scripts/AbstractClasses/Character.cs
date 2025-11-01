using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MoveBehaviour))]
[RequireComponent(typeof(AnimationBehaviour))]

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected MoveBehaviour _move;
    [SerializeField] protected AnimationBehaviour _animation;

    protected virtual void Awake()
    {
        _move = GetComponent<MoveBehaviour>();
        _animation = GetComponent<AnimationBehaviour>();
    }
}