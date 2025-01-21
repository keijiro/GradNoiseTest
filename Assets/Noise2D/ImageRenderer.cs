using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace NoiseTest {

[ExecuteInEditMode]
public sealed class ImageRenderer : MonoBehaviour
{
    #region Public properties

    [field:SerializeField] public NoiseType NoiseType { get; set; }
    [field:SerializeField] public int Resolution { get; set; } = 1024;
    [field:SerializeField] public float Frequency { get; set; } = 10;
    [field:SerializeField] public int Octaves { get; set; } = 1;
    [field:SerializeField] public float Amplitude { get; set; } = 1;

    #endregion

    #region Project asset references

    [SerializeField, HideInInspector] Shader _shader = null;
    [SerializeField, HideInInspector] Mesh _mesh = null;

    #endregion

    #region Private members

    NativeArray<byte> _array;
    Texture2D _texture;
    Material _material;

    void UpdateTexture()
    {
        if (!_array.IsCreated || _array.Length != Resolution * Resolution)
        {
            if (_array.IsCreated) _array.Dispose();
            _array = new NativeArray<byte>
              (Resolution * Resolution, Allocator.Persistent,
               NativeArrayOptions.UninitializedMemory);
        }

        for (var (yi, offs) = (0, 0); yi < Resolution; yi++)
        {
            var y = Frequency * yi / Resolution;
            for (var xi = 0; xi < Resolution; xi++, offs++)
            {
                var x = Frequency * xi / Resolution;
                var val = NoiseGen.Fractal(NoiseType, math.float2(x, y), Octaves);
                _array[offs] = (byte)(255 * math.saturate((val * Amplitude + 1) / 2));
            }
        }

        if (_texture == null || _texture.width != Resolution)
        {
            CoreUtils.Destroy(_texture);

            _texture = new Texture2D
              (Resolution, Resolution, TextureFormat.R8, false);
            _texture.hideFlags = HideFlags.HideAndDontSave;
        }

        _texture.SetPixelData(_array, 0);
        _texture.Apply();

        if (_material == null)
            _material = CoreUtils.CreateEngineMaterial(_shader);

        _material.mainTexture = _texture;
    }

    #endregion

    #region MonoBehaviour implementation

    void Start()
      => UpdateTexture();

    void OnValidate()
    {
        Resolution = Mathf.Clamp(Resolution, 1, 4096);
        UpdateTexture();
    }

    void OnDestroy()
    {
        CoreUtils.Destroy(_texture);
        CoreUtils.Destroy(_material);
    }

    void OnDisable()
    {
        if (_array.IsCreated) _array.Dispose();
    }

    void LateUpdate()
      => Graphics.DrawMesh
           (_mesh, transform.localToWorldMatrix, _material, gameObject.layer);

    #endregion
}

} // namespace NoiseTest
