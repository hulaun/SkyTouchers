using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform endPointTransform;

    Vector3 cameraPosition;
    Vector3 playerPosition;
    // Start is called before the first frame update
    private void Awake()
    {
        cameraPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = playerTransform.position;
        if (playerPosition.x < endPointTransform.position.x)
        {
            MoveWithPlayer();
        }
        else
        {
            cameraPosition = new Vector3(endPointTransform.position.x, cameraPosition.y, cameraPosition.z);
            transform.position = cameraPosition;
        }
    }
    void MoveWithPlayer()
    {
        cameraPosition = new Vector3(playerTransform.position.x, cameraPosition.y, cameraPosition.z);
        transform.position = cameraPosition;
    }
}
