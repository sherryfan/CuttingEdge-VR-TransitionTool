using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class CESync : MonoBehaviour
{
    public List<GameObject> SyncList;

    // Update is called once per frame
    void Update()
    {
        foreach (var item in SyncList)
        {
            if (item != null) {
                item.transform.localPosition = transform.localPosition;
                item.transform.localRotation = transform.localRotation;
                item.transform.localScale = transform.localScale;
            }
        }
    }
}
