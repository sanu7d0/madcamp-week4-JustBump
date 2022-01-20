using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform targetTransform;

    void FixedUpdate() {
        this.transform.position = 
            new Vector3(targetTransform.position.x, targetTransform.position.y, this.transform.position.z); 
    }
}
