using System.Collections.Generic;
using UnityEngine;

public enum CameraLocations 
{
    StageOne,
    StageTwo,
    StageThree,
}

public class CameraManager : MonoBehaviour
{
    private Camera mainCamera;

    public static CameraManager Instance;

    public Dictionary<CameraLocations, Vector2> locationsList = new Dictionary<CameraLocations, Vector2> { };

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
        locationsList.Add(CameraLocations.StageOne, new Vector2(0, 0));
        locationsList.Add(CameraLocations.StageTwo, new Vector2(0, -10));
        locationsList.Add(CameraLocations.StageThree, new Vector2(21, -10));
    }

    public void ChangeStage(CameraLocations stage)
    {
        Vector2 location;

        if (Instance.locationsList.TryGetValue(stage, out location))
        {
            Vector3 newCameraPosition = new Vector3(location.x, location.y, mainCamera.transform.position.z);
            mainCamera.transform.position = newCameraPosition;
        }
    }
}