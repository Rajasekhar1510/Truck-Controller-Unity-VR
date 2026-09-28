using System.Collections;
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

        // Start the Coroutine loop instead of using Update
        StartCoroutine(MovementRoutine());
    }

    private IEnumerator MovementRoutine()
    {
        // Loop continuously while the NPC exists
        while (true)
        {
            // If the light is Red, the NPC is allowed to start heading to a waypoint
            if (trafficLight.CanNPCsCross)
            {
                Transform target = waypoints[currentWaypointIndex];

                // Halt this outer loop and run the inner movement loop until the waypoint is reached
                yield return StartCoroutine(WalkToWaypoint(target));

                // Once the waypoint is reached, update the index for the next journey
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = 0;
                }
            }
            else
            {
                // Optimization: If the light is green/yellow, pause execution entirely.
                // This replaces the need to check 'if (CanNPCsCross)' every single frame in Update().
                yield return new WaitUntil(() => trafficLight.CanNPCsCross);
            }
        }
    }

    private IEnumerator WalkToWaypoint(Transform target)
    {
        // This loop ignores the traffic light. Once it starts, the NPC commits to reaching the target.
        while (Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            // Move towards the waypoint
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // Face the waypoint
            Vector3 direction = (target.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            // Yield execution until the next frame, then resume moving
            yield return null;
        }
    }
}