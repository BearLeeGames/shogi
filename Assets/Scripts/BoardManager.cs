using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class BoardManager : MonoBehaviour {

    private void Start()
    {
        // creating a cube at 0, 0, 0
        Vector3[] vertices = getVertices(new Vector3(0, 0, 0));
        int[] triangles = getTriangles();

        createMesh(vertices, triangles);

        Debug.Log(SystemInfo.graphicsShaderLevel);

    }

    // param: Vector3 of the bottom left back corner
    // return: all the other vertices that make it a cube
    private Vector3[] getVertices(Vector3 v)
    {

        Vector3[] vertices = {
            new Vector3 (v.x, v.y, v.z),
            new Vector3 (v.x + 1, v.y, v.z),
            new Vector3 (v.x + 1, v.y + 1, v.z),
            new Vector3 (v.x, v.y + 1, v.z),
            new Vector3 (v.x, v.y + 1, v.z + 1),
            new Vector3 (v.x + 1, v.y + 1, v.z + 1),
            new Vector3 (v.x + 1, 0, v.z + 1),
            new Vector3 (v.x, v.y, v.z + 1),
        };

        return vertices;
    }

    // param: none
    // return: list integers that form the triangles needed to create the cube
    private int[] getTriangles()
    {
        // Note: this is based entirely on the getVertices format
        int[] triangles =
        {
            0, 2, 1, //face front
			0, 3, 2,
            2, 3, 4, //face top
			2, 4, 5,
            1, 2, 5, //face right
			1, 5, 6,
            0, 7, 4, //face left
			0, 4, 3,
            5, 4, 7, //face back
			5, 7, 6,
            0, 6, 7, //face bottom
			0, 1, 6
        };

        return triangles;
    }

    // param: list of Vector3's (vertices) and list of ints (triangles)
    // return: mesh object
    private Mesh createMesh(Vector3[] vertices, int[] triangles)
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }


}   
