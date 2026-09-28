using System;
using System.Collections;
using UnityEngine;

public class NpcCarMovement : MonoBehaviour
{
    private Transform[] waypoints;
    private float speed;
    private float rotationSpeed = 150f;
    private Action<NpcCarMovement> onPathComplete;

    //Called once by the Spawner to inject references
    public void Setup(Transform[] pathWaypoints, float carSpeed, Action<NpcCarMovement> completionCallback)
    {
        waypoints = pathWaypoints;
        speed = carSpeed;
        onPathComplete = completionCallback;
    }

    // Called every time the car is pulled from the object pool
    public void StartMoving()
    {
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        int currentWaypointIndex = 0;

        while (currentWaypointIndex < waypoints.Length)
        {
            Transform target = waypoints[currentWaypointIndex];

            while (Vector3.SqrMagnitude(transform.position - target.position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

                Vector3 direction = (target.position - transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
                }

                yield return null;
            }

            currentWaypointIndex++;
        }

        //Reached the final waypoint, tell the Spawner to recycle this car
        onPathComplete?.Invoke(this);
    }
}