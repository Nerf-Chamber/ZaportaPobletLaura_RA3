using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AnimationBehaviour))]

public class Apple : CollectableObject
{
    private AnimationBehaviour _animation;

    public static event Action OnAppleFaceEnded;

    private void Awake()
    {
        firstGoUp = true;

        _animation = GetComponent<AnimationBehaviour>();

        Dialogue.OnEndDialogueEnded += AppleFace;
        BaseMenu.OnRestartChosen += RestartAppleFace;
    }

    private void AppleFace()
    {
        _animation.SetAppleState(true);
        StartCoroutine(Delay(3f));
    }
    private void RestartAppleFace() => _animation.SetAppleState(false);

    private IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        OnAppleFaceEnded.Invoke();
    }
}