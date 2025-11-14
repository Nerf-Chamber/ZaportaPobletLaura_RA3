using UnityEngine;

public abstract class CollectableObject : MonoBehaviour
{
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 1f;

    protected Vector3 startPos;
    protected bool firstGoUp;
    protected bool isCollected = false;

    private void Start()
    {
        startPos = transform.position;
    }
    protected void FloatUpDown(bool firstGoUp)
    {
        float newY = firstGoUp 
            ? startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude 
            : startPos.y - Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
    public virtual void Collected() { }
    void Update()
    {
        if (!isCollected)
        {
            FloatUpDown(firstGoUp);
        }
    }
}