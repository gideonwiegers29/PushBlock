using UnityEngine;

public class GoalBlockManager : MonoBehaviour
{
    public GameObject goalBlock;
    public GameObject playerBlock;
    public Background background;
    public GameObject UIParent;
    GameObject levelButton;
    public AudioSource levelCompleteSound;


    void Start()
    {
        string[] levelNames = { "levelOne", "levelTwo", "levelThree", "levelFour" };
        foreach (string name in levelNames)
        {
            if (PlayerPrefs.GetInt(name + "_complete", 0) == 1)
            {
                foreach (Transform child in UIParent.transform)
                {
                    if (child.gameObject.name == name)
                    {
                        Renderer r = child.GetComponent<Renderer>();
                        Color color;
                        ColorUtility.TryParseHtmlString("#FF5733", out color);
                        r.material.color = color;
                        break;
                    }
                }
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (IsOccupied(goalBlock.transform.position, playerBlock)) {
            Debug.Log("Level Complete");
            goalBlock.transform.position += Vector3.up;
            levelCompleteSound.GetComponent<AudioSource>().Play();
            if (PlayerPrefs.GetInt($"{background.loadedLevelName}_complete", 0) == 0)
            {
                markLvlAsComplete(background.loadedLevel, background.loadedLevelName);
            }
            
            background.SwitchCameras();
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs cleared");
        }
    }

    void markLvlAsComplete(int[,] level, string levelName) {
        Debug.Log("called");
        PlayerPrefs.SetInt($"{levelName}_complete", 1);
        PlayerPrefs.Save();

        foreach (Transform child in UIParent.transform) {
            
            if (child.gameObject.name == levelName) {
                levelButton = child.gameObject;
                break;
            }
        }
        Renderer objRenderer = levelButton.GetComponent<Renderer>();
        Color color;
        ColorUtility.TryParseHtmlString("#FF5733", out color);
        objRenderer.material.color = color;
        


        
        int done = PlayerPrefs.GetInt($"{levelName}_complete", 0);
    }

    bool IsOccupied(Vector3 position, GameObject target)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, 0.1f);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == target)
            {
                return true;
            }
        }

        return false;
    }

}
