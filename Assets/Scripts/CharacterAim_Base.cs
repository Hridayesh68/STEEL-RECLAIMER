using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAim_Base : MonoBehaviour
{
    public event EventHandler<OnShootEventArgs> onShoot;

    [Serializable]
    public class OnShootEventArgs : EventArgs
    {
        public Vector3 spawnPosition;
        public Vector3 shootDirection;
    }

    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform gunEndPoint;

    [Header("Aim Settings")]
    [SerializeField] private float maxAimDistance = 500f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Projectile")]
    [SerializeField] private float projectileSpawnDistance = 1.5f;

    private Vector3 currentAimPoint;

    private void Update()
    {
        UpdateAimPoint();

        RotateTowardsAim();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    private void UpdateAimPoint()
    {
        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance))
        {
            currentAimPoint = hit.point;
        }
        else
        {
            currentAimPoint = ray.GetPoint(maxAimDistance);
        }
    }

    private void RotateTowardsAim()
    {
        if (playerBody == null)
            return;

        Vector3 direction =
            currentAimPoint - playerBody.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        playerBody.rotation =
            Quaternion.Slerp(
                playerBody.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        // Spawn projectile slightly in front of camera
        Vector3 spawnPosition =
            playerCamera.transform.position +
            playerCamera.transform.forward *
            projectileSpawnDistance;

        // Direction from camera to aim point
        Vector3 shootDirection =
            (currentAimPoint - spawnPosition).normalized;

        OnShootEventArgs args = new OnShootEventArgs
        {
            spawnPosition = spawnPosition,
            shootDirection = shootDirection
        };

        onShoot?.Invoke(this, args);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(currentAimPoint, 0.2f);
    }
}