using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;

public class CEUtilities : MonoBehaviour
{
    static public CEUtilities Singleton;
    GameObject BigRig;
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
        Head = VRTK_DeviceFinder.DeviceTransform(VRTK_DeviceFinder.Devices.Headset);
        Transform t = Head.parent;
        while (!t.name.Contains("CameraRig")) {
            if (t.parent != null) {
                t = t.parent;
            }
            else {
                break;
            }
        }
        if(!t.name.Contains("OVR")){
            isVIVE = true;
        }
        BigRig = t.gameObject;
    }
    public GameObject GetHeadObject() {
        return Head.gameObject;
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

    public float ViewAngleFromCameraForwardToTarget(Vector3 target) {
        Vector3 TargetDir = Vector3.ProjectOnPlane(target - Head.position, Vector3.up);
        Vector3 HeadDir = Vector3.ProjectOnPlane(Head.forward, Vector3.up);
        float targetY = Quaternion.FromToRotation(TargetDir, HeadDir).eulerAngles.y;
        return targetY;
    }
    public void RestartScene() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
