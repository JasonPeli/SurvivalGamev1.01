using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] UIPanels;

    [SerializeField]
    private ThirdPersonOrbitCamBasic playerCameraScript;
    private float defaultHorizontalAimingSpeed;
    
    private float defaultVerticalAimingSpeed;

    [HideInInspector]
    public bool atLeastOnePanelOpened;

    [Header("Message Ephemere")]

    [SerializeField]
    private Text ephemereMessageText;
    [SerializeField]
    private GameObject ephemereMessagePanel;
    [SerializeField]
    private float messageDuration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        defaultHorizontalAimingSpeed = playerCameraScript.horizontalAimingSpeed;
        defaultVerticalAimingSpeed = playerCameraScript.verticalAimingSpeed;
        ephemereMessagePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        atLeastOnePanelOpened = UIPanels.Any((panel) => panel == panel.activeSelf);
        if (atLeastOnePanelOpened)
        {
            playerCameraScript.horizontalAimingSpeed = 0;
            playerCameraScript.verticalAimingSpeed = 0;
        }
        else
        {
            playerCameraScript.horizontalAimingSpeed = defaultHorizontalAimingSpeed;
            playerCameraScript.verticalAimingSpeed = defaultVerticalAimingSpeed;
        }
    }

    public void ShowEphemereMessage(string message)
    {
        StartCoroutine(ShowMessageCoroutine(message));
    }

    private IEnumerator ShowMessageCoroutine(string message)
    {
        ephemereMessageText.text = message;
        ephemereMessagePanel.SetActive(true);
        yield return new WaitForSeconds(messageDuration);
        ephemereMessagePanel.SetActive(false);
    }
}
