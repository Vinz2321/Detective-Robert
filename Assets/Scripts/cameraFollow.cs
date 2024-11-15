using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow: MonoBehaviour
{

    public Transform player;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y,transform.position.z);
        Vector3 SmoothedPosition = Vector3.Lerp(transform.position,desiredPosition,smoothSpeed);
        transform.position = SmoothedPosition;

        transform.LookAt(player);
    }
}
