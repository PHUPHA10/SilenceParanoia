using UnityEngine;

public class Hightlight : MonoBehaviour
{
    public Material mat;
    public int indexOFMat = 1;

    private void Awake()
    {
        mat = GetComponent<Renderer>().materials[indexOFMat];
    }
    public void Outline(bool show)
    {
        mat.SetFloat("_ShowOutline", show ? 1 : 0);
    }
}
