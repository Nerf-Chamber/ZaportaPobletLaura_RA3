using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startPos, length;
    public GameObject player;
    public float parallaxEffect;

    public float smoothTime = 0.3f;
    private float velocity = 0f;

    private void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    private void FixedUpdate()
    {
        float targetX = startPos + player.transform.position.x * parallaxEffect;
        float smoothX = Mathf.SmoothDamp(transform.position.x, targetX, ref velocity, smoothTime);

        transform.position = new Vector3(smoothX, transform.position.y, transform.position.z);

        float movement = player.transform.position.x * (1 - parallaxEffect);

        if (movement > startPos + length)
            startPos += length;
        else if (movement < startPos - length)
            startPos -= length;
    }
}