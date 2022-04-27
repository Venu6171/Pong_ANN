using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;
    public static GameManager GetInstance()
    {
        return Instance;
    }

    public static int PlayerScore1 = 0;
    public static int PlayerScore2 = 0;

    [SerializeField] private int scoreLimit;

    public GUISkin layout;
    private GameObject _Ball;
    public int matchCount = 0;
    private int matchCounter = 10;
    public string inputValueFileName;
    public string targetValueFileName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Game_Manager Instance created");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Game_Manager duplicate Destroyed");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _Ball = GameObject.FindGameObjectWithTag("Ball");
    }

    public static void Score(string wallID)
    {
        if (wallID == "RightWall")
        {
            PlayerScore1++;
        }
        else
        {
            PlayerScore2++;
        }
    }

    void OnGUI()
    {
        GUI.skin = layout;
        GUI.Label(new Rect(Screen.width / 2 - 150 - 12, 20, 100, 100), "" + PlayerScore1);
        GUI.Label(new Rect(Screen.width / 2 + 150 + 12, 20, 100, 100), "" + PlayerScore2);

        if (PlayerScore1 == matchCounter || PlayerScore2 == matchCounter)
        {
            if (matchCounter != scoreLimit)
            {
                matchCount += 1;
                matchCounter += 10;
            }
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 60, 35, 120, 53), "RESTART"))
        {
            PlayerScore1 = 0;
            PlayerScore2 = 0;
            _Ball.SendMessage("RestartGame", 0.5f, SendMessageOptions.RequireReceiver);
        }

        if (PlayerScore1 == scoreLimit)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER ONE WINS");
            _Ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }
        else if (PlayerScore2 == scoreLimit)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER TWO WINS");
            _Ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }
    }
}
