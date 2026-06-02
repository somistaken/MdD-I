using System.Collections;
using TMPro;
using UnityEngine;

public class FeedbackUI : MonoBehaviour
{
    private static FeedbackUI instance;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI feedbackText;

    [Header("Settings")]
    [SerializeField] private float displayTime = 3f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        feedbackText.enabled = false;
    }

    public static FeedbackUI GetInstance()
    {
        return instance;
    }

    public void ShowMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(DisplayMessageRoutine(message));
    }

    private IEnumerator DisplayMessageRoutine(string message)
    {
        feedbackText.text = message;
        feedbackText.enabled = true;

        yield return new WaitForSeconds(displayTime);

        feedbackText.enabled = false;
    }
}