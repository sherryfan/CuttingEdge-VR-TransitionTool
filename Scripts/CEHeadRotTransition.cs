using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEHeadRotTransition : MonoBehaviour
{
    [SerializeField]
    GameObject m_Camera, target;
    public bool CanTransition = false;
    [SerializeField]
    float fullAngle = 80f, gazeThreshold = 5f, minSpeed = 10f, maxSpeed = 60f, normalSpeed = 40f;
    [SerializeField]
    bool useAnimationClip = false;
    [SerializeField]
    bool targetGoesLeft = false;
    float nowSpeed;
    public bool done = false, started = false;
    public float process = 0f;
    Vector3 oPoint, originalPos;
    Camera m_Camera_Cam;
    [SerializeField]
    CECameraBrain brain;
    [SerializeField]
    AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
    public bool giveUpControl = false;
    public delegate void eventVoid();
    public eventVoid StartTransition;
    float dirMult {
        get {
            if (targetGoesLeft) return -1;
            else return 1;
        }
    }
    public float SpeedPercentage {
        get {
            return nowSpeed / normalSpeed;
        }
    }
    // Should only be used when using animation clip
    public void SetDone(bool val) {
        done = val;
    }
    // Start is called before the first frame update
    void Start()
    {
        originalPos = target.transform.position;
        oPoint = m_Camera.transform.position;
        nowSpeed = minSpeed;
        m_Camera_Cam = m_Camera.GetComponent<Camera>();
    }
    bool motionShouldHaveAttention(float lookAngle, float objectAngle) {
        float angle = lookAngle - objectAngle;
        float tolerance = CEUtilities.Singleton.ToleranceBasedOnComfort(m_Camera_Cam);
        // if (angle >= 0) {
        //     if (angle < 3 * gazeThreshold) {
        //         return true;
        //     }
        // }
        // else {
        //     if (-angle < gazeThreshold) {
        //         return true;
        //     }
        // }
        return Mathf.Abs(angle) <= tolerance * gazeThreshold;
        // return false;
    }
    // Update is called once per frame
    void Update()
    {
        if (done)
            return;
        if (CanTransition)
        {
            float viewAngleToStart = CEUtilities.Singleton.ViewAngleFromCameraForwardToTarget(originalPos, m_Camera_Cam);
            float objAngleToStart = CEUtilities.Singleton.AngleBetweenOriginalTargetAndNewPosition(originalPos, m_Camera_Cam, target.transform.position);
            // Debug.Log("HeadRotT: " + viewAngleToStart);
            float processAngle = fullAngle * process;
            if (!started && motionShouldHaveAttention(viewAngleToStart, 0))
            {
                started = true;
                if (StartTransition != null) {
                    StartTransition.Invoke();
                }
            }
            if (started) {
                float targetSpeed;
                if (giveUpControl || motionShouldHaveAttention(viewAngleToStart, objAngleToStart))
                {
                    targetSpeed = normalSpeed;
                }
                else {
                    if (viewAngleToStart < objAngleToStart) {
                        targetSpeed = minSpeed;
                    }
                    else {
                        targetSpeed = maxSpeed;
                    }
                }
                nowSpeed = Mathf.Lerp(nowSpeed, targetSpeed, 0.1f);
                processAngle += nowSpeed * Time.deltaTime;
                if (!useAnimationClip) {
                    if (processAngle > fullAngle)
                        done = true;
                    process = Mathf.Clamp01(processAngle / fullAngle);
                }
                
            }
            
        }
        if (brain != null) {
            brain.BlendProcess = curve.Evaluate(process);
        }
    }
}
