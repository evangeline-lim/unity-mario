using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player; // Mario's Transform
    public Transform endLimit; // GameObject that indicates end of map
    private float offset; // initial x-offset between camera and Mario
    private float startX; // smallest x-coordinate of the Camera
    private float endX; // largest x-coordinate of the camera
    private float viewportHalfWidth;
    
    // Start is called before the first frame update
    void Start()
    {
        // get coordinate of the bottomleft of the viewport
        // z doesn't matter since the camera is orthographic
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        viewportHalfWidth = Mathf.Abs(bottomLeft.x - this.transform.position.x);

        offset = this.transform.position.x - player.position.x;
        startX = this.transform.position.x;
        endX = endLimit.transform.position.x - viewportHalfWidth;
        
    }

    // Update is called once per frame
    void Update()
    {
        float desiredX = player.position.x + offset;
        // check if desiredX is within startX and endX
        if (desiredX > startX && desiredX < endX)
        this.transform.position = new Vector3(desiredX, this.transform.position.y, this.transform.position.z);
        
    }
}
