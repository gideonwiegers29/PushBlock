using UnityEngine;

public class LevelLoader : MonoBehaviour
{

    
    public GameObject blockPrefab;
    public GameObject playerBlock;
    public GameObject goalBlock;
    public GameObject player;
    public bool levelLoaded;

    void PlaceBlock(int x, int y, int blockType)
    {
        Debug.Log($"placed at {x}, {y} with type {blockType}");
        if (blockType == 1) {
            Instantiate(blockPrefab, new Vector3(x -7, .8f, y - 7), Quaternion.identity);
        } else if (blockType == 2) {
            playerBlock.transform.position = new Vector3(x - 7, 1, y - 7);
            player.transform.position = new Vector3(x - 7, 3, y - 7);
        } else if (blockType == 3)
        {
            goalBlock.transform.position = new Vector3(x - 7, 1, y - 7);
        }
        levelLoaded = true;
        
    }

    public void LoadLevel(int[,] level)
    {
        if (levelLoaded == true) {
            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            foreach (GameObject obj in allObjects)
            {
                Debug.Log(obj.name);
                if (obj.name == "Cube(Clone)")
                {
                    Destroy(obj);
                }
            }
            levelLoaded = false;
        } else {
            levelLoaded = true;
        }
        int rows = level.GetLength(0);
        int cols = level.GetLength(1);
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                PlaceBlock(col, row, level[row, col]);
            }
        }
    }

    void Start()
    {
        // LoadLevel(Levels.demoLevel);
    }
}
