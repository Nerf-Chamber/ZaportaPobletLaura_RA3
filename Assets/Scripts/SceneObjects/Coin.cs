using UnityEngine;

[RequireComponent(typeof(AnimationBehaviour))]

public class Coin : CollectableObject, ICollectable
{
    private AnimationBehaviour _animation;
    private AudioClip clip;

    private void Awake()
    {
        firstGoUp = false;
        _animation = GetComponent<AnimationBehaviour>();
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

    public void CollectedAnimationEnded() { Destroy(gameObject); }
}