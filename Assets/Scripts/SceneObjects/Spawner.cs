using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float timeToWait = 0;
    private float timeBetweenObjects = 3f;

    [SerializeField] private GameObject gameObject;
    [SerializeField] private Vector2 objectDirection;
    [SerializeField] private float objectSpeed;

    public Stack<GameObject> objectStack = new Stack<GameObject>();

    void Update()
    {
        if (Time.time >= timeToWait)
        {
            if (objectStack.Count == 0)
            {
                InstantiateObject();
            }
            else
            {
                Pop();
            }
            timeToWait = Time.time + timeBetweenObjects;
        }
    }
    public void Push(GameObject go)
    {
        objectStack.Push(go);
        go.SetActive(false);
    }
    public GameObject Pop()
    {
        GameObject go = objectStack.Pop();
        go.SetActive(true);
        go.transform.position = transform.position;
        go.GetComponent<Rigidbody2D>().linearVelocity = objectDirection * objectSpeed;
        return go;
    }
    public void InstantiateObject()
    {
        ISpawnable spawnable = gameObject.GetComponent<ISpawnable>();

        if (spawnable != null) 
        {
            GameObject go = Instantiate(gameObject, transform.position, transform.parent.rotation);
            go.GetComponent<Rigidbody2D>().linearVelocity = objectDirection * objectSpeed;
            (go.GetComponent<ISpawnable>()).spawner = this;
        }
    }
}