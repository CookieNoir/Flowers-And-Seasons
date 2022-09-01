using UnityEngine;

public static class Destroyer
{
    public static void DestroyChildren(Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}