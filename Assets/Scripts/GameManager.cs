using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    int score;
    public static GameManager inst;

    [SerializeField] Text scoreText;

    [SerializeField] PlayerMovement playerMovement;

    private void Awake ()
    {
        inst = this;
    }

    private void Start () {
    }

	private void Update () {
	
	}
    public void IncrementScore()
    {
        // To activate the PlayerMovement component if it is disabled
        if (playerMovement != null && !playerMovement.enabled)
        {
            playerMovement.enabled = true;
        }
        score++;
        scoreText.text = "SCORE: " + score;

        playerMovement.speed += playerMovement.speedIncreasePerPoint;
    }
}