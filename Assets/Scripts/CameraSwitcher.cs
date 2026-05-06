using UnityEngine;

public class Background : MonoBehaviour
{
    // Assign these in the Inspector
    public Camera cameraOne;
    public Camera cameraTwo;
    private float pollingTime = 1f; // Update interval in seconds
    private float time;
    private int frameCount;
    public int targetFrameRate = 100;
    public GameObject text;
    public bool ifScreenToggled = true;
    public float mouseX;
    public float mouseY;

    void Start()
    {
        // Ensure initial states are correct
        if (cameraOne != null)
            cameraOne.enabled = true;
        if (cameraTwo != null)
            cameraTwo.enabled = false;
        QualitySettings.vSyncCount = 0;
        targetFrameRate = 100;
    }

    void Update()
    {
        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;
        // Check for a key press (e.g., the 'C' key)
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCameras();
            ifScreenToggled = !ifScreenToggled;
            text.SetActive(ifScreenToggled);
        }
        if (mouseX > 975)
        {
            if (mouseX < 1575)
            {
                if (mouseY < 960)
                {
                    if (mouseY > 660)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                             {
                                SwitchCameras();
                                ifScreenToggled = !ifScreenToggled;
                                text.SetActive(ifScreenToggled);
                            }
                        }
                    }
                }
            }
        }
        time += Time.deltaTime;
        frameCount++;   
        if (time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            Debug.Log("FPS: " + frameRate);

            // Reset the counters
            time -= pollingTime;
            frameCount = 0;
            Application.targetFrameRate = targetFrameRate + 1;
        }
    }

    void SwitchCameras()
    {
        // Toggle the enabled state of both cameras
        if (cameraOne != null && cameraTwo != null)
        {
            cameraOne.enabled = !cameraOne.enabled;
            cameraTwo.enabled = !cameraTwo.enabled;
        }
    }
}