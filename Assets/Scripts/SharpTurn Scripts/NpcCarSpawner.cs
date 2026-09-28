using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCarSpawner : MonoBehaviour
{
    [Header("Pool Setup")]
    [SerializeField] private NpcCarMovement[] carPrefabs;
    [SerializeField] private int poolSizePerPrefab = 3;

    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;

    [Header("Timing Settings")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float respawnDelay = 5f;

    [Header("Car Settings")]
    [SerializeField] private float carSpeed = 10f;

    private Queue<NpcCarMovement> carPool = new Queue<NpcCarMovement>();

    private void Start()
    {
        InitializePool();
        StartCoroutine(SpawnRoutine());
    }

    private void InitializePool()
    {
        foreach (NpcCarMovement prefab in carPrefabs)
        {
            for (int i = 0; i < poolSizePerPrefab; i++)
            {
                NpcCarMovement carInstance = Instantiate(prefab, transform.position, Quaternion.identity);
                carInstance.gameObject.SetActive(false);

                carInstance.Setup(waypoints, carSpeed, OnCarPathComplete);

                carPool.Enqueue(carInstance);
            }
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (carPool.Count > 0 && waypoints.Length > 0)
            {
                NpcCarMovement car = carPool.Dequeue();

                car.transform.position = waypoints[0].position;
                car.transform.rotation = waypoints[0].rotation;

                // Activate and move
                car.gameObject.SetActive(true);
                car.StartMoving();
            }

            // Wait for the exact interval before trying to spawn the next car
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    //This is triggered by NpcCarMovement when it reaches the last waypoint
    private void OnCarPathComplete(NpcCarMovement car)
    {
        car.gameObject.SetActive(false);
        StartCoroutine(RecycleDelayRoutine(car));
    }

    private IEnumerator RecycleDelayRoutine(NpcCarMovement car)
    {
        yield return new WaitForSeconds(respawnDelay);
        carPool.Enqueue(car);
    }
}