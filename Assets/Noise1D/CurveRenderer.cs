using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace NoiseTest {

[ExecuteInEditMode]
public sealed class CurveRenderer : MonoBehaviour
{
    #region Public properties

    [field:SerializeField] public NoiseType NoiseType { get; set; }
    [field:SerializeField] public int Resolution { get; set; } = 1024;
    [field:SerializeField] public float Frequency { get; set; } = 10;
    [field:SerializeField] public int Octaves { get; set; } = 1;
    [field:SerializeField] public float Amplitude { get; set; } = 1;

    #endregion

    #region Project asset references

    [SerializeField, HideInInspector] Material _material = null;

    #endregion

    #region Private members

    (NativeArray<int> i, NativeArray<float3> p) _array;
    Mesh _mesh;

    void UpdateMesh()
    {
        if (!_array.i.IsCreated || _array.i.Length != Resolution)
        {
            if (_array.i.IsCreated) _array.i.Dispose();
            if (_array.p.IsCreated) _array.p.Dispose();

            _array.i = new NativeArray<int>
              (Resolution, Allocator.Persistent,
               NativeArrayOptions.UninitializedMemory);

            _array.p = new NativeArray<float3>
              (Resolution, Allocator.Persistent,
               NativeArrayOptions.UninitializedMemory);
        }

        for (var i = 0; i < Resolution; i++)
        {
            var x = (float)i / Resolution - 0.5f;
            var y = NoiseGen.Fractal(NoiseType, (x + 0.5f) * Frequency, Octaves);
            _array.i[i] = i;
            _array.p[i] = math.float3(x, y * Amplitude, 0);
        }

        if (_mesh == null || _mesh.vertexCount != Resolution)
        {
            CoreUtils.Destroy(_mesh);

            _mesh = new Mesh();
            _mesh.hideFlags = HideFlags.HideAndDontSave;
        }

        _mesh.Clear();
        _mesh.SetVertices(_array.p);
        _mesh.SetIndices(_array.i, MeshTopology.LineStrip, 0, true);
    }

    #endregion

    #region MonoBehaviour implementation

    void Start()
      => UpdateMesh();

    void OnValidate()
    {
        Resolution = Mathf.Clamp(Resolution, 8, 4096);
        UpdateMesh();
    }

    void OnDestroy()
      => CoreUtils.Destroy(_mesh);

    void OnDisable()
    {
        if (_array.i.IsCreated) _array.i.Dispose();
        if (_array.p.IsCreated) _array.p.Dispose();
    }

    void LateUpdate()
      => Graphics.DrawMesh
           (_mesh, transform.localToWorldMatrix, _material, gameObject.layer);

    #endregion
}

} // namespace NoiseTest
