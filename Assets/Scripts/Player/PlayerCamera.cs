using UnityEngine;
using Unity.Cinemachine;
using PrimeTween;

[RequireComponent(typeof(CinemachineCamera))]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float defaultFOVAdjustTime = 0.25f;
    [SerializeField] private Ease FOVAdjustEase = Ease.OutQuad;

    private CinemachineCamera cam;
    private float baseFOV;
    private float currentFOV => cam.Lens.FieldOfView;
    private Tween FOVTween;

    private void Awake()
    {
        cam = GetComponent<CinemachineCamera>();
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
        if (FOVTween.isAlive)
            FOVTween.Stop();

        if (duration == 0)
            duration = defaultFOVAdjustTime;

        float current = currentFOV;
        FOVTween = Tween.Custom(current, target, duration, value => cam.Lens.FieldOfView = value, FOVAdjustEase);
    }
    public void ResetFOV()
    {
        cam.Lens.FieldOfView = baseFOV;
    }
    public void ResetFOVOverTime()
    {
        SetFOVOverTime(baseFOV);
    }
}
