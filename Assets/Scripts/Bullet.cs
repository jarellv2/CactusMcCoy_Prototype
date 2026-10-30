using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float direction = 1f;
    public float speed = 12f;
    public int damage = 25;

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }
}