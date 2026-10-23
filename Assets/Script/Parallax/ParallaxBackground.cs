using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera mainCamera;
    private float lastCamPositionX;
    private float camHalfWidth;

    [SerializeField] private ParallaxLayer[] backgroundLayers;

    private void Awake()
    {
        mainCamera = Camera.main;
        camHalfWidth  = mainCamera.orthographicSize * mainCamera.aspect;
        InitializeLayers();
    }

    private void FixedUpdate()
    {
        float currentCameraPositionX = mainCamera.transform.position.x;
        float distanceToMove  = currentCameraPositionX - lastCamPositionX;
        lastCamPositionX = currentCameraPositionX;

        float camLeftEdge = currentCameraPositionX - camHalfWidth;
        float camRightEdge = currentCameraPositionX + camHalfWidth;

        foreach(ParallaxLayer layer in backgroundLayers)
        {
            layer.Move(distanceToMove);
            layer.LoopBackground(camLeftEdge, camRightEdge);
        }
    }

    private void InitializeLayers()
    {
        foreach(ParallaxLayer layer in backgroundLayers)
        {
            layer.CalculateImageWidth();
        }
    }
}
