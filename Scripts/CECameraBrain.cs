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
    Material MainCamMat_Tween, MainCamMat_AlphaAdd;
    Material tweenMat;
    
    [SerializeField]
    Material SlaveMat_Sweep, SlaveMat_Spread_Cen, SlaveMat_Spread_Dir, SlaveMat_Spread_Inverse;
    public bool SpreadCenterInverse = false;
    // Shader SpreadCenterInverseShader;
    public enum TransitionStyle {
        blend, sweep, spread_center, spread_dir
    };
    public TransitionStyle transitionStyle = TransitionStyle.blend;
    // [SerializeField]
    // RenderTexture testTexure;
    public Transform CamATransitionCenter;
    [SerializeField]
    bool ReadFromPref = false;
    bool triggered = false;
    CECameraSlaveCam activeSlaveCam;
    RenderTexture SetBlendCamera(Camera c) {
        // RenderTexture ret = new RenderTexture(XRSettings.eyeTextureDesc);
        RenderTexture ret = new RenderTexture(XRSettings.eyeTextureDesc.width, XRSettings.eyeTextureDesc.height, 24);
        // Debug.Log(ret.height);
        c.gameObject.GetComponent<CECameraSlaveCam>().targetTexture = ret;
        c.gameObject.GetComponent<CECameraSlaveCam>().brain = this;
        ret.vrUsage = VRTextureUsage.TwoEyes;
        // c.targetTexture = ret;
        // RenderTexture ret = c.targetTexture;
        return ret;
    }
    public void SetSpeedMultiplier(float f) {
        CamA.GetComponent<CECameraSlaveCam>().SpeedMultiplier = f;
        CamB.GetComponent<CECameraSlaveCam>().SpeedMultiplier = f;
    }
    public Camera GetMainDisplayCamera() {
        if (transitionStyle == TransitionStyle.blend) {
            if (BlendProcess < 0.5)
                return CamA;
            else
            {
                return CamB;
            }
        }
        else if (transitionStyle == TransitionStyle.spread_dir || transitionStyle == TransitionStyle.sweep) {
            return CamB;
        }
        else {
            if (triggered)
                return CamB;
            else
            {
                return CamA;
            }
        }
    }
    bool spreadScanning = false;
    public void SwitchCamera(TransitionStyle style, bool doNotActuallySwitch = false) {
        if (!doNotActuallySwitch) {
            var tmpTex = texA;
            texA = texB;
            texB = tmpTex;
            var tmpCam = CamA;
            CamA = CamB;
            CamB = tmpCam;
        }
        BlendProcess = 1 - BlendProcess;
        transitionStyle = style;
        triggered = false;
        initializeMaterials();
        tweenMat.SetFloat("_Tweener", BlendProcess);
    }
    void initializeMaterials() {
        CamA.depth = -10;
        CamB.depth = -9;
        var slaveCamA = CamA.GetComponent<CECameraSlaveCam>();
        var slaveCamB = CamB.GetComponent<CECameraSlaveCam>();
        activeSlaveCam = slaveCamA;
        slaveCamB.ConstructWorldPos = false;
        slaveCamB.IsSweep = false;
        slaveCamB.EffectMaterial = null;
        switch (transitionStyle) {
            case TransitionStyle.blend:
                tweenMat = new Material(MainCamMat_Tween);
                slaveCamA.ConstructWorldPos = false;
                slaveCamA.IsSweep = false;
                break;
            case TransitionStyle.sweep:
                tweenMat = new Material(MainCamMat_AlphaAdd);
                slaveCamA.ConstructWorldPos = true;
                slaveCamA.IsSweep = true;
                slaveCamA.EffectMaterial = new Material(SlaveMat_Sweep);
                slaveCamA.EffectMaterial.SetFloat("_Tweener", 0);
                slaveCamA.Scanner = CamA.transform;
                break;
            case TransitionStyle.spread_center:
                tweenMat = new Material(MainCamMat_AlphaAdd);
                slaveCamA.ConstructWorldPos = true;
                slaveCamA.IsSweep = false;
                // slaveCamA.EffectMaterial = new Material(SlaveMat_Spread_Cen);
                if (SpreadCenterInverse) {
                    slaveCamA.EffectMaterial = new Material(SlaveMat_Spread_Inverse);
                }
                else {
                    slaveCamA.EffectMaterial = new Material(SlaveMat_Spread_Cen);
                }
                slaveCamA.Scanner = CamATransitionCenter;
                break;
            case TransitionStyle.spread_dir:
                tweenMat = new Material(MainCamMat_AlphaAdd);
                slaveCamA.ConstructWorldPos = true;
                slaveCamA.IsSweep = false;
                slaveCamA.EffectMaterial = new Material(SlaveMat_Spread_Dir);
                slaveCamA.Scanner = CamATransitionCenter;
                break;
        }
        tweenMat.SetTexture("_TexA", texA);
        tweenMat.SetTexture("_TexB", texB);
    }
    public void SetSweep(bool useCamDir) {
        if (useCamDir) {
            activeSlaveCam.SetSweep(Vector3.forward, true);
        }
        else {
            activeSlaveCam.SetSweep((CamATransitionCenter.position - CamA.transform.position).normalized, false);
        }
    }
    void Awake()
    {
        // SpreadCenterInverseShader = Shader.Find("Hidden/ScannerActiveCamInverse");
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
        texB = SetBlendCamera(CamB);
        initializeMaterials();
    }
    bool done = false;
    private void LateUpdate() {
        if (transitionStyle == TransitionStyle.blend)
            tweenMat.SetFloat("_Tweener", BlendProcess);
        if (transitionStyle == TransitionStyle.sweep) {
            activeSlaveCam.EffectMaterial.SetFloat("_Tweener", BlendProcess);
        }
    }
    public void TriggerTransition() {
        CamA.GetComponent<CECameraSlaveCam>().TriggerTransition();
        triggered = true;
    }
    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        // Graphics.Blit(src, dest, tweenMat);
        // Graphics.Blit(src, dest, tweenMat);
        Graphics.Blit(src, dest, tweenMat);
        // Graphics.Blit(src, dest);
    }
}
