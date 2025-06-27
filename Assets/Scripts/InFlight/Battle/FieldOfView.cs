using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class FieldOfView : MonoBehaviour
{
    [Range(0, 360)]
    public float fov = 360.0f;

    public int rayCount = 360;
    public float viewDistance = 7.0f;
    public LayerMask layerMask;
    private Mesh mesh;
    public Transform playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        mesh.name = "FOV";
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void LateUpdate()
    {
        DrawFOV();
        transform.position = new Vector3(playerPosition.position.x, playerPosition.position.y - 0.75f, playerPosition.position.z);
    }

    private void DrawFOV()
    {
        float angle = fov * 0.5f;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, GetVectorFromAngle(transform.eulerAngles.z + angle), viewDistance, layerMask);

            if (raycastHit2D.collider == null)
            {
                vertex = GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                vertex = GetVectorFromAngle(angle) * (raycastHit2D.distance / viewDistance) * viewDistance;
            }
            vertices[vertexIndex] = vertex;

            if (0 < i)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            ++vertexIndex;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
