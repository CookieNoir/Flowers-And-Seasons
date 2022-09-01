using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class Grid : MonoBehaviour
{
    [SerializeField, Range(0.01f, 0.5f)] private float _thickness;

    public void CreateGrid(Vector2Int worldSize)
    {
        int verticalCount = worldSize.x * (worldSize.y + 1);
        int horizontalCount = (worldSize.x + 1) * worldSize.y;
        int totalVerticesCount = 6 * (verticalCount + horizontalCount);
        int totalTrianglesCount = 2 * totalVerticesCount;
        Vector3[] vertices = new Vector3[totalVerticesCount];
        Color[] colors = new Color[totalVerticesCount];
        int[] triangles = new int[totalTrianglesCount];
        int verticesOffset = 0;
        int trianglesOffset = 0;
        for (int i = 0; i <= worldSize.x; ++i)
        {
            for (int j = 0; j < worldSize.y; ++j)
            {
                vertices[verticesOffset] = new Vector3(i, 0f, j);
                vertices[verticesOffset + 1] = new Vector3(i - _thickness, 0f, j + _thickness);
                vertices[verticesOffset + 2] = new Vector3(i - _thickness, 0f, j + 1 - _thickness);
                vertices[verticesOffset + 3] = new Vector3(i, 0f, j + 1);
                vertices[verticesOffset + 4] = new Vector3(i + _thickness, 0f, j + 1 - _thickness);
                vertices[verticesOffset + 5] = new Vector3(i + _thickness, 0f, j + _thickness);

                colors[verticesOffset] = Color.white;
                colors[verticesOffset + 1] = Color.clear;
                colors[verticesOffset + 2] = Color.clear;
                colors[verticesOffset + 3] = Color.white;
                colors[verticesOffset + 4] = Color.clear;
                colors[verticesOffset + 5] = Color.clear;

                triangles[trianglesOffset] = verticesOffset;
                triangles[trianglesOffset + 1] = verticesOffset + 1;
                triangles[trianglesOffset + 2] = verticesOffset + 2;

                triangles[trianglesOffset + 3] = verticesOffset;
                triangles[trianglesOffset + 4] = verticesOffset + 2;
                triangles[trianglesOffset + 5] = verticesOffset + 3;

                triangles[trianglesOffset + 6] = verticesOffset + 3;
                triangles[trianglesOffset + 7] = verticesOffset + 4;
                triangles[trianglesOffset + 8] = verticesOffset + 5;

                triangles[trianglesOffset + 9] = verticesOffset + 3;
                triangles[trianglesOffset + 10] = verticesOffset + 5;
                triangles[trianglesOffset + 11] = verticesOffset;

                verticesOffset += 6;
                trianglesOffset += 12;
            }
        }
        for (int i = 0; i < worldSize.x; ++i)
        {
            for (int j = 0; j <= worldSize.y; ++j)
            {
                vertices[verticesOffset] = new Vector3(i, 0f, j);
                vertices[verticesOffset + 1] = new Vector3(i + _thickness, 0f, j + _thickness);
                vertices[verticesOffset + 2] = new Vector3(i + 1 - _thickness, 0f, j + _thickness);
                vertices[verticesOffset + 3] = new Vector3(i + 1, 0f, j);
                vertices[verticesOffset + 4] = new Vector3(i + 1 - _thickness, 0f, j - _thickness);
                vertices[verticesOffset + 5] = new Vector3(i + _thickness, 0f, j - _thickness);

                colors[verticesOffset] = Color.white;
                colors[verticesOffset + 1] = Color.clear;
                colors[verticesOffset + 2] = Color.clear;
                colors[verticesOffset + 3] = Color.white;
                colors[verticesOffset + 4] = Color.clear;
                colors[verticesOffset + 5] = Color.clear;

                triangles[trianglesOffset] = verticesOffset;
                triangles[trianglesOffset + 1] = verticesOffset + 1;
                triangles[trianglesOffset + 2] = verticesOffset + 2;

                triangles[trianglesOffset + 3] = verticesOffset;
                triangles[trianglesOffset + 4] = verticesOffset + 2;
                triangles[trianglesOffset + 5] = verticesOffset + 3;

                triangles[trianglesOffset + 6] = verticesOffset + 3;
                triangles[trianglesOffset + 7] = verticesOffset + 4;
                triangles[trianglesOffset + 8] = verticesOffset + 5;

                triangles[trianglesOffset + 9] = verticesOffset + 3;
                triangles[trianglesOffset + 10] = verticesOffset + 5;
                triangles[trianglesOffset + 11] = verticesOffset;

                verticesOffset += 6;
                trianglesOffset += 12;
            }
        }
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.triangles = triangles;
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void Clear()
    {
        GetComponent<MeshFilter>().mesh = null;
    }
}
