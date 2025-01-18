using UnityEngine;
using System;
using System.Linq;

public sealed class CurveRenderer : MonoBehaviour
{
    [SerializeField] Material _material = null;

    Mesh _mesh;

    public void SetValues(ReadOnlySpan<float> values)
    {
        if (_mesh == null)
            _mesh = new Mesh();
        else
            _mesh.Clear();

        var mx = 1.0f / values.Length;

        _mesh.vertices = values.ToArray().
                         Select((y, i) => new Vector3(mx * i, y, 0)).ToArray();

        _mesh.SetIndices(Enumerable.Range(0, values.Length).ToArray(),
                         MeshTopology.LineStrip, 0);

        _mesh.RecalculateBounds();
    }

    void OnDestroy()
    {
        if (_mesh != null) Destroy(_mesh);
    }

    void LateUpdate()
    {
        if (_mesh != null)
            Graphics.DrawMesh(_mesh, transform.localToWorldMatrix,
                              _material, gameObject.layer);
    }
}
