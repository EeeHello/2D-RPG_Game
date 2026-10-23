using System;
using UnityEngine;

[System.Serializable]
public class ParallaxLayer 
{
    [SerializeField] private Transform background;
    [SerializeField] private float parallaxMultiplier;
    [SerializeField] private float imageWidthOffset = 10f;

    private float imageFullWidth;
    private float imageHalfWidth;

    public void CalculateImageWidth()
    {
        imageFullWidth  = background.GetComponent<SpriteRenderer>().bounds.size.x;
        imageHalfWidth = imageFullWidth / 2;
    }

    public void Move(float distanceToMOve)
    {
        background.position += Vector3.right * (distanceToMOve * parallaxMultiplier);
    }

    public void LoopBackground (float camLeftEdge, float camRightEdge)
    {
        float imageRightEdge = (background.position.x + imageHalfWidth) - imageWidthOffset;
        float imageLeftEdge  = (background.position.x - imageHalfWidth) + imageWidthOffset;

        if (imageRightEdge < camLeftEdge)
        {
            background.position += Vector3.right * imageFullWidth;
        }
        else if (imageLeftEdge > camRightEdge)
        {
            background.position += Vector3.left * -imageFullWidth;
        }

    }
}
