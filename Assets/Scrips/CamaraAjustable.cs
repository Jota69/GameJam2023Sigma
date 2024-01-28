using Cinemachine;
using UnityEngine;

public class ZoomCamara : MonoBehaviour
{
    public CinemachineVirtualCamera camaraVirtual;
    public float minFOV = 15f;
    public float maxFOV = 60f;
    public float sensibilidad = 10f;

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            camaraVirtual.m_Lens.FieldOfView -= Input.GetAxis("Mouse ScrollWheel") * sensibilidad;
            camaraVirtual.m_Lens.FieldOfView = Mathf.Clamp(camaraVirtual.m_Lens.FieldOfView, minFOV, maxFOV);
        }
    }
}
