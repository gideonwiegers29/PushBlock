using UnityEngine;
using UnityEngine.SceneManagement;

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
    public GameObject resetText;
    public static bool ifScreenToggled = true;
    public float mouseX;
    public float mouseY;
    public static bool playing;
    public int[,] loadedLevel;
    public string loadedLevelName;
    public AudioSource buttonPressedSound;

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
        if (Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.H) && Input.GetKey(KeyCode.J)) {
            SceneManager.LoadScene("levelEditor");
        }


        // Check for a key press (e.g., the 'C' key)
        if (playing)
        {
            if (Input.GetKeyDown(KeyCode.P)) {
                SwitchCameras();
            }
        }

        if (!playing && Input.GetMouseButtonDown(0)) {
            Camera cam = cameraOne.enabled ? cameraOne : cameraTwo;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                TryLoadLevel(hit.collider.tag);
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

    public void SwitchCameras()
    {
        // Toggle the enabled state of both cameras
        if (cameraOne != null && cameraTwo != null)
        {
            cameraOne.enabled = !cameraOne.enabled;
            cameraTwo.enabled = !cameraTwo.enabled;
        }
        ifScreenToggled = !ifScreenToggled;
        text.SetActive(ifScreenToggled);
        resetText.SetActive(!ifScreenToggled);
        playing = !playing;
    }

    void TryLoadLevel(string tag)
    {
        int[,] level = null;
        string name = null;
        switch (tag)
        {
            case "Level1": level = Levels.levelOne;   name = "levelOne";   break;
            case "Level2": level = Levels.levelTwo;   name = "levelTwo";   break;
            case "Level3": level = Levels.levelThree; name = "levelThree"; break;
            case "Level4": level = Levels.levelFour;  name = "levelFour";  break;
            case "Level5": level = Levels.levelFive;  name = "levelFive";  break;
            case "Level6": level = Levels.levelSix;   name = "levelSix";   break;
            case "Level7": level = Levels.levelSeven; name = "levelSeven"; break;
            default: return;
        }
        loadedLevel = level;
        loadedLevelName = name;
        levelLoader.LoadLevel(level);
        buttonPressedSound.Play();
        SwitchCameras();
    }

}