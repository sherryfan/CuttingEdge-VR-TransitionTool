using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CECameraSlaveCam : MonoBehaviour
{
    [HideInInspector]
    public RenderTexture targetTexture;
    [HideInInspector]
    public bool ConstructWorldPos = false;
    [HideInInspector]
    public bool IsSweep = false;
    public float SweepTime = 0.6f;
    // [HideInInspector]
    public Transform Scanner;
    // [HideInInspector]
    public Material EffectMaterial;
    [HideInInspector]
    public CECameraBrain brain;
    public float ScanDistance;
    public float SpeedMultiplier = 1f;

    private Camera _camera;

    // Demo Code
    bool spreadCtrScanning;
	Matrix4x4 leftEye, rightEye, leftToWorld, rightToWorld;
    IEnumerator sweep() {
        float elapse = 0f;
        while (elapse <= SweepTime) {
            elapse += Time.deltaTime;
            EffectMaterial.SetFloat("_Tweener", Mathf.Clamp01(elapse / SweepTime));
            yield return null;
        }
        spreadCtrScanning = false;
    }
    public void SetSweep(Vector3 dir, bool useCamDir) {
        EffectMaterial.SetVector("_WorldSpaceViewDir", dir);
        if (useCamDir)
            EffectMaterial.SetVector("_WorldSpaceViewDir", _camera.transform.forward);
        EffectMaterial.SetVector("_WorldSpaceScannerPos", _camera.transform.position);
    }
    public void TriggerTransition() {
        if (IsSweep) {
            StartCoroutine(sweep());
            EffectMaterial.SetVector("_WorldSpaceViewDir", _camera.transform.forward);
            EffectMaterial.SetVector("_WorldSpaceScannerPos", _camera.transform.position);
        }
        else if (!spreadCtrScanning) {
            spreadCtrScanning = true;
            ScanDistance = 0;
        }
    }
    void Update()
    {
        if (spreadCtrScanning)
        {
            ScanDistance += Time.deltaTime * SpeedMultiplier;
        }

    }
    // End Demo Code

    void OnEnable()
    {
        _camera = GetComponent<Camera>();
        _camera.depthTextureMode = DepthTextureMode.Depth;
    }
    private void OnPreRender() // This is just a Later-Than-Late Update
    {
        if (ConstructWorldPos) {
            if (_camera.stereoEnabled)
            {
                // Left and Right Eye inverse View Matrices
                leftToWorld = _camera.GetStereoViewMatrix(Camera.StereoscopicEye.Left).inverse;
                rightToWorld = _camera.GetStereoViewMatrix(Camera.StereoscopicEye.Right).inverse;
                EffectMaterial.SetMatrix("_LeftEyeToWorld", leftToWorld);
                EffectMaterial.SetMatrix("_RightEyeToWorld", rightToWorld);

                leftEye = _camera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
                rightEye = _camera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);

                // Compensate for RenderTexture...
                leftEye = GL.GetGPUProjectionMatrix(leftEye, true).inverse;
                rightEye = GL.GetGPUProjectionMatrix(rightEye, true).inverse;
                leftEye[1, 1] *= -1;
                rightEye[1, 1] *= -1;

                EffectMaterial.SetMatrix("_LeftEyeProjection", leftEye);
                EffectMaterial.SetMatrix("_RightEyeProjection", rightEye);
            }
            else {
                leftToWorld = _camera.GetStereoViewMatrix(Camera.StereoscopicEye.Left).inverse;
                EffectMaterial.SetMatrix("_LeftEyeToWorld", leftToWorld);
                leftEye = _camera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
                // Compensate for RenderTexture...
                leftEye = GL.GetGPUProjectionMatrix(leftEye, true).inverse;
                leftEye[1, 1] *= -1;
                EffectMaterial.SetMatrix("_LeftEyeProjection", leftEye);
            }
        }
        
    }


    // [ImageEffectOpaque]
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (ConstructWorldPos){
            if (brain.transitionStyle == CECameraBrain.TransitionStyle.spread_center) {
                EffectMaterial.SetVector("_WorldSpaceScannerPos", Scanner.position);
                EffectMaterial.SetFloat("_ScanDistance", ScanDistance * ScanDistance);
            }
            if (brain.transitionStyle == CECameraBrain.TransitionStyle.spread_dir) {
                EffectMaterial.SetVector("_WorldSpaceScannerDir", Scanner.forward);
                EffectMaterial.SetFloat("_ScanDistance", Vector3.Dot(Scanner.position, Scanner.forward));
                // Debug.Log(Vector3.Dot(Scanner.position, Scanner.forward));
            }
            
            Graphics.Blit(src, targetTexture, EffectMaterial);
        }
        else {
            Graphics.Blit(src, targetTexture);
        }
        Graphics.Blit(src, dst);
    }
}
