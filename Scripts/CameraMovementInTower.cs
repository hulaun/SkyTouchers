using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementInTower : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform endPointTransform1;
    [SerializeField] Transform endPointTransform2;

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
        if (playerPosition.x > endPointTransform1.position.x && playerPosition.x < endPointTransform2.position.x)
        {
            MoveWithPlayer();
        }
        else
        {
            if(Vector3.Distance(cameraPosition, endPointTransform1.position)< Vector3.Distance(cameraPosition, endPointTransform2.position))
            {
                cameraPosition = new Vector3(endPointTransform1.position.x, cameraPosition.y, cameraPosition.z);
                transform.position = cameraPosition;
            }
            else
            {
                cameraPosition = new Vector3(endPointTransform2.position.x, cameraPosition.y, cameraPosition.z);
                transform.position = cameraPosition;
            }
            
        }
    }
    void MoveWithPlayer()
    {
        cameraPosition = new Vector3(playerTransform.position.x, cameraPosition.y, cameraPosition.z);
        transform.position = cameraPosition;
    }
}
