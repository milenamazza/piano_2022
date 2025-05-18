using UnityEngine;
using System.Collections.Generic;

public static class Utils
{
    public static Transform FindDeepChild(this Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            Transform result = child.FindDeepChild(name);
            if (result != null)
                return result;
        }
        return null;
    }
    
    public static List<Transform> FindAllChildrenByName(this Transform parent, string name)
    {
        List<Transform> result = new List<Transform>();

        foreach (Transform child in parent)
        {
            if (child.name == name)
                result.Add(child);

            // Ricorsione nei figli dei figli
            result.AddRange(child.FindAllChildrenByName(name));
        }

        return result;
    }
}
