using UnityEngine;

public abstract class ICommand
{
    public abstract void Execute(PlayerController player);
}

public class JumpCommand : ICommand
{
    public override void Execute(PlayerController p) => p.Jump();
}

public class ShootCommand : ICommand
{
    public override void Execute(PlayerController p) => p.Shoot();
}

public class SwitchFireModeCommand : ICommand
{
    public override void Execute(PlayerController p) => p.ToggleFireMode();
}

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 7f;
    public bool isBurstFire = false;
    public int baseBulletDamage = 25;

    private float facingDir = 1f;
    private Rigidbody2D rb;

    private ICommand jumpCmd = new JumpCommand();
    private ICommand shootCmd = new ShootCommand();
    private ICommand switchModeCmd = new SwitchFireModeCommand();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MyPlugin.UpdateWeaponDamage(baseBulletDamage, isBurstFire);
    }

    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");
        if (move != 0) facingDir = Mathf.Sign(move);
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) jumpCmd.Execute(this);
        if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0)) shootCmd.Execute(this);
        if (Input.GetKeyDown(KeyCode.Q)) switchModeCmd.Execute(this);
    }

    public void Jump()
    {
        if (Mathf.Abs(rb.linearVelocity.y) < 0.05f)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void Shoot()
    {
        int currentDamage = MyPlugin.Damage;
        ProjectileFactory.Instance.SpawnShot(transform.position, facingDir, isBurstFire, currentDamage);
    }

    public void ToggleFireMode()
    {
        isBurstFire = !isBurstFire;
        MyPlugin.UpdateWeaponDamage(baseBulletDamage, isBurstFire);
        Debug.Log("Burst Mode: " + isBurstFire + " | DLL Damage: " + MyPlugin.Damage);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Key")
        {
            GameManager.Instance.CollectKey();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.name == "Door")
        {
            if (GameManager.Instance.hasKey)
            {
                GameManager.Instance.CompleteLevel();
            }
            else
            {
                Debug.Log("Door is locked! Find the Gold Key first.");
            }
        }
        else if (other.gameObject.name == "Hazard")
        {
            GameManager.Instance.TakeDamage(100);
        }
    }
}