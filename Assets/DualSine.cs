using UnityEngine;
using System;
using System.Linq;

public sealed class DualSine : MonoBehaviour
{
    void Start()
      => GetComponent<CurveRenderer>().SetValues
           (Enumerable.Range(0, 2048).
            Select(i => Curve(i * 0.02f)).ToArray());

    float Curve(float x)
      => 0.15f * (-3.2f * Mathf.Sin(-1.3f * x)
                  -1.2f * Mathf.Sin(-1.8f * x * Mathf.Exp(1))
                  +1.9f * Mathf.Sin( 0.7f * x * Mathf.PI));
}
