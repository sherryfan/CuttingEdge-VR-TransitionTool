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
    private bool isDissolve;

    //public bool gazeEnter, grabEnter;

    [SerializeField]
    private GameObject scene1, scene2;
    [SerializeField]
    private float defaultTriggerDuration;
    [SerializeField]
    private CECameraBrain _CameraBrain;

    [SerializeField]
    private CEKeyObject targetInteractable;
    [SerializeField]
    private float blendSpeed = 0.5f;


    private bool dissolving;

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

        if (Input.GetKey(KeyCode.Alpha2))
        {
            // GrabTriggerController.instance.isTimeBased = false;
            // GrabTriggerController.instance.isDissolve = false;
            // GrabTriggerController.instance.isTriggeredOnLookAway = true;
        }


    }

    public IEnumerator OnCutTriggered(bool isLookAway)
    {
        if (triggerType == TriggerType.OnInteractionEnter && _CameraBrain.BlendProcess == 0)
        {
            yield return new WaitForSeconds(defaultTriggerDuration);
            Cut();
        }
        else
        {// Not Time-based
            if (isLookAway && _CameraBrain.BlendProcess == 0)
            {
                Cut();
            }

            yield return null;
        }
    }

    private void Cut()
    {
        if (isDissolve)
        {
            dissolving = true;
            CESoundManager.Singleton.StartTransition();

        }
        else
        {
            scene1.SetActive(false);
            scene2.SetActive(true);
        }
    }

}
