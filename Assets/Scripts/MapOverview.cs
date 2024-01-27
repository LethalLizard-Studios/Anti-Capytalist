using UnityEngine;
using UnityEngine.InputSystem;

public class MapOverview : MonoBehaviour
{
    [SerializeField] private Transform cameraObj;

    [SerializeField] private Vector3 defaultPos = Vector3.zero;
    [SerializeField] private Vector3 defaultRot = Vector3.zero;

    [SerializeField] private Vector3 overviewPos = Vector3.zero;
    [SerializeField] private Vector3 overviewRot = Vector3.zero;

    private Camera m_Camera;
    private bool isOnOverview = false;

    private void Start()
    {
        m_Camera = cameraObj.GetComponent<Camera>();
    }

    public void ToggleOverview(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            Toggle();
    }

    //Seperated so it can be called by buttons aswell as Inputs.
    public void Toggle()
    {
        isOnOverview = !isOnOverview;
    }

    private void Update()
    {
        if (isOnOverview)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, overviewPos, Time.deltaTime);
            cameraObj.localRotation = Quaternion.Lerp(cameraObj.localRotation, Quaternion.Euler(overviewRot), Time.deltaTime);
            m_Camera.orthographicSize = Mathf.Lerp(m_Camera.orthographicSize, 90, Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPos, Time.deltaTime);
            cameraObj.localRotation = Quaternion.Lerp(cameraObj.localRotation, Quaternion.Euler(defaultRot), Time.deltaTime);
            m_Camera.orthographicSize = Mathf.Lerp(m_Camera.orthographicSize, 50, Time.deltaTime);
        }
    }
}
