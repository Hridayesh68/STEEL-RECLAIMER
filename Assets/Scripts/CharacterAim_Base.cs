using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAim_Base : MonoBehaviour
{
    public event EventHandler<OnShootEventArgs> onShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Vector3 gunEndPointPosition;
        public Vector3 shootDirection;
    }

    [SerializeField] private Transform gunEndPoint;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float maxShootDistance = 1000f;

    private void Update()
    {
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (gunEndPoint == null)
        {
            Debug.LogError("GunEndPoint not assigned!");
            return;
        }

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // Ray from center of screen (crosshair)
        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f));

        Vector3 targetPoint;

        if (Physics.Raycast(ray,
                            out RaycastHit hit,
                            maxShootDistance))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(maxShootDistance);
        }

        // Direction from gun muzzle to target
        Vector3 shootDirection =
            (targetPoint - gunEndPoint.position).normalized;

        Debug.DrawRay(
            gunEndPoint.position,
            shootDirection * 50f,
            Color.red,
            1f);

        OnShootEventArgs args = new OnShootEventArgs
        {
            gunEndPointPosition = gunEndPoint.position,
            shootDirection = shootDirection
        };

        onShoot?.Invoke(this, args);
    }
}