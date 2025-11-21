using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AnimationBehaviour))]

public class Coin : CollectableObject, ICollectable
{
    private AnimationBehaviour _animation;
    private AudioSource audioSource;
    private AudioClip clip;

    private Vector2 initialPosition;

    private void Awake()
    {
        firstGoUp = false;
        initialPosition = transform.position;
        _animation = GetComponent<AnimationBehaviour>();
        audioSource = GetComponent<AudioSource>();
        BaseMenu.OnRestartChosen += ReenableCoin;
    }

    public override void Collected()
    {
        isCollected = true;
        GetComponent<BoxCollider2D>().enabled = false;
        _animation.SetCollectedState(isCollected);
        AudioManager.PlaySound(audioSource, clip, AudioClips.CoinSound);
    }

    public void ReenableCoin()
    {
        if (isCollected)
        {
            isCollected = false;
            _animation.SetCollectedState(isCollected);
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            transform.position = initialPosition;
        }
    }
    private void CollectedAnimationEnded() { GetComponent<SpriteRenderer>().enabled = false; }
}