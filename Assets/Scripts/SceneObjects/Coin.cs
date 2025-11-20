using System;
using UnityEngine;

[RequireComponent(typeof(AnimationBehaviour))]

public class Coin : CollectableObject, ICollectable
{
    private AnimationBehaviour _animation;
    private AudioClip clip;

    private Vector2 initialPosition;

    private void Awake()
    {
        firstGoUp = false;
        initialPosition = transform.position;
        _animation = GetComponent<AnimationBehaviour>();
        PauseMenu.OnRestartChosen += ReenableCoin;
    }

    public override void Collected()
    {
        isCollected = true;

        GetComponent<BoxCollider2D>().enabled = false;

        _animation.SetCollectedState(isCollected);

        if (AudioManager.Instance.clipList.TryGetValue(AudioClips.CoinSound, out clip))
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void ReenableCoin()
    {
        isCollected = false;
        _animation.SetCollectedState(isCollected);
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        transform.position = initialPosition;
    }
    private void CollectedAnimationEnded() { GetComponent<SpriteRenderer>().enabled = false; }
}