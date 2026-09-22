using UnityEngine;

public class wheel : MonoBehaviour
{
    public WheelCollider wheelCollider;
    public Transform wheelMesh;
    public bool wheelTurn; //true IF the wheels turn left/right

    void Update()
    {
        if (wheelTurn == true)
        {
            wheelMesh.localEulerAngles = new Vector3(
                wheelMesh.localEulerAngles.x,
                wheelCollider.steerAngle - wheelMesh.localEulerAngles.z,
                wheelMesh.localEulerAngles.z
            );
        }

        wheelMesh.Rotate(wheelCollider.rpm / 60 * 360 * Time.deltaTime, 0, 0);
    }
}