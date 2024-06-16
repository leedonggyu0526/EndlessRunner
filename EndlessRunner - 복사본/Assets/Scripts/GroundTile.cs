using UnityEngine;

public class GroundTile : MonoBehaviour
{

    GroundSpawner groundSpawner;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject obstaclePrefab;

    private void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
    }

    private void OnTriggerExit(Collider collider)
    {
        Debug.Log("Triggered");
        if (collider.gameObject.name == "EndPoint")
        {
            Debug.Log("Spawn");
            groundSpawner.SpawnTile(true);
            Destroy(gameObject, 2);
        }
    }

    public void SpawnObstacle()
    {
        int obstacleSpawn = 10;
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("Road");
        for (int i = 0; i < obstacleSpawn; i++)
        {
            GameObject temp = Instantiate(obstaclePrefab, transform);
            if (targetObjects.Length > 0)
            {
                GameObject targetObject = targetObjects[i % targetObjects.Length];
                temp.transform.position = GetRandomPointAboveObject(targetObject);
            }
        }
    }

    Vector3 GetRandomPointAboveObject(GameObject targetObject)
    {
        Collider targetCollider = targetObject.GetComponent<Collider>();
        Bounds bounds = targetCollider.bounds;
        Vector3 point = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.max.y + 1,
            Random.Range(bounds.min.z, bounds.max.z)
        );
        return point;
    }


    public void SpawnCoins()
    {
        int coinSpawn = 10;
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("Road");
        for (int i = 0; i < coinSpawn; i++)
        {
            GameObject temp = Instantiate(coinPrefab, transform);
            if (targetObjects.Length > 0)
            {
                GameObject targetObject = targetObjects[i % targetObjects.Length];
                temp.transform.position = GetRandomPointAboveObject(targetObject);
            }
        }
    }
}
