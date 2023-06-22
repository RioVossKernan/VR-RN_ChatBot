using System.Collections;
using System.Collections.Generic;
using OpenAI;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class InputReader : MonoBehaviour
{

    List<InputDevice> inputDevices = new List<InputDevice>();
    private APIOpenAI api;
    public Transform playerPos;
    public GameObject OpenAIObj;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        InitializeInputReader();
        api = OpenAIObj.GetComponent<APIOpenAI>();

        //OpenAIObj.GetComponent<TextMeshPro>().text = "Waiting...";
    }

    // Update is called once per frame
    void Update()
    {
        if(inputDevices.Count < 2)
        {
            InitializeInputReader();
        }
    }


    void InitializeInputReader()
    {
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, inputDevices);

        foreach (var inputDevice in inputDevices)
        {
            inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
            if(triggerValue > 0.9)
            {
                api.EndRecording();
                //OpenAIObj.GetComponent<TextMeshPro>().text = "Right";
            }


            inputDevice.TryGetFeatureValue(CommonUsages.secondary2DAxis, out Vector2 move);
            playerPos.position += new Vector3(move.x,move.y) * Time.deltaTime * moveSpeed;
        }


        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, inputDevices);

        foreach (var inputDevice in inputDevices)
        {
            inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
            if (triggerValue > 0.9)
            {
                api.StartRecording();
                //OpenAIObj.GetComponent<TextMeshPro>().text = "Left";
            }
        }


    }
}
