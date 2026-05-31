using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using UnityEngine.Animations.Rigging;
using Unity.Cinemachine;
using System.Collections;

public class WeaponHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineThirdPersonFollow cm_camera;

    private Animator anim;
    private ThirdPersonController controller;

    [Header("Shooting")]
    [SerializeField] private float fireRate = 0.09f;
    [SerializeField] private float shootBlendTime = 0.075f;
    [SerializeField] private string shootStateName = "Fire_Rifle";
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private ParticleSystem muzzleFlash;

    private bool canShoot = true;

    [Header("Aiming")]
    [SerializeField] private float cameraTransitionSpeed = 7f;
    [SerializeField] private float ikTransitionSpeed = 10f;
    [SerializeField] private MultiAimConstraint aimIk;

    [Space(10)]
    [SerializeField] private float aimVerticalArmLength = 0.2f;
    [SerializeField] private float aimCameraSide = 0.75f;
    [SerializeField] private float aimCameraDistance = 0.85f;

    private float defaultVerticalArmLength;
    private float defaultCameraSide;
    private float defaultCameraDistance;

    public bool Aiming { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject crosshair;

    private void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<ThirdPersonController>();

        defaultVerticalArmLength = cm_camera.VerticalArmLength;
        defaultCameraSide = cm_camera.CameraSide;
        defaultCameraDistance = cm_camera.CameraDistance;
    }

  private void Update()
{
    // INPUT SYSTEM
    Aiming = Mouse.current != null &&
             Mouse.current.rightButton.isPressed;

    bool shootInp = Mouse.current != null &&
                    Mouse.current.leftButton.isPressed;

    // ANIMATIONS
    anim.SetBool("Aiming", Aiming);

    if (controller != null)
        controller.strafe = Aiming;

    // CAMERA
    float targetVerticalArmLength =
        Aiming ? aimVerticalArmLength : defaultVerticalArmLength;

    float targetSide =
        Aiming ? aimCameraSide : defaultCameraSide;

    float targetDistance =
        Aiming ? aimCameraDistance : defaultCameraDistance;

    cm_camera.VerticalArmLength = Mathf.Lerp(
        cm_camera.VerticalArmLength,
        targetVerticalArmLength,
        cameraTransitionSpeed * Time.deltaTime);

    cm_camera.CameraSide = Mathf.Lerp(
        cm_camera.CameraSide,
        targetSide,
        cameraTransitionSpeed * Time.deltaTime);

    cm_camera.CameraDistance = Mathf.Lerp(
        cm_camera.CameraDistance,
        targetDistance,
        cameraTransitionSpeed * Time.deltaTime);

    // CROSSHAIR
    if (crosshair != null)
        crosshair.SetActive(Aiming);

    // AIM IK
    if (aimIk != null)
    {
        float targetWeight = Aiming ? 1f : 0f;

        aimIk.weight = Mathf.Lerp(
            aimIk.weight,
            targetWeight,
            ikTransitionSpeed * Time.deltaTime);
    }

    // SHOOT
    if (shootInp && Aiming)
    {
        Shoot();
    }
}
    private void Shoot()
    {
        if (!canShoot)
            return;

        if (shootSound != null)
        {
            AudioSource.PlayClipAtPoint(
                shootSound,
                transform.position);
        }

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        if (anim != null)
        {
            anim.CrossFadeInFixedTime(
                shootStateName,
                shootBlendTime);
        }

        StartCoroutine(ResetFireRate());
    }

    private IEnumerator ResetFireRate()
    {
        canShoot = false;

        yield return new WaitForSeconds(fireRate);

        canShoot = true;
    }
}