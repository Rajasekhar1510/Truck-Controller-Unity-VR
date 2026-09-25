using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float speed = 2f;
    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private TrafficLightController trafficLight;

    public void Initialize(Transform[] assignedWaypoints, TrafficLightController lightController)
    {
        waypoints = assignedWaypoints;
        trafficLight = lightController;
    }

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0 || trafficLight == null) return;

        // Only move toward the next waypoint if the traffic light allows it
        if (trafficLight.CanNPCsCross)
        {
            MoveToNextWaypoint();
        }
    }

    private void MoveToNextWaypoint()
    {
        Transform target = waypoints[currentWaypointIndex];

        // Move towards the waypoint
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Face the waypoint
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        // Check if waypoint is reached
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                // Loop back to start to continuously cross back and forth
                currentWaypointIndex = 0;
            }
        }
    }
}