using UnityEngine;

public abstract class CollectableObject : MonoBehaviour
{
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 1f;

    protected Vector3 startPos;
    private bool collected = false;

    protected void FloatUpDown()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
    void Update()
    {
        if (!collected)
        {
            FloatUpDown();
        }
    }
}