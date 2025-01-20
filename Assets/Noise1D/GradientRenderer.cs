using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Noise1D {

[ExecuteInEditMode]
public sealed class GradientRenderer : MonoBehaviour
{
    #region Public properties

    public enum Method { Perlin1D, Gradient360 }

    [field:SerializeField] public Method NoiseType { get; set; } = Method.Perlin1D;
    [field:SerializeField] public int Resolution { get; set; } = 1024;
    [field:SerializeField] public float Frequency { get; set; } = 10;

    #endregion

    #region Project asset references

    [SerializeField, HideInInspector] Shader _shader = null;
    [SerializeField, HideInInspector] Mesh _mesh = null;

    #endregion

    #region Private members

    NativeArray<byte> _array;
    Texture2D _texture;
    Material _material;

    float GetNoiseAt(float p)
      => NoiseType switch
         { Method.Perlin1D => Perlin1D.GetAt(p),
           Method.Gradient360 => Gradient360.GetAt(p),
           _ => 0 };

    void UpdateTexture()
    {
        if (!_array.IsCreated || _array.Length != Resolution)
        {
            if (_array.IsCreated) _array.Dispose();
            _array = new NativeArray<byte>
              (Resolution, Allocator.Persistent,
               NativeArrayOptions.UninitializedMemory);
        }

        for (var i = 0; i < Resolution; i++)
        {
            var x = (float)i / Resolution - 0.5f;
            var y = GetNoiseAt((x + 0.5f) * Frequency);
            _array[i] = (byte)(255 * (y + 1) / 2);
        }

        if (_texture == null || _texture.width != Resolution)
        {
            CoreUtils.Destroy(_texture);

            _texture = new Texture2D
              (Resolution, 1, TextureFormat.R8, false);
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

} // namespace Noise2D
