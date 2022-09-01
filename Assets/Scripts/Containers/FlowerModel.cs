using System;
using UnityEngine;

[Serializable]
public struct FlowerModel
{
    public GameObject flowerPrefab;
    public Sprite icon;
    [Min(2)] public int numberOfSeeds;
    public bool growsInSpring;
    public bool growsInSummer;
    public bool growsInAutumn;
    public bool growsInWinter;
}