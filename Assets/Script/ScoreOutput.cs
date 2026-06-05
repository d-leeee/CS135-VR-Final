using UnityEngine;
using UnityEngine.UI;

public class ScoreOutput : MonoBehaviour
{
    public static int score = 0;
    public Text scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score:" + score;
        }
    }
}
