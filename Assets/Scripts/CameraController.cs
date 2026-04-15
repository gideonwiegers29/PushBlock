using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public bool cursorLocked;
    public int mouseMovement;
    public float mouseX;
    public float mouseY;
    public float sensitivity = 2.0f;
    public float playerSpeed = 5.0f;

    public GameObject player;
    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * sensitivity;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        player.transform.Rotate(Vector3.up * mouseX * mouseMovement); // Left/Right (on the player body)
        transform.Rotate(Vector3.left * mouseY * mouseMovement);

        if (Input.GetKey(KeyCode.W)) {
            player.transform.Translate(Vector3.left * Time.deltaTime * playerSpeed);
        }
        if (Input.GetKey(KeyCode.A)) {
            player.transform.Translate(Vector3.back * Time.deltaTime * playerSpeed);
        }
        if (Input.GetKey(KeyCode.D)) {
            player.transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed);
        }
        if (Input.GetKey(KeyCode.S)) {
            player.transform.Translate(Vector3.right * Time.deltaTime * playerSpeed);
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            player.GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }

        player.transform.rotation = Quaternion.Euler(0, player.transform.rotation.eulerAngles.y, 0);

        // Cursor Lock
        if (Input.GetKeyDown(KeyCode.L)) {
            cursorLocked = !cursorLocked;
        }
        if (cursorLocked) {
            Cursor.lockState = CursorLockMode.Locked;
            mouseMovement = 1;
        } else {
            Cursor.lockState = CursorLockMode.None;
            mouseMovement = 0;
        }
        
    }
}