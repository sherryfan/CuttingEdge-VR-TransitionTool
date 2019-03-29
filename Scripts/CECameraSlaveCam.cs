using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CECameraSlaveCam : MonoBehaviour
{
    public RenderTexture targetTexture;
    [HideInInspector]
    public bool ConstructWorldPos = false;
    [HideInInspector]
    public bool IsSweep = false;
    public float SweepTime = 0.6f;
    public Transform ScannerOrigin;
    public Material EffectMaterial;
    public float ScanDistance;

    private Camera _camera;

    // Demo Code
    bool spreadScanning;
	Matrix4x4 leftEye, rightEye, leftToWorld, rightToWorld;
    IEnumerator sweep() {
        float elapse = 0f;
        while (elapse <= SweepTime) {
            elapse += Time.deltaTime;
            EffectMaterial.SetFloat("_Tweener", Mathf.Clamp01(elapse / SweepTime + 0.3f));
            yield return null;
        }
        spreadScanning = false;
    }
    public void TriggerTransition() {
        EffectMaterial.SetVector("_WorldSpaceViewDir", _camera.transform.forward);
        if (!spreadScanning) {
            spreadScanning = true;
            if (IsSweep) {
                StartCoroutine(sweep());
            }
        }
            
        ScanDistance = 0;
    }
    void Update()
    {
        if (spreadScanning)
        {
            ScanDistance += Time.deltaTime;

        }

        if (Input.GetKeyDown(KeyCode.C) && ConstructWorldPos)
        {
            EffectMaterial.SetVector("_WorldSpaceViewDir", _camera.transform.forward);
            if (!spreadScanning) {
                spreadScanning = true;
                if (IsSweep) {
                    StartCoroutine(sweep());
                }
            }
                
            ScanDistance = 0;
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
            EffectMaterial.SetVector("_WorldSpaceScannerPos", ScannerOrigin.position);
            EffectMaterial.SetFloat("_ScanDistance", ScanDistance);
            Graphics.Blit(src, targetTexture, EffectMaterial);
        }
        else {
            Graphics.Blit(src, targetTexture);
        }
    }
}
