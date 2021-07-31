using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLayer : MonoBehaviour
{
    [Header("Properties")]
    public float speed = 0.0f;
    public float density = 0.0f;
    public float temperature = 0.0f;
    public float salinity = 0.0f;
    public float ph = 0.0f;
    public float initialDepth = -1.0f;
    public float finalDepth = -1.0f;

    public float row;
    public float col;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public void CreateShape()
    {
        vertices = new Vector3[4];

        vertices[0] = new Vector3(0.0f, -initialDepth, 0.0f);
        vertices[1] = new Vector3(0.0f, -initialDepth, col);
        vertices[2] = new Vector3(row, -initialDepth, 0.0f);
        vertices[3] = new Vector3(row, -initialDepth, col);

        triangles = new int[6];

        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 1;
        triangles[4] = 2;
        triangles[5] = 3;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void CreateBottomShape()
    {
        vertices = new Vector3[8];

        vertices[0] = new Vector3(0.0f, -initialDepth, 0.0f);
        vertices[1] = new Vector3(0.0f, -initialDepth, col);
        vertices[2] = new Vector3(row, -initialDepth, 0.0f);
        vertices[3] = new Vector3(row, -initialDepth, col);

        vertices[4] = new Vector3(0.0f, -finalDepth, 0.0f);
        vertices[5] = new Vector3(0.0f, -finalDepth, col);
        vertices[6] = new Vector3(row, -finalDepth, 0.0f);
        vertices[7] = new Vector3(row, -finalDepth, col);

        triangles = new int[12];

        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 1;
        triangles[4] = 2;
        triangles[5] = 3;

        triangles[6] = 4;
        triangles[7] = 5;
        triangles[8] = 6;
        triangles[9] = 5;
        triangles[10] = 7;
        triangles[11] = 6;

        GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

}
