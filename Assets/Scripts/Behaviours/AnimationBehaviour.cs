using UnityEngine;

public class AnimationBehaviour : MonoBehaviour
{
    private Animator _an;

    private void Awake()
    {
        _an = GetComponent<Animator>();
    }

    public void FlipX(ref bool isFacingRight)
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    public void FlipX()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    public void FlipY()
    {
        Vector3 localScale = transform.localScale;
        localScale.y *= -1;
        transform.localScale = localScale;
    }
    public void SetRunState(bool isRunning)
    {
        _an.SetBool("isRunning", isRunning);
    }
    public void SetDeathState(bool isDead)
    {
        _an.SetBool("isDead", isDead);
    }
}