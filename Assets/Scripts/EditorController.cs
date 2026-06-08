using System.IO;
using System.Text;
using UnityEngine;

public class EditorController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (cam == null) cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;

        SeedSingleton(playerBlockObj, 2);
        SeedSingleton(goalBlockObj, 3);
        SeedSingleton(player, 4);

        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody>();
            playerCol = player.GetComponent<Collider>();
        }
    }
    public bool cursorLocked;
    public int mouseMovement;
    public float mouseX;
    public float mouseY;
    public float sensitivity = 2.0f;
    public float playerSpeed = 10.0f;
    public float verticalRotation;
    private float minPitch = -90, maxPitch = 90;


    public GameObject player;
    public GameObject blockPrefab;
    public GameObject playerBlockObj;
    public GameObject goalBlockObj;
    public Camera cam;

    private const int Size = 16;
    private int[,] grid = new int[Size, Size];
    private GameObject[,] tileObjects = new GameObject[Size, Size];
    private int brush = 1;
    private Vector2Int? hoverCell;
    private bool playtesting;
    private int[,] playtestSnapshot;
    private Rigidbody playerRb;
    private Collider playerCol;
    private bool savedGravity;
    private bool savedKinematic;
    private bool savedColEnabled;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) {
            player.transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed);
        }
        if (Input.GetKey(KeyCode.A)) {
            player.transform.Translate(Vector3.left * Time.deltaTime * playerSpeed);
        }
        if (Input.GetKey(KeyCode.D)) {
            player.transform.Translate(Vector3.right * Time.deltaTime * playerSpeed);
        }
        if (Input.GetKey(KeyCode.S)) {
            player.transform.Translate(Vector3.back * Time.deltaTime * playerSpeed);
        }
        if (!playtesting && Input.GetKey(KeyCode.Space)) {
            player.transform.Translate(Vector3.up * Time.deltaTime * playerSpeed);
        }
        if (!playtesting && Input.GetKey(KeyCode.LeftShift)) {
            player.transform.Translate(Vector3.down * Time.deltaTime * playerSpeed);
        }

        if (playtesting) {
            player.transform.rotation = Quaternion.Euler(0, player.transform.rotation.eulerAngles.y, 0);
        }
        // toggle with L
        if (Input.GetKeyDown(KeyCode.L)) {
            cursorLocked = !cursorLocked;
        }

        // Cursor Lock
        if (cursorLocked) {
            Cursor.lockState = CursorLockMode.Locked;
            mouseMovement = 1;
        } else {
            Cursor.lockState = CursorLockMode.None;
            mouseMovement = 0;
        }
        // yRotation = transform.eulerAngles.y;

        // playtest toggle
        if (Input.GetKeyDown(KeyCode.P)) TogglePlaytest();

        // reset during playtest
        if (playtesting && Input.GetKeyDown(KeyCode.R))
        {
            BlockHandler.onReset?.Invoke();
            if (playtestSnapshot != null) LoadLevel(playtestSnapshot);
        }

        if (!playtesting)
        {
            // brush selection
            if (Input.GetKeyDown(KeyCode.Alpha1)) brush = 1;
            if (Input.GetKeyDown(KeyCode.Alpha2)) brush = 2;
            if (Input.GetKeyDown(KeyCode.Alpha3)) brush = 3;
            if (Input.GetKeyDown(KeyCode.Alpha4)) brush = 4;

            // aim raycast
            hoverCell = null;
            if (cam != null)
            {
                Ray ray = new Ray(cam.transform.position, cam.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                {
                    Vector3 p = hit.point + hit.normal * 0.01f;
                    Vector2Int cell = WorldToCell(p);
                    if (InBounds(cell)) hoverCell = cell;
                }
            }

            // place / erase (only while cursor is locked, so it doesn't fire on UI clicks)
            if (cursorLocked && hoverCell.HasValue)
            {
                if (Input.GetMouseButtonDown(0)) Place(hoverCell.Value, brush);
                if (Input.GetMouseButtonDown(1)) Place(hoverCell.Value, 0);
            }

            // save / load
            if (Input.GetKeyDown(KeyCode.F1)) ExportLevel();
            if (Input.GetKeyDown(KeyCode.F2)) LoadLevel(Levels.levelOne);
            if (Input.GetKeyDown(KeyCode.F3)) LoadLevel(Levels.levelTwo);
            if (Input.GetKeyDown(KeyCode.F4)) LoadLevel(Levels.levelThree);
            if (Input.GetKeyDown(KeyCode.F5)) LoadLevel(Levels.levelFour);
        }
        else
        {
            hoverCell = null;
        }
    }

    void TogglePlaytest()
    {
        if (!playtesting)
        {
            playtestSnapshot = (int[,])grid.Clone();
            playtesting = true;
            cursorLocked = true;
            if (playerRb != null)
            {
                savedGravity = playerRb.useGravity;
                savedKinematic = playerRb.isKinematic;
                playerRb.useGravity = true;
                playerRb.isKinematic = false;
            }
            if (playerCol != null)
            {
                savedColEnabled = playerCol.enabled;
                playerCol.enabled = true;
            }
        }
        else
        {
            playtesting = false;
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;
                playerRb.useGravity = savedGravity;
                playerRb.isKinematic = savedKinematic;
            }
            if (playerCol != null) playerCol.enabled = savedColEnabled;
            BlockHandler.onReset?.Invoke();
            if (playtestSnapshot != null) LoadLevel(playtestSnapshot);
        }
    }
    void LateUpdate()
    {
        mouseX = Input.GetAxis("Mouse X") * sensitivity;
        mouseY = Input.GetAxis("Mouse Y");
        verticalRotation -= mouseY * sensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, minPitch, maxPitch);
        player.transform.Rotate(Vector3.up * mouseX * mouseMovement); // Left/Right (on the player body)
        transform.localRotation = Quaternion.Euler(verticalRotation * mouseMovement, 0, 0);

    }

    void SeedSingleton(GameObject obj, int type)
    {
        if (obj == null) return;
        Vector2Int c = WorldToCell(obj.transform.position);
        if (!InBounds(c)) return;
        grid[c.y, c.x] = type;
        if (type != 4) tileObjects[c.y, c.x] = obj;
    }

    Vector2Int WorldToCell(Vector3 pos)
    {
        return new Vector2Int(Mathf.RoundToInt(pos.x) + 7, Mathf.RoundToInt(pos.z) + 7);
    }

    bool InBounds(Vector2Int c)
    {
        return c.x >= 0 && c.x < Size && c.y >= 0 && c.y < Size;
    }

    void ClearSingletonFromGrid(int type)
    {
        for (int r = 0; r < Size; r++)
            for (int c = 0; c < Size; c++)
                if (grid[r, c] == type)
                {
                    grid[r, c] = 0;
                    tileObjects[r, c] = null;
                }
    }

    void Place(Vector2Int cell, int type)
    {
        int prev = grid[cell.y, cell.x];
        if (prev == 1 && tileObjects[cell.y, cell.x] != null)
        {
            Destroy(tileObjects[cell.y, cell.x]);
            tileObjects[cell.y, cell.x] = null;
        }
        if (prev == 2 || prev == 3 || prev == 4)
        {
            grid[cell.y, cell.x] = 0;
            tileObjects[cell.y, cell.x] = null;
        }

        if (type == 2 || type == 3 || type == 4) ClearSingletonFromGrid(type);

        grid[cell.y, cell.x] = type;
        float x = cell.x - 7;
        float z = cell.y - 7;

        switch (type)
        {
            case 0:
                break;
            case 1:
                if (blockPrefab != null)
                {
                    var go = Instantiate(blockPrefab, new Vector3(x, 0.5f, z), Quaternion.identity);
                    tileObjects[cell.y, cell.x] = go;
                }
                break;
            case 2:
                if (playerBlockObj != null)
                {
                    playerBlockObj.transform.position = new Vector3(x, 1f, z);
                    tileObjects[cell.y, cell.x] = playerBlockObj;
                }
                break;
            case 3:
                if (goalBlockObj != null)
                {
                    goalBlockObj.transform.position = new Vector3(x, 1f, z);
                    tileObjects[cell.y, cell.x] = goalBlockObj;
                }
                break;
            case 4:
                if (player != null)
                    player.transform.position = new Vector3(x, 1.875f, z);
                break;
        }
    }

    void LoadLevel(int[,] level)
    {
        for (int r = 0; r < Size; r++)
            for (int c = 0; c < Size; c++)
            {
                if (grid[r, c] == 1 && tileObjects[r, c] != null) Destroy(tileObjects[r, c]);
                grid[r, c] = 0;
                tileObjects[r, c] = null;
            }
        int rows = level.GetLength(0);
        int cols = level.GetLength(1);
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                if (level[r, c] != 0)
                    Place(new Vector2Int(c, r), level[r, c]);
    }

    void ExportLevel()
    {
        var sb = new StringBuilder();
        sb.AppendLine("public static int[,] customLevel = new int[,] {");
        for (int r = 0; r < Size; r++)
        {
            sb.Append("    { ");
            for (int c = 0; c < Size; c++)
            {
                int v = grid[r, c] == 4 ? 0 : grid[r, c];
                sb.Append(v);
                if (c < Size - 1) sb.Append(", ");
            }
            sb.AppendLine(" },");
        }
        sb.AppendLine("};");
        string text = sb.ToString();
        Debug.Log(text);
        string path = Path.Combine(Application.persistentDataPath, "customLevel.txt");
        File.WriteAllText(path, text);
        Debug.Log("Saved to " + path);
    }

    void OnGUI()
    {
        if (cursorLocked)
        {
            float cx = Screen.width * 0.5f;
            float cy = Screen.height * 0.5f;
            GUI.color = Color.white;
            GUI.DrawTexture(new Rect(cx - 10, cy - 2, 20, 4), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(cx - 2, cy - 10, 4, 20), Texture2D.whiteTexture);
        }

        GUIStyle style = new GUIStyle(GUI.skin.label) { fontSize = 28 };
        style.normal.textColor = Color.white;
        int lh = 36;
        if (playtesting)
        {
            GUI.Label(new Rect(20, 20, 1600, lh), "PLAYTESTING — P exit, R reset, L toggle cursor", style);
        }
        else
        {
            GUI.Label(new Rect(20, 20, 1600, lh), $"Brush: {BrushName(brush)}   (1=Block 2=Pushable 3=Goal 4=Player)", style);
            GUI.Label(new Rect(20, 20 + lh, 1600, lh), "LMB place, RMB erase, F1 export, F2-F5 load levelOne..Four, L cursor, P playtest", style);
            if (hoverCell.HasValue)
                GUI.Label(new Rect(20, 20 + lh * 2, 1600, lh), $"Cell: ({hoverCell.Value.x}, {hoverCell.Value.y})", style);
        }
    }

    static string BrushName(int t)
    {
        switch (t)
        {
            case 1: return "Block";
            case 2: return "Pushable";
            case 3: return "Goal";
            case 4: return "Player";
        }
        return "?";
    }
}
