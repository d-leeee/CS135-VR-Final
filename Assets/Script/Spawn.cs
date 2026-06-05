using UnityEngine;

public class Spawn : MonoBehaviour
{
    [Header("Prefab to Spawn")]
    public GameObject prefabToSpawn;

    [Header("Rate of Spawning (in seconds)")]
    public float spawnInterval = 2f; 

    // Keeps track of when to spawn the next object
    private float nextSpawnTime;

    void Update()
    {
        // Check if the current time has passed the next scheduled spawn time
        if (Time.time >= nextSpawnTime)
        {
            SpawnObject();
            
            // Calculate when the next spawn should happen
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnObject()
    {
        // Ensure we actually assigned a prefab in the inspector
        if (prefabToSpawn != null)
        {
            // Instantiate creates a clone of the prefab at the spawner's current position and rotation
            Instantiate(prefabToSpawn, transform.position, transform.rotation);
            Instantiate(prefabToSpawn, transform.position, transform.rotation);
        }
        else
        {
            Debug.LogWarning("Please assign a Prefab to the ObjectSpawner script!");
        }
    }
}