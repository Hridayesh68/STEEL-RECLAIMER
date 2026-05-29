using UnityEngine;

public class HealthBarBillboard : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (cam != null)
        {
            transform.forward = cam.transform.forward;
        }
    }
}