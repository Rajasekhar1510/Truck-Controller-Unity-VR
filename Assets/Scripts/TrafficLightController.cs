using System.Collections;
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    [Header("Light Objects")]
    public GameObject redLight;
    public GameObject yellowLight;
    public GameObject greenLight;

    [Header("Durations (Seconds)")]
    public float redDuration = 5f;
    public float yellowDuration = 2f;
    public float greenDuration = 5f;

    [Header("Crosswalk Settings")]
    public GameObject crosswalkFailureZone;//if the player goes here when red light is on, the level resets

    public bool CanNPCsCross { get; private set; }

    private void Start()
    {
        StartCoroutine(LightCycleRoutine());
    }

    private IEnumerator LightCycleRoutine()
    {
        while (true)
        {
            SetLights(false, false, true);
            CanNPCsCross = false;
            if (crosswalkFailureZone != null) 
                crosswalkFailureZone.SetActive(false);

            yield return new WaitForSeconds(greenDuration);

            SetLights(false, true, false);
            yield return new WaitForSeconds(yellowDuration);


            SetLights(true, false, false);
            CanNPCsCross = true;

            if (crosswalkFailureZone != null) 
                crosswalkFailureZone.SetActive(true);

            yield return new WaitForSeconds(redDuration);
        }
    }

    private void SetLights(bool r, bool y, bool g)
    {
        if (redLight != null) 
            redLight.SetActive(r);

        if (yellowLight != null) 
            yellowLight.SetActive(y);

        if (greenLight != null) 
            greenLight.SetActive(g);
    }
}