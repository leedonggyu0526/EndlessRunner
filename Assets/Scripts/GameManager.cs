using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    int score;
    public static GameManager inst;
    public GameObject chaser;
    public float delay = 3f;

    [SerializeField] Text scoreText;

    [SerializeField] PlayerMovement playerMovement;

    private void Awake ()
    {
        inst = this;
    }

    private void Start()
    {
        StartCoroutine(SpawnChaserAfterDelay(delay));
    }

    private IEnumerator SpawnChaserAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(chaser, new Vector3(0, 0, 5), Quaternion.identity);
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
        scoreText.text = "Score: " + score;

        playerMovement.speed += playerMovement.speedIncreasePerPoint;
    }
    public int GetScore()
    {
        return score;
    }
}