using UnityEngine;
using Unity.Cinemachine;
using PrimeTween;
using System;

[RequireComponent(typeof(CinemachineCamera))]
public class PlayerCamera : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Transform cameraTargetPistol;
    [SerializeField] private Transform cameraTargetShotgun;
    [SerializeField] private Transform heightOffsetRoot;

    [Header("Attributes")]
    [SerializeField] private float defaultFOVAdjustTime = 0.25f;
    [SerializeField] private Ease FOVAdjustEase = Ease.OutQuad;

    [SerializeField] private float defaultHeightAdjustTime = 0.25f;
    [SerializeField] private Ease HeightAdjustEase = Ease.OutQuad;

    private CinemachineCamera cam;
    private float baseFOV;
    private float currentFOV => cam.Lens.FieldOfView;
    private Tween FOVTween;
    private Tween heightTween;

    private void Awake()
    {
        cam = GetComponent<CinemachineCamera>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (cameraTargetPistol.gameObject.activeInHierarchy)
        {
            transform.position = cameraTargetPistol.position;
            transform.rotation = cameraTargetPistol.rotation;
        }
        else if (cameraTargetShotgun.gameObject.activeInHierarchy)
        {
            transform.position = cameraTargetShotgun.position;
            transform.rotation = cameraTargetShotgun.rotation;
        }

    }

    public void SetFOV(float fov)
    {
        baseFOV = fov;
        cam.Lens.FieldOfView = baseFOV;
    }

    public void AdjustFOV(float offset)
    {
        cam.Lens.FieldOfView = baseFOV + offset;
    }

    public void AdjustFOVOverTime(float offset, float duration = 0)
    {
        SetFOVOverTime(baseFOV + offset, duration);
    }

    public void SetFOVOverTime(float target, float duration = 0)
    {
        if (duration == 0)
            duration = defaultFOVAdjustTime;
        float current = currentFOV;

        SetValueOverTime(target, current, duration, FOVAdjustEase, FOVTween, newValue => cam.Lens.FieldOfView = newValue);
    }
    public void ResetFOV()
    {
        cam.Lens.FieldOfView = baseFOV;
    }
    public void ResetFOVOverTime()
    {
        SetFOVOverTime(baseFOV);
    }

    public void SetHeightOffset(float value)
    {
        heightOffsetRoot.transform.localPosition = new Vector3(0, value, 0);
    }

    public void SetHeightOffsetOverTime(float value, float duration = 0)
    {
        if (duration == 0)
            duration = defaultHeightAdjustTime;
        float current = heightOffsetRoot.transform.localPosition.y;

        SetValueOverTime(value, current, duration, HeightAdjustEase, heightTween, SetHeightOffset);
    }

    private void SetValueOverTime(float target, float current, float duration, Ease ease, Tween targetTween, Action<float> setValue)
    {
        if (targetTween.isAlive)
            targetTween.Stop();

        targetTween = Tween.Custom(current, target, duration, setValue, ease);
    }
}
