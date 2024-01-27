using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI.Table;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private AnimationCurve m_jumpCurve;

    private Vector3 pos = Vector3.zero;
    private Quaternion rot = Quaternion.identity;
    private float startDist = 0.0f;

    private const int JUMP_HEIGHT_MULTIPLIER = 4;

    public void JumpToPoint(Vector3 pos)
    {
        this.pos = pos;
        startDist = Vector3.Distance(transform.position, pos);

        rot = Quaternion.LookRotation(transform.position - pos, Vector3.forward);
        rot.x = 0.0f;
        rot.z = 0.0f;
    }

    private void Update()
    {
        float y = pos.y;

        if (startDist > 0)
            y += m_jumpCurve.Evaluate(Vector3.Distance(transform.position, pos) / startDist) * JUMP_HEIGHT_MULTIPLIER;

        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5);
        transform.position = Vector3.Lerp(transform.position, new Vector3(pos.x, y, pos.z), Time.deltaTime * 5.0f);
    }
}
