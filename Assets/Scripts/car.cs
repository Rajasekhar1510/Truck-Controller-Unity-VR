using UnityEngine;

public class car : MonoBehaviour
{
    public Rigidbody rigid;
    public WheelCollider wheel1, wheel2, wheel3, wheel4;
    public float drivespeed = 500f;
    public float reverseSpeed = 150f;
    public float brakeForce = 3000f;

    [Header("Steering Settings")]
    public float steerspeed = 30f; 
    public Transform steeringWheel; 
    public float maxSteeringWheelAngle = 140f;

    void FixedUpdate()
    {
        float currentMotor = 0f;
        float currentBrake = 0f;

        if (OVRInput.Get(OVRInput.RawButton.A))
        {
            currentMotor = drivespeed;
            currentBrake = 0f;
        }
        else if (OVRInput.Get(OVRInput.RawButton.B))
        {
            Vector3 localVelocity = transform.InverseTransformDirection(rigid.linearVelocity);

            if (localVelocity.z > 0.5f)
            {
                currentMotor = 0f;
                currentBrake = brakeForce;
            }
            else
            {
                currentMotor = -reverseSpeed;
                currentBrake = 0f;
            }
        }
        else
        {
            currentMotor = 0f;
            currentBrake = 50f;
        }

        wheel1.motorTorque = currentMotor;
        wheel2.motorTorque = currentMotor;
        wheel3.motorTorque = currentMotor;
        wheel4.motorTorque = currentMotor;

        wheel1.brakeTorque = currentBrake;
        wheel2.brakeTorque = currentBrake;
        wheel3.brakeTorque = currentBrake;
        wheel4.brakeTorque = currentBrake;

        if (steeringWheel != null)
        {
            float zAngle = steeringWheel.localEulerAngles.z;

            if (zAngle > 180f)
            {
                zAngle -= 360f;
            }

            float steeringInput = Mathf.Clamp(zAngle / maxSteeringWheelAngle, -1f, 1f);

            wheel1.steerAngle = steerspeed * steeringInput;
            wheel2.steerAngle = steerspeed * steeringInput;
        }
    }
}