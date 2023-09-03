using System.Collections;
using System.Collections.Generic;
using GOAP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanPopup : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    [SerializeField] private TextMeshProUGUI agentNameText;
    [SerializeField] private TextMeshProUGUI goalText;
    [SerializeField] private TextMeshProUGUI planText;
    [SerializeField] private Button closeButton;
    [SerializeField] private LayerMask layerToDetect;

    private Camera mainCamera;
    void Start()
    {
        popup.SetActive(false);
        mainCamera = Camera.main;
        closeButton.onClick.AddListener(OnClose);
    }

    private void OnClose()
    {
        popup.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the camera to the mouse pointer
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Declare a variable to store information about the hit
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit, 50f, layerToDetect))
            {
                // Check if the ray hit a game object
                GameObject hitObject = hit.collider.gameObject;

                // Do something with the hit object
                Debug.Log("Hit: " + hitObject.name);

                // You can perform actions on the hit object here.
                GAgent agent = hitObject.GetComponent<GAgent>();
                agentNameText.text = "Name : " + agent.name;
                goalText.text = "Goals : ";
                foreach (KeyValuePair<SubGoal, int> sg in agent.goals)
                {
                    foreach (KeyValuePair<string, int> sub in sg.Key.sgoals)
                    {
                        goalText.text += "\n" + sub.Key;
                    }

                }
                planText.text = "Plan : ";
                foreach (var action in agent.actions)
                {
                    planText.text += "\n" + action.actionName;
                }
                popup.gameObject.SetActive(true);
            }
        }
    }
}
