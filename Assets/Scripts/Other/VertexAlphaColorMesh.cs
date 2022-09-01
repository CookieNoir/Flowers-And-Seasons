using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class VertexAlphaColorMesh : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float _startAlpha;
    private Color _color;
    private float _alpha;
    private MeshFilter _meshFilter;
    private Mesh _mesh;
    private Color[] _colors;

    public void CreateMeshWithColor(Color color)
    {
        _meshFilter = GetComponent<MeshFilter>();
        _mesh = new Mesh();
        _mesh.vertices = _meshFilter.mesh.vertices;
        _mesh.uv = _meshFilter.mesh.uv;
        _mesh.normals = _meshFilter.mesh.normals;
        _mesh.triangles = _meshFilter.mesh.triangles;
        _colors = new Color[_mesh.vertexCount];
        SetColorAndAlphaValues(color, _startAlpha);
        _meshFilter.mesh = _mesh;
    }

    public void SetColorAndAlphaValues(Color color, float alpha)
    {
        _color = color;
        _alpha = alpha;
        _ChangeVertexColors();
    }

    public void SetColor(Color color)
    {
        _color = color;
        _ChangeVertexColors();
    }

    public void SetAlpha(float alpha)
    {
        _alpha = alpha;
        _ChangeVertexColors();
    }

    private void _ChangeVertexColors()
    {
        for (int i = 0; i < _colors.Length; ++i)
        {
            _colors[i] = new Color(_color.r, _color.g, _color.b, _color.a * _alpha);
        }
        _mesh.colors = _colors;
    }
}