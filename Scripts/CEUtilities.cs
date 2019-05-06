using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CEUtilities : MonoBehaviour
{
    static public CEUtilities Singleton;
    GameObject BigRig;
    [SerializeField]
    Transform Head;

    public bool isVIVE;
    void Awake() {
        if (Singleton != null) {
            Debug.LogError("More than one utility instance");
        }
        Singleton = this;
        if (BigRig == null) {
            BigRig = gameObject;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // Head = VRTK_DeviceFinder.DeviceTransform(VRTK_DeviceFinder.Devices.Headset);
        // Transform t = Head.parent;
        // while (!t.name.Contains("CameraRig")) {
        //     if (t.parent != null) {
        //         t = t.parent;
        //     }
        //     else {
        //         break;
        //     }
        // }
        // if(!t.name.Contains("OVR")){
        //     isVIVE = true;
        // }
        // BigRig = t.gameObject;
    }
    public GameObject GetHeadObject() {
        return Camera.main.gameObject;
        // return Head.gameObject;
    }
    public GameObject GetCameraRig() {
        return BigRig;
    }
    public Vector3 GetHeadsetPos() {
        return Head.position;
    }
    public void ForceLook(Vector3 target, float offset = 0f) {
        Vector3 TargetDir = Vector3.ProjectOnPlane(target - Head.position, Vector3.up);
        Vector3 HeadDir = Vector3.ProjectOnPlane(Head.forward, Vector3.up);
        float targetY = Quaternion.FromToRotation(HeadDir, TargetDir).eulerAngles.y + offset;
        BigRig.transform.Rotate(0, targetY, 0);
    }

    public float ViewAngleFromCameraForwardToTarget(Vector3 target, Camera cam = null) {
        // Debug.Log(Head);
        Transform HeadInUse = Head;
        if (cam != null) {
            HeadInUse = cam.transform;
        }
        Vector3 TargetDir = Vector3.ProjectOnPlane(target - HeadInUse.position, Vector3.up);
        Vector3 HeadDir = Vector3.ProjectOnPlane(HeadInUse.forward, Vector3.up);
        float targetY = Quaternion.FromToRotation(TargetDir, HeadDir).eulerAngles.y;
        if (targetY > 180) {
            targetY -= 360;
        }
        return targetY;
    }

    /// Will return a bigger value when your neck is at a uncomfortable angle
    public float ToleranceBasedOnComfort(Camera cam) {
        Vector3 parentF = Vector3.forward;
        if (cam.transform.parent != null) {
            parentF = cam.transform.parent.forward;
        }
        Vector3 parentA = Vector3.ProjectOnPlane(parentF, Vector3.up);
        Vector3 selfA = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up);
        float targetY = Quaternion.FromToRotation(parentA, selfA).eulerAngles.y;
        if (targetY > 180) {
            targetY -= 360;
        }
        return 1f + Mathf.Abs(targetY) / 45f;
        // return 1f;
    }
    public float AngleBetweenOriginalTargetAndNewPosition(Vector3 target, Camera cam, Vector3 nowTarget) {
        Transform HeadInUse = Head;
        if (cam != null) {
            HeadInUse = cam.transform;
        }
        Vector3 TargetDir = Vector3.ProjectOnPlane(target - HeadInUse.position, Vector3.up);
        Vector3 HeadDir = Vector3.ProjectOnPlane(nowTarget - HeadInUse.position, Vector3.up);
        float targetY = Quaternion.FromToRotation(TargetDir, HeadDir).eulerAngles.y;
        if (targetY > 180) {
            targetY -= 360;
        }
        return targetY;
    }
    public void RestartScene() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
