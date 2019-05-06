using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class CEAdvSync : MonoBehaviour
{
    [System.Serializable]
    public class CESyncObj {
        public GameObject obj;
        public GameObject relativeTo;
    }
    public List<CESyncObj> SyncList;
    public int ObjInchargeIdx = 0;
    [SerializeField]
    bool runOnce = false;

    void Sync() {
        if (SyncList.Count == 0 || ObjInchargeIdx >= SyncList.Count)
            return;
        if (SyncList[ObjInchargeIdx].obj == null || SyncList[ObjInchargeIdx].relativeTo == null) {
            return;
        }
        var objWT = SyncList[ObjInchargeIdx].obj.transform;
        var objWorldMat = Matrix4x4.TRS(objWT.position, objWT.rotation, objWT.localScale);
        objWorldMat = SyncList[ObjInchargeIdx].relativeTo.transform.worldToLocalMatrix * objWorldMat;
        for (int i = 0; i < SyncList.Count; i++) {
            if (i == ObjInchargeIdx)
                continue;
            if (SyncList[i].obj == null || SyncList[i].relativeTo == null) {
                continue;
            }
            var tarWT = SyncList[i].relativeTo.transform.localToWorldMatrix * objWorldMat; // * ;
            FromMatrix(SyncList[i].obj.transform, tarWT);
        }
    }
    void LateUpdate()
    {
        if (!runOnce)
            Sync();
    }
    private void Awake() {
        if (runOnce && Application.isPlaying) {
            Sync();
        }
    }
    public static Quaternion ExtractRotation(Matrix4x4 matrix)
    {
        Vector3 forward;
        forward.x = matrix.m02;
        forward.y = matrix.m12;
        forward.z = matrix.m22;
 
        Vector3 upwards;
        upwards.x = matrix.m01;
        upwards.y = matrix.m11;
        upwards.z = matrix.m21;
 
        return Quaternion.LookRotation(forward, upwards);
    }
 
    public static Vector3 ExtractPosition(Matrix4x4 matrix)
    {
        Vector3 position;
        position.x = matrix.m03;
        position.y = matrix.m13;
        position.z = matrix.m23;
        return position;
    }
 
    public static Vector3 ExtractScale(Matrix4x4 matrix)
    {
        Vector3 scale;
        scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
        scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
        scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
        return scale;
    }
    public static void FromMatrix(Transform transform, Matrix4x4 matrix)
    {
        transform.localScale = ExtractScale(matrix);
        transform.rotation = ExtractRotation(matrix);
        transform.position = ExtractPosition(matrix);
    }
}
