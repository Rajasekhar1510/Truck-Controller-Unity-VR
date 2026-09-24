using UnityEngine;

public class WallDetector : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (ParkingTrigger.Instance != null)
            {
                ParkingTrigger.Instance.FailObjective();
            }
        }
    }
}