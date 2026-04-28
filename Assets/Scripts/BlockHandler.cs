using UnityEngine;

public class BlockHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public GameObject player;
    public GameObject cube;
    private Rigidbody rb;


    // Update is called once per frame
    void Update()
    {
        cube.transform.position = new Vector3(Mathf.Round(cube.transform.position.x), Mathf.Round(cube.transform.position.y), Mathf.Round(cube.transform.position.z));
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.transform == player.transform) {
            Vector3 normal = collision.GetContact(0).normal;
        
            transform.Translate(new Vector3(Mathf.Round(normal.x), Mathf.Round(normal.y), Mathf.Round(normal.z)));
    }


    }
}
