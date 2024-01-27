using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePointer : MonoBehaviour
{
    private Vector2 m_position = Vector2.zero;

    [SerializeField] private GameObject m_pointer;
    [SerializeField] private CameraMovement movement;

    [SerializeField] private LayerMask m_layerMask;

    private Controls _input;

    private void Start()
    {
        if (!GameManager.Instance.controller)
            gameObject.SetActive(false);

        _input = new Controls();
        _input.Player.Enable();
        _input.Player.Movement.performed += obj => m_position = obj.ReadValue<Vector2>();
    }

    public void SelectIsland(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("Trying to Select a Island");

            SphereCollider collider = null;

            if (movement.currentIsland != null)
            {
                if (movement.currentIsland.gameObject.TryGetComponent<SphereCollider>(out collider))
                    collider.enabled = false;
                else
                    Debug.LogError("Current Island does not have a Sphere Collider!");
            }

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, 60, m_layerMask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);

                Island hitIsland = null;
                if (hit.transform.TryGetComponent<Island>(out hitIsland))
                    movement.Select(hitIsland);
                else
                    Debug.LogError("Hit Island does not have a Island Component attached!");
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
            }

            if (collider != null)
                collider.enabled = true;
        }
    }



    bool test = false;

    private void Update()
    {
        if (m_position == Vector2.zero)
            return;

        if ((Mathf.Abs(m_position.x) + Mathf.Abs(m_position.y)) > 1.2f)
        {
            StopAllCoroutines();
            m_pointer.SetActive(true);
            test = false;
        }
        else
        {
            if (m_pointer.activeSelf && !test && (m_position.y <= 0 || m_position.x + 1 < 0.5f))
            {
                StopAllCoroutines();
                StartCoroutine(countDown());
                test = true;
            }
        }

        m_position = m_position.normalized;

        Quaternion rot = Quaternion.identity;

        if (m_position.y >= 0)
            rot = Quaternion.Euler(0, 0, (Mathf.Clamp(m_position.x + 1, 0.0f, 2.0f) * -90.0f) + 35.0f);
        else
            rot = Quaternion.Euler(0, 0, ((2 - Mathf.Clamp(m_position.x + 1, 0.0f, 2.0f)) * -90.0f) + 215.0f);

        transform.localRotation = Quaternion.Lerp(transform.localRotation, rot, Time.deltaTime * 50);
    }

    IEnumerator countDown()
    {
        yield return new WaitForSeconds(1);

        m_pointer.SetActive(false);
    }
}
