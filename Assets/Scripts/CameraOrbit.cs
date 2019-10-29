using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour {
    [SerializeField] private Camera cam;

    private Vector3 prevPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            prevPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 direction = prevPosition - cam.ScreenToViewportPoint(Input.mousePosition);
            cam.transform.position = new Vector3();

            cam.transform.Rotate(new Vector3(x: 1, y: 0, z: 0), angle:direction.y * 180);
            cam.transform.Rotate(new Vector3(x: 0, y: 1, z: 0), angle:-direction.x * 180, relativeTo: Space.World);

            cam.transform.Translate(new Vector3(x: 0, y: 0, z: -10));

            prevPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetAxis ("Mouse ScrollWheel") > 0)
        {
            if (cam.fieldOfView > 40)
                cam.fieldOfView--;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (cam.fieldOfView < 100)
                cam.fieldOfView++;
        }
    }

    public void ResetCamera() {
        cam.transform.position = new Vector3(x: 0, y: 0, z: -10);
        cam.transform.rotation = new Quaternion(x: 0, y: 0, z: 0, w:0);
        cam.fieldOfView = 60;
    }
}
