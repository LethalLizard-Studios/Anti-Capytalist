using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public Island currentIsland;

    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private CharacterMovement character;
    [SerializeField] private TeamController teams;
    [SerializeField] private GameObject tellJokeUI;
    [SerializeField] private GameObject tutorialUI;

    private AudioSource moveSound;

    private const int ROTATE_SPEED = 50;
    private int rotateDir = 0;

    private Vector3 m_movePos = Vector3.zero;

    private void Start()
    {
        moveSound = GetComponent<AudioSource>();

        Move(currentIsland);
        InteractionInterface.Instance.Show(currentIsland);
        character.JumpToPoint(currentIsland.transform.position);
        m_movePos = transform.position;
    }

    public void RotateRight(InputAction.CallbackContext context)
    {
        if (tellJokeUI.activeSelf)
            return;

        if (context.phase == InputActionPhase.Started)
            rotateDir = -1;
        else if (context.phase == InputActionPhase.Canceled)
            rotateDir = 0;
    }

    public void RotateLeft(InputAction.CallbackContext context)
    {
        if (tellJokeUI.activeSelf)
            return;

        if (context.phase == InputActionPhase.Started)
            rotateDir = 1;
        else if (context.phase == InputActionPhase.Canceled)
            rotateDir = 0;
    }

    private void Move(Island island)
    {
        if (tellJokeUI.activeSelf || tutorialUI.activeSelf)
            return;

        moveSound.pitch = Random.Range(1.05f, 1.2f);
        moveSound.Play();

        Vector3 pos = island.transform.position;

        ShopManager.Instance.CloseStorefront();

        character.JumpToPoint(island.m_playerMovePt.position);
        m_movePos = new Vector3(pos.x, pos.y + 10f, pos.z);

    }

    public void Select(Island island)
    {
        if (tellJokeUI.activeSelf || tutorialUI.activeSelf)
            return;

        float dist = Vector3.Distance(transform.position, currentIsland.transform.position);
        if (dist > 15)
            return;

        currentIsland.Deselect();

        island.Select();
        currentIsland = island;

        Move(island);
        teams.UpdateUI(island);
    }

    public void ClickMove(InputAction.CallbackContext context)
    {
        if (tellJokeUI.activeSelf || tutorialUI.activeSelf)
            return;

        if (context.phase == InputActionPhase.Started)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out hit, 100000f, layerMask))
            {
                if (hit.transform != null)
                {
                    Island island;

                    if (hit.transform.TryGetComponent(out island))
                    {
                        float dist = Vector3.Distance(transform.position, island.transform.position);

                        if (dist <= 60)
                            Select(island);
                    }
                }
            } 
        }
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, m_movePos, Time.deltaTime * 5.0f);
        transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y + (rotateDir * ROTATE_SPEED * Time.deltaTime), 0);
    }
}
