using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteReaderUI : MonoBehaviour
{
    static NoteReaderUI instance;

    [Header("References")]
    [SerializeField] private GameObject readerPanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("more than 1 reader ui in the scene");
        }
        instance = this;

        readerPanel.SetActive(false);
    }

    public static NoteReaderUI GetInstance()
    {
        return instance;
    }

    public void OpenNote(ItemSO noteData)
    {
        titleText.text = noteData.itemName;
        contentText.text = noteData.description;

        readerPanel.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseNote()
    {
        readerPanel.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}