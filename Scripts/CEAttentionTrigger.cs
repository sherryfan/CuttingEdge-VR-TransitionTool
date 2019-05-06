using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CEAttentionTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public enum TriggerType
    {
        OnInteractionEnter, OnInteractionExit
    }
    public TriggerType triggerType;

    [SerializeField]
    private GameObject scene1, scene2;
    [SerializeField]
    private float defaultTriggerDuration;
    [SerializeField]
    private CECameraBrain _CameraBrain;

    [SerializeField]
    private CEInteractable targetInteractable;
    [SerializeField]
    private float blendSpeed = 0.5f;
[SerializeField]
private List<GameObject> listToDisappear, listToAppear;

    public bool dissolving;

    static float t = 0.0f;

    public static CEAttentionTrigger instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        dissolving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (dissolving)
        {
            if (_CameraBrain.BlendProcess == 1)
            {
                // blending is complete
                dissolving = false;
                foreach (GameObject e in listToDisappear){
                    e.SetActive(false);
                }
                foreach (GameObject e in listToAppear){
                    e.SetActive(true);
                }
            }
            else
            {
                _CameraBrain.BlendProcess = Mathf.Lerp(0, 1, t);
                t += blendSpeed * Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


    }

    public IEnumerator OnCutTriggered(bool isLookAway)
    {
        if (triggerType == TriggerType.OnInteractionEnter && _CameraBrain.BlendProcess == 0)
        {
            yield return new WaitForSeconds(defaultTriggerDuration);
            dissolving = true;
            CESoundManager.Singleton.StartTransition();
        }
        else
        {// Not Time-based
            if (isLookAway && _CameraBrain.BlendProcess == 0)
            {
                dissolving = true;
                CESoundManager.Singleton.StartTransition();
            }

            yield return null;
        }
    }


}
