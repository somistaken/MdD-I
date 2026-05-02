using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private EnemyAI enemyAI;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        enemyAI.SetPlayerReference(playerTransform);
    }

    public static GameManager GetInstance()
    {
        return instance;
    }

    public void RegisterPlayer(Transform newPlayerTransform)
    {
        playerTransform = newPlayerTransform;

        enemyAI.SetPlayerReference(playerTransform);
    }
}