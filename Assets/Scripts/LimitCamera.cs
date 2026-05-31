using UnityEngine;

public class LimitCamera : MonoBehaviour
{
    [Header("Player Armature")]
    public Transform playerArmature;

    [Header("Camera Height")]
    public float height = 25f;

    [Header("Smooth Follow")]
    public float followSpeed = 10f;

    private void Start()
    {
        // Auto-find if not assigned
        if (playerArmature == null)
        {
            GameObject player = GameObject.Find("Player");

            if (player != null)
            {
                Transform armature = player.transform.Find("PlayerArmature");

                if (armature != null)
                {
                    playerArmature = armature;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (playerArmature == null)
            return;

        Vector3 targetPosition = new Vector3(
            playerArmature.position.x,
            playerArmature.position.y + height,
            playerArmature.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );

        // Always look straight down
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}