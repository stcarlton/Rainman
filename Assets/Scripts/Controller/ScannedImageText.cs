using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using System;

public class ScannedImageText : MonoBehaviour
{
    ARTrackedImage _trackedImage;
    TextMeshProUGUI _headerText;

    void Awake()
    {
        _trackedImage = GetComponent<ARTrackedImage>();
        _headerText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        _headerText.text = Math.Round(_trackedImage.transform.position.x,2).ToString() + " "
             + Math.Round(_trackedImage.transform.position.y, 2).ToString() + " "
             + Math.Round(_trackedImage.transform.position.z, 2).ToString();
    }
}