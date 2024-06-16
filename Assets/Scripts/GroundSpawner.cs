using UnityEngine;

public class GroundSpawner : MonoBehaviour
{

    [SerializeField] GameObject[] groundTile;
    Vector3 nextSpawnPoint;
    Quaternion nextSpawnRotation; 

    public void SpawnTile(bool spawnItems)
    {
        int randomIndex = Random.Range(0, groundTile.Length);
        GameObject temp = Instantiate(groundTile[randomIndex], nextSpawnPoint, nextSpawnRotation);
        nextSpawnPoint = temp.transform.Find("EndPoint").position;
        nextSpawnRotation = temp.transform.Find("EndPoint").rotation;

        if (spawnItems)
        {   
            temp.GetComponent<GroundTile>().SpawnObstacle();
            temp.GetComponent<GroundTile>().SpawnCoins();
        }
    }

    private void Start()
    {
        SpawnTile(true);
        SpawnTile(true);
    }
}
