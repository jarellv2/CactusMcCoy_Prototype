using UnityEngine;
using System.Collections;

public abstract class ProjectileSpawner : MonoBehaviour
{
    public abstract Bullet CreateBullet(Vector2 pos, float dir, int damage);
}

public class ProjectileFactory : ProjectileSpawner
{
    public static ProjectileFactory Instance { get; private set; }
    public GameObject bulletPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnShot(Vector2 pos, float dir, bool burstMode, int damage)
    {
        if (!burstMode)
        {
            CreateBullet(pos, dir, damage);
        }
        else
        {
            StartCoroutine(BurstRoutine(pos, dir, damage));
        }
    }

    private IEnumerator BurstRoutine(Vector2 pos, float dir, int damage)
    {
        for (int i = 0; i < 3; i++)
        {
            CreateBullet(pos, dir, damage);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public override Bullet CreateBullet(Vector2 pos, float dir, int damage)
    {
        GameObject b = Instantiate(bulletPrefab, pos, Quaternion.identity);
        Bullet bulletScript = b.GetComponent<Bullet>();
        if (bulletScript == null) bulletScript = b.AddComponent<Bullet>();
        bulletScript.direction = dir;
        bulletScript.damage = damage;
        Destroy(b, 2f);
        return bulletScript;
    }
}