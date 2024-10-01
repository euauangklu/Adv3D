using System;
using System.Collections;
using System.Collections.Generic;
using GDD.Sinagleton;
using UnityEngine;

public class CaptureCameraManager : CanDestroy_Sinagleton<CaptureCameraManager>
{
    private Camera _captureCam;

    public Camera captureCam
    {
        get => _captureCam;
    }

    private void Awake()
    {
        _captureCam = GetComponent<Camera>();
    }

    public void SetEnableCamera(bool isEnable)
    {
        _captureCam.enabled = isEnable;
    }
}
