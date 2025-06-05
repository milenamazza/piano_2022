using UnityEngine;

public class RotationDebug : MonoBehaviour
{
    void Update()
    {
        Debug.Log($"XR Origin Rotation: {transform.rotation.eulerAngles}");
        Debug.Log($"Camera Rotation: {Camera.main.transform.rotation.eulerAngles}");
    }
}
