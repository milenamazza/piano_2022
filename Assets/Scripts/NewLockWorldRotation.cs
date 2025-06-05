using UnityEngine;

public class LockWorldRotation : MonoBehaviour
{
    void LateUpdate() 
    {
        // 1. Blocca la rotazione dell'XR Origin
        transform.rotation = Quaternion.identity;
        
        // 2. Blocca il movimento orizzontale (opzionale)
        transform.position = new Vector3(0, transform.position.y, 0);
        
        // 3. Reset extra per sicurezza
        if (Camera.main != null)
        {
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localRotation = Quaternion.identity;
        }
    }
}
