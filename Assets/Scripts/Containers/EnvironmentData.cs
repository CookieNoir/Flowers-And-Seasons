using System;
using UnityEngine;
[CreateAssetMenu(fileName = "Environment Data", menuName = "ScriptableObjects/Environment Data")]
public class EnvironmentData : ScriptableObject
{
    public Vector3 sunEulerAngle;
    public Color sunColor;
    [Range(0f, 1f)] public float intensity;
    public Color skyColor;
    public Color ambientColor;
}