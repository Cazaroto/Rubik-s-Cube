using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Cube Manager Reference")]
    public CubeManager cubeManager;

    List<GameObject> pieces = new List<GameObject>(),
                     planes = new List<GameObject>();
    Vector3 localRotation;
    bool cameraDisabled, rotateDisabled;


    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            if (!rotateDisabled)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100))
                {
                    cameraDisabled = true;

                    if (pieces.Count < 2 &&
                        !pieces.Exists(x => x == hit.collider.transform.gameObject) &&
                        hit.transform.parent.gameObject != cubeManager.gameObject)
                    {
                        pieces.Add(hit.collider.transform.parent.gameObject);
                        planes.Add(hit.collider.gameObject);
                    }
                    else if (pieces.Count == 2)
                    {
                        cubeManager.DetectRotation(pieces, planes);
                        rotateDisabled = true;
                    }
                }
            }

            if (!cameraDisabled)
            {
                rotateDisabled = true;
                localRotation.x += Input.GetAxis("Mouse X") * 15;
                localRotation.y += Input.GetAxis("Mouse Y") * -15;
                localRotation.y = Mathf.Clamp(localRotation.y, -90, 90);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            pieces.Clear();
            planes.Clear();
            cameraDisabled = rotateDisabled = false;
        }

        Quaternion qt = Quaternion.Euler(localRotation.y, localRotation.x, 0);
        transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, qt, Time.deltaTime * 15);
    }
}
