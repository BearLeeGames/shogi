using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour {

    public float turnSpeed = 4.0f;
    public Transform board;

    private Vector3 offset;

    void Start() {
        offset = new Vector3(board.position.x +5.0f, board.position.y + 10.0f, board.position.z);
    }

    void LateUpdate() {
        if (Input.GetMouseButton(0)) {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
            transform.position = board.position + offset;
            transform.LookAt(board.position);

            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * turnSpeed, Vector3.left) * offset;
            transform.position = board.position + offset;
            transform.LookAt(board.position);
        }
    }
}
