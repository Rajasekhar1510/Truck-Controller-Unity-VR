using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ParkingTrigger : MonoBehaviour
{
    public static ParkingTrigger Instance;

    [Header("UI Panels")]
    public GameObject successPanel;
    public GameObject failurePanel;

    [Header("Scene Settings")]
    public float sceneLoadDelay = 2f;

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        if (successPanel != null)
        {
            successPanel.SetActive(false);
        }
        if (failurePanel != null)
        {
            failurePanel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGameOver)
            return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(HandleSuccess());
        }
    }

    public void FailObjective()
    {
        if (isGameOver)
            return;
        StartCoroutine(HandleFailure());
    }

    private IEnumerator HandleSuccess()
    {
        isGameOver = true;

        if (successPanel != null)
        {
            successPanel.SetActive(true);
        }

        yield return new WaitForSeconds(sceneLoadDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator HandleFailure()
    {
        isGameOver = true;

        if (failurePanel != null)
        {
            failurePanel.SetActive(true);
        }

        yield return new WaitForSeconds(sceneLoadDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}