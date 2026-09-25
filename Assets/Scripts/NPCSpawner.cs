using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public int npcCount = 5;

    [Header("References")]
    public Transform spawnPoint;
    public Transform[] crossingWaypoints;
    public TrafficLightController trafficLight;

    private void Start()
    {
        for (int i = 0; i < npcCount; i++)
        {
            // Add a small random offset so NPCs don't spawn exactly inside each other
            Vector3 randomOffset = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
            GameObject newNPC = Instantiate(npcPrefab, spawnPoint.position + randomOffset, spawnPoint.rotation);

            NPCMovement movement = newNPC.GetComponent<NPCMovement>();
            if (movement != null)
            {
                movement.Initialize(crossingWaypoints, trafficLight);
            }
        }
    }
}