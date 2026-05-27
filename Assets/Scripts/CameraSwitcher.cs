using UnityEngine;

public class Background : MonoBehaviour
{
    // Assign these in the Inspector
    public LevelLoader levelLoader;
    public BlockHandler blockHandler;
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
    public static bool playing;
    public int[,] loadedLevel;

    void Start()
    {
        // Ensure initial states are correct
        if (cameraOne != null)
            cameraOne.enabled = true;
        if (cameraTwo != null)
            cameraTwo.enabled = false;
        QualitySettings.vSyncCount = 0;
        targetFrameRate = 100;
        playing = false; 
    }

    void Update()
    {
        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;

        if (Input.GetKeyDown(KeyCode.R)) {
            BlockHandler.onReset?.Invoke();
            levelLoader.LoadLevel(loadedLevel);

        }

        // Check for a key press (e.g., the 'C' key)
        if (playing)
        {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SwitchCameras();
        }
        }
        if(!playing){
        if (mouseY < 726)
        {
            if(mouseY > 480)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //level 1
                    if(mouseX > 225)
                    {
                        if (mouseX < 450)
                        {
                            Debug.Log(1);
                            loadedLevel = Levels.levelOne;
                            levelLoader.LoadLevel(loadedLevel);
                            SwitchCameras();
                        }
                    }
                    //level 2
                    if(mouseX > 540)
                    {
                        if (mouseX < 750)
                        {
                            Debug.Log(2);
                            loadedLevel = Levels.levelTwo;
                            levelLoader.LoadLevel(loadedLevel);
                            SwitchCameras();
                        }
                    }
                    //level 3
                    if(mouseX > 850)
                    {
                        if (mouseX < 1075)
                        {
                            Debug.Log(3);
                            loadedLevel = Levels.levelThree;
                            levelLoader.LoadLevel(loadedLevel);
                            SwitchCameras();
                        }
                    }
                    //level 4
                    if(mouseX > 1150)
                    {
                        if (mouseX < 1400)
                        {
                            Debug.Log(4);
                            loadedLevel = Levels.levelFour;
                            levelLoader.LoadLevel(loadedLevel);
                            SwitchCameras();
                        }
                    }
                    //level 5
                    if(mouseX > 1475)
                    {
                        if (mouseX < 1715)
                        {
                            Debug.Log(5);
                        }
                    } 
                    // level 6
                    if(mouseX > 1780)
                    {
                        if (mouseX < 2030)
                        {
                            Debug.Log(6);
                        }
                    }
                    //level 7
                    if(mouseX > 2100)
                    {   
                        if (mouseX < 2350)
                        {
                            Debug.Log(7);
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
            // Debug.Log("FPS: " + frameRate);

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
        ifScreenToggled = !ifScreenToggled;
        text.SetActive(ifScreenToggled);
        playing = !playing;
    }
}