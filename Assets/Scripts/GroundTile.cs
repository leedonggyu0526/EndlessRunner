using System.Collections.Generic;
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

    public void EndPointReact(Collider collider)
    {
        //Debug.Log("Triggered");
        //Debug.Log(collider.gameObject.name);
        if (collider.gameObject.name == "Player")
        {
            //Debug.Log("Spawn");
            groundSpawner.SpawnTile(true);
            Destroy(gameObject, 2);
        }
    }

    public void SpawnObstacle(GameObject ground)
    {
        int obstacleSpawn = 10;
        List<GameObject> targetObjects = FindChildObjectsWithTag(ground, "Road");
        for (int i = 0; i < obstacleSpawn; i++)
        {
            GameObject temp = Instantiate(obstaclePrefab, transform);
            if (targetObjects.Count > 0)
            {
                GameObject targetObject = targetObjects[i % targetObjects.Count];
                temp.transform.position = GetRandomPointAboveObject(targetObject);
            }
        }
    }

    public void SpawnCoins(GameObject ground)
    {
        int coinSpawn = 10;
        List<GameObject> targetObjects = FindChildObjectsWithTag(ground, "Road");
        for (int i = 0; i < coinSpawn; i++)
        {
            GameObject temp = Instantiate(coinPrefab, transform);
            if (targetObjects.Count > 0)
            {
                GameObject targetObject = targetObjects[i % targetObjects.Count];
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



    List<GameObject> FindChildObjectsWithTag(GameObject parent, string tag)
    {
        List<GameObject> taggedObjects = new List<GameObject>();

        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag(tag))
            {
                taggedObjects.Add(child.gameObject);
            }
        }

        return taggedObjects;
    }

}
