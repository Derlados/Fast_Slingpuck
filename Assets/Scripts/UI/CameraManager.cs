using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Canvas can1, can2;
    public Camera camera;
    public float speedSize, speedMove;

    Status cameraStatus = Status.free;

    enum Status
    {
        free,
        zoom,
    }

    private void Start()
    {
        can2.GetComponent<RectTransform>().sizeDelta = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)) * 2;
    }

    private void Update()
    {
        if (cameraStatus == Status.zoom)
        {
            Vector2 temp = Vector2.MoveTowards(camera.transform.position, gameObject.transform.position, Time.deltaTime * speedMove);

            if (temp == (Vector2)gameObject.transform.position)
                cameraStatus = Status.free;

            camera.transform.position = new Vector3(temp.x, temp.y, camera.transform.position.z);
            camera.orthographicSize -= Time.deltaTime * speedSize;
        }

    }
}

