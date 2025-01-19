using UnityEngine;
using System.Linq;

public sealed class GradientRenderer : MonoBehaviour
{
    [SerializeField] Noise.Method _method = Noise.Method.Perlin;
    [SerializeField] float _range = 10;
    [SerializeField] int _resolution = 1024;
    [SerializeField] Shader _shader = null;
    [SerializeField] Mesh _mesh = null;

    Texture2D _texture;
    Material _material;

    void Start()
    {
        var pixels = Enumerable.Range(0, _resolution).
          Select(i => (float)i / _resolution).
          Select(x => Noise.Func(_method, _range * x)).
          Select(y => Color.white * (y + 1) / 2);

        _texture = new Texture2D(_resolution, 1, TextureFormat.RGBA32, false);
        _texture.SetPixels(pixels.ToArray());
        _texture.Apply();

        _material = new Material(_shader);
        _material.mainTexture = _texture;
    }

    void OnDestroy()
    {
        Destroy(_material);
        Destroy(_texture);
    }

    void LateUpdate()
      => Graphics.DrawMesh
           (_mesh, transform.localToWorldMatrix, _material, gameObject.layer);
}
