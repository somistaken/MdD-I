using UnityEngine;

public class EscapeTrigger : MonoBehaviour
{
    private bool hasEscaped = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasEscaped && other.CompareTag("Player"))
        {
            hasEscaped = true;

            if (WinScreenUI.GetInstance() != null)
            {
                WinScreenUI.GetInstance().ShowWinScreen();
            }
        }
    }
}