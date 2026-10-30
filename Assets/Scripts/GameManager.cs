using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int playerHealth = 100;
    public bool hasKey = false;
    public bool isGameOver = false;
    public bool isLevelComplete = false;

    private DateTime _sessionStartTime;
    private DateTime _sessionEndTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;
        _sessionStartTime = DateTime.Now;
        Debug.Log("Game session start @: " + _sessionStartTime);
    }

    private void OnApplicationQuit()
    {
        _sessionEndTime = DateTime.Now;
        TimeSpan timeDifference = _sessionEndTime.Subtract(_sessionStartTime);
        Debug.Log("Game session ended @: " + _sessionEndTime);
        Debug.Log("Game session lasted: " + timeDifference);
    }

    public void TakeDamage(int dmg)
    {
        if (isGameOver || isLevelComplete) return;

        playerHealth -= dmg;
        Debug.Log("Player Health: " + playerHealth);

        if (playerHealth <= 0)
        {
            playerHealth = 0;
            isGameOver = true;
            Time.timeScale = 0f;
            Debug.Log("GAME OVER! Player died.");
        }
    }

    public void CollectKey()
    {
        hasKey = true;
        Debug.Log("Key Collected! Go to the Door.");
    }

    public void CompleteLevel()
    {
        if (isGameOver || isLevelComplete) return;

        isLevelComplete = true;
        Time.timeScale = 0f;
        Debug.Log("LEVEL COMPLETE! Door Unlocked!");
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 260, 110), "Cactus Prototype HUD");
        GUI.Label(new Rect(20, 35, 240, 20), "Health: " + playerHealth);
        GUI.Label(new Rect(20, 55, 240, 20), "DLL Weapon Damage: " + MyPlugin.Damage);
        GUI.Label(new Rect(20, 75, 240, 20), "Mode (Q): " + (MyPlugin.Damage > 25 ? "Burst (3x)" : "Single"));
        GUI.Label(new Rect(20, 95, 240, 20), "Key Collected: " + (hasKey ? "YES" : "NO"));

        if (isGameOver)
        {
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 40, 200, 80), "GAME OVER - YOU DIED");
            if (GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height / 2, 120, 30), "Restart Level"))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else if (isLevelComplete)
        {
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 40, 200, 80), "LEVEL COMPLETE!");
            if (GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height / 2, 120, 30), "Play Again"))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}