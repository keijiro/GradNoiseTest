using UnityEngine;
using System.Linq;

public sealed class CurveRenderer : MonoBehaviour
{
    [SerializeField] Noise.Method _method = Noise.Method.Perlin;
    [SerializeField] float _range = 10;
    [SerializeField] int _resolution = 1024;
    [SerializeField] Material _material = null;

    Mesh _mesh;

    void Start()
    {
        var indices = Enumerable.Range(0, _resolution);

        var vertices = indices.Select(i => (float)i / _resolution).
          Select(x => new Vector3(x - 0.5f, Noise.Func(_method, _range * x), 0));

        _mesh = new Mesh();
        _mesh.vertices = vertices.ToArray();
        _mesh.SetIndices(indices.ToArray(), MeshTopology.LineStrip, 0);
        _mesh.RecalculateBounds();
    }

    void OnDestroy()
      => Destroy(_mesh);

    void LateUpdate()
      => Graphics.DrawMesh
           (_mesh, transform.localToWorldMatrix, _material, gameObject.layer);
}
