using UnityEngine;
using System;
using System.Linq;

public sealed class CurveRenderer : MonoBehaviour
{
    public enum NoiseType { Perlin, Sine }

    [SerializeField] NoiseType _type = NoiseType.Perlin;
    [SerializeField] float _range = 10;
    [SerializeField] int _resolution = 1024;
    [SerializeField] Material _material = null;

    Mesh _mesh;

    float NoiseFunc(float x)
      => _type switch { NoiseType.Perlin => Noise.Perlin(x),
                        NoiseType.Sine => Noise.Sine(x),
                        _ => 0 };

    void Start()
    {
        var indices = Enumerable.Range(0, _resolution);

        var vertices = indices.Select(i => (float)i / _resolution).
          Select(x => new Vector3(x - 0.5f, NoiseFunc(_range * x), 0));

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
