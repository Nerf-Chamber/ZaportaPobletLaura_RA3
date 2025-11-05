using UnityEngine;

// Quan es destrueixi => spawner.Push(gameObject);
public class Bullet : MonoBehaviour, ISpawnable
{
    [SerializeField] private Rigidbody2D _rb;

    public Spawner spawner { get; set; }

}