using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public float mouseX;
    public float mouseY;
    public float sensitivity = 2.0f;
    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * sensitivity;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        transform.parent.Rotate(Vector3.up * mouseX); // Left/Right (on the player body)
        transform.Rotate(Vector3.left * mouseY);
    }
}