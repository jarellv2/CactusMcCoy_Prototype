using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f;
    public float patrolDistance = 3f;
    public int health = 50;
    private Vector2 startPos;

    void Start() { startPos = transform.position; }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * speed, patrolDistance);
        transform.position = new Vector2(startPos.x + offset, startPos.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Bullet b = other.GetComponent<Bullet>();
        if (b != null)
        {
            health -= b.damage;
            Destroy(other.gameObject);
            Debug.Log("Enemy hit! Remaining HP: " + health);
            if (health <= 0) Destroy(gameObject);
        }
        else if (other.gameObject.name == "Player")
        {
            GameManager.Instance.TakeDamage(20);
        }
    }
}