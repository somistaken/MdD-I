using UnityEngine;
using UnityEngine.AI;

public class DoorController : MonoBehaviour, IInteractable
{
    public enum AxisDirection { X_Rojo, Y_Verde, Z_Azul }

    [Header("Orientation Settings")]
    [Tooltip("Selecciona el eje (la flecha del Gizmo) que apunta hacia la cara de esta puerta.")]
    [SerializeField] private AxisDirection faceAxis = AxisDirection.Z_Azul;

    [Tooltip("Marca esta casilla si la puerta se abre hacia tu cara en lugar de alejarse.")]
    [SerializeField] private bool invertDirection = false;

    [Header("Lock System")]
    [Tooltip("Si está marcado, la puerta requerirá aceite para poder abrirse por primera vez.")]
    [SerializeField] private bool isStuck = false;

    private Animator doorAnim;
    private AudioSource doorSound;
    private bool doorIsOpen;
    private bool openedInward;
    private NavMeshObstacle navObstacle;

    private void Awake()
    {
        doorIsOpen = false;
        doorAnim = GetComponent<Animator>();
        doorSound = GetComponent<AudioSource>();

        navObstacle = GetComponent<NavMeshObstacle>();
        if (navObstacle != null) navObstacle.carving = true;
    }

    public void Interact()
    {
        AnimatorStateInfo stateInfo = doorAnim.GetCurrentAnimatorStateInfo(0);

        if (doorAnim.IsInTransition(0) || (!stateInfo.IsName("Idle") && stateInfo.normalizedTime < 1f))
        {
            return;
        }

        if (isStuck)
        {
            if (PlayerInventory.GetInstance().IsItemInInventory("oil"))
            {
                isStuck = false;
                PlayerInventory.GetInstance().RemoveItem("oil");

                if (FeedbackUI.GetInstance() != null)
                {
                    FeedbackUI.GetInstance().ShowMessage("He engrasado las bisagras. Ya puedo abrirla.");
                }
                return;
            }
            else
            {
                if (FeedbackUI.GetInstance() != null)
                {
                    FeedbackUI.GetInstance().ShowMessage("La puerta está atascada. Necesito aceite...");
                }
                return;
            }
        }
        // ------------------------------------

        doorSound.Play();
        doorIsOpen = !doorIsOpen;

        if (doorIsOpen)
        {
            if (navObstacle != null) navObstacle.carving = false;

            Vector3 directionToPlayer = (Camera.main.transform.position - transform.position).normalized;

            Vector3 faceDirection = transform.forward;

            if (faceAxis == AxisDirection.X_Rojo) faceDirection = transform.right;
            else if (faceAxis == AxisDirection.Y_Verde) faceDirection = transform.up;

            float dotProduct = Vector3.Dot(faceDirection, directionToPlayer);

            if (invertDirection)
            {
                dotProduct *= -1;
            }

            if (dotProduct < 0)
            {
                doorAnim.Play("DoorOpenInward");
                openedInward = true;
            }
            else
            {
                doorAnim.Play("DoorOpenOutward");
                openedInward = false;
            }
        }
        else
        {
            if (navObstacle != null) navObstacle.carving = true;

            if (openedInward)
            {
                doorAnim.Play("DoorCloseInward");
            }
            else
            {
                doorAnim.Play("DoorCloseOutward");
            }
        }
    }
}