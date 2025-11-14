using UnityEngine;

public class Coin : CollectableObject, ICollectable
{
    private void Awake()
    {
        firstGoUp = false;
    }

    public override void Collected()
    {
        // animaició
        Destroy(gameObject);
    }
}