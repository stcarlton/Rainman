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
    Camera _mainCamera;
    Card _thisCard;

    void Awake()
    {
        _trackedImage = GetComponent<ARTrackedImage>();
        _headerText = GetComponentInChildren<TextMeshProUGUI>();
        _mainCamera = Camera.main;
    }

    void Update()
    {
        _headerText.text = Math.Round(Vector3.Distance(_mainCamera.transform.position, transform.position),2).ToString();
    }
}