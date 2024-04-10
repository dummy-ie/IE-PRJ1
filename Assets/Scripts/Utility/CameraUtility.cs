using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraUtility
{
    private static Camera _mainCamera;
    public static Camera mainCamera
    {
        get
        {
            if (!_mainCamera)
                _mainCamera = Camera.main;
            return _mainCamera;
        }
    }

    private static CinemachineVirtualCamera _playerVCam;
    public static CinemachineVirtualCamera playerVCam
    {
        get
        {
            if (!_playerVCam)
                _playerVCam = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CinemachineVirtualCamera>(true);
            return _playerVCam;
        }
    }
}
