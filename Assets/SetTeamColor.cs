using UnityEngine;

public class SetTeamColor : MonoBehaviour
{
    private Material teamMat;
    public MeshRenderer[] meshes;

    public void Change(Material mat)
    {
        teamMat = mat;
    }
}
