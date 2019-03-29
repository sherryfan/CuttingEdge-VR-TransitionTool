using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
[RequireComponent(typeof(Camera))]
public class CECameraBrain : MonoBehaviour
{
    Camera brain;
    [SerializeField]
    Camera CamA, CamB;
    [SerializeField, Range(0f, 1f)]
    public float BlendProcess = 0f;
    RenderTexture texA, texB;
    [SerializeField]
    Material BlendingMatNormal, BlendingMatAlpha;
    Material tweenMat;
    
    [SerializeField]
    Material SweepMat, SpreadMat;
    public enum TransitionStyle {
        blend, sweep, spread
    };
    public TransitionStyle transitionStyle = TransitionStyle.blend;
    [SerializeField]
    RenderTexture testTexure;
    [SerializeField]
    Transform CamATransitionCenter;
    [SerializeField]
    bool ReadFromPref = false;
    CECameraSlaveCam slaveCamA;
    RenderTexture SetBlendCamera(Camera c) {
        // RenderTexture ret = new RenderTexture(XRSettings.eyeTextureDesc);
        RenderTexture ret = new RenderTexture(XRSettings.eyeTextureDesc.width, XRSettings.eyeTextureDesc.height, 24);
        c.gameObject.GetComponent<CECameraSlaveCam>().targetTexture = ret;
        ret.vrUsage = VRTextureUsage.TwoEyes;
        // c.targetTexture = ret;
        // RenderTexture ret = c.targetTexture;
        return ret;
    }
    bool spreadScanning = false;
    void Awake()
    {
        if (ReadFromPref) {
            string t = PlayerPrefs.GetString("GS2Trans", "none");
            if (t != "none") {
                transitionStyle = (TransitionStyle)System.Enum.Parse(typeof(TransitionStyle), t);
            }
        }
        brain = GetComponent<Camera>();
        brain.cullingMask = 0;
        brain.clearFlags = CameraClearFlags.Nothing;
        texA = SetBlendCamera(CamA);
        CamA.depth = -10;
        texB = SetBlendCamera(CamB);
        CamB.depth = -9;
        slaveCamA = CamA.GetComponent<CECameraSlaveCam>();
        switch (transitionStyle) {
            case TransitionStyle.blend:
                tweenMat = new Material(BlendingMatNormal);
                slaveCamA.ConstructWorldPos = false;
                slaveCamA.IsSweep = false;
                break;
            case TransitionStyle.sweep:
                tweenMat = new Material(BlendingMatAlpha);
                slaveCamA.ConstructWorldPos = true;
                slaveCamA.IsSweep = true;
                slaveCamA.EffectMaterial = new Material(SweepMat);
                slaveCamA.EffectMaterial.SetFloat("_Tweener", 0);
                slaveCamA.ScannerOrigin = CamA.transform;
                break;
            case TransitionStyle.spread:
                tweenMat = new Material(BlendingMatAlpha);
                slaveCamA.ConstructWorldPos = true;
                slaveCamA.IsSweep = false;
                slaveCamA.EffectMaterial = new Material(SpreadMat);
                slaveCamA.ScannerOrigin = CamATransitionCenter;
                break;
        }
        
        tweenMat.SetTexture("_TexA", texA);
        tweenMat.SetTexture("_TexB", texB);
    }
    bool done = false;
    private void Update() {
        tweenMat.SetFloat("_Tweener", BlendProcess);
        if (transitionStyle != TransitionStyle.blend) {
            if (BlendProcess > 0.5f && !done) {
                slaveCamA.TriggerTransition();
                done = true;
            }
        }
    }
    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        // Graphics.Blit(src, dest, tweenMat);
        // Graphics.Blit(src, dest, tweenMat);
        Graphics.Blit(src, dest, tweenMat);
        // Graphics.Blit(src, dest);
    }
}
