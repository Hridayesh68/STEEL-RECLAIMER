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

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
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

        OnShootEventArgs args = new OnShootEventArgs
        {
            gunEndPointPosition = gunEndPoint.position,
            shootDirection = gunEndPoint.forward
        };

        onShoot?.Invoke(this, args);
    }
}