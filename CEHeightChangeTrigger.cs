using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Sherry Fan
[RequireComponent(typeof(CESceneHelper))]
public class CEHeightChangeTrigger : MonoBehaviour
{
    static public CEHeightChangeTrigger Singleton;
    [SerializeField]
    CESceneHelper sceneHelper;
    public enum TriggerDirection
    {
        Up, Down
    }
    public TriggerDirection triggerDirection = TriggerDirection.Down;
    [SerializeField]
    float heightTriggerDistance = 0.5f;

    [SerializeField]
    string NextScene = "GS4Road1";
    [SerializeField]
    GameObject Head;
    float originalHeight; // Z value of CenterEyeAnchor
    bool headTracked;
    // Start is called before the first frame update
    void Start()
    {
        // prepare the next scene at start
        sceneHelper = GetComponent<CESceneHelper>();
        sceneHelper.PrepareScene(NextScene);
        StartCoroutine(GetHeadHeight());
    }
    IEnumerator GetHeadHeight()
    {
        yield return new WaitForEndOfFrame();
        originalHeight = Head.transform.localPosition.y;
        //print("original height " + originalHeight);
        headTracked = true;
    }
    public void LoadScene()
    {
        sceneHelper.LoadScene();
    }
    // Update is called once per frame
    void Update()
    {
        if (headTracked)
        {
            //print(Head.transform.localPosition.y);
            switch (triggerDirection)
            {
                case TriggerDirection.Up:
                    if ((Head.transform.localPosition.y - originalHeight) > heightTriggerDistance)
                    {
                        LoadScene();
                    }
                    break;
                case TriggerDirection.Down:
                    if ((originalHeight - Head.transform.localPosition.y) > heightTriggerDistance)
                    {
                        LoadScene();
                    }
                    break;
            }

        }
    }
}
