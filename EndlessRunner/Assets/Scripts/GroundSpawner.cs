using UnityEngine;

public class GroundSpawner : MonoBehaviour {

    [SerializeField] GameObject[] groundTile;
    Vector3 nextSpawnPoint;

    public void SpawnTile (bool spawnItems)
    {
        int randomIndex = Random.Range(0, groundTile.Length);
        GameObject temp = Instantiate(groundTile[randomIndex], nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = temp.transform.GetChild(1).transform.position;

        if (spawnItems) {
            temp.GetComponent<GroundTile>().SpawnObstacle();
            temp.GetComponent<GroundTile>().SpawnCoins();
        }
    }

    private void Start () {
        for (int i = 0; i < 3; i++) {
            SpawnTile(true);
        }
    }
}