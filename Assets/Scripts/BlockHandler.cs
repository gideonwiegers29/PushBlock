using UnityEngine;
using System.Threading.Tasks;

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
        cube.transform.position = new Vector3(Mathf.Round(cube.transform.position.x), 1, Mathf.Round(cube.transform.position.z));
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }


    bool IsOccupied(Vector3 position, float radius = 0.1f) {
        Collider[] colliders = Physics.OverlapSphere(position, radius);
        Debug.Log(colliders);
        return colliders.Length > 0;
    }
    async void moveBlock(Collision collision) {
        if (collision.gameObject.transform == player.transform) {
            Vector3 normal = collision.GetContact(0).normal;
            Vector3 position = cube.transform.position;
            
            Debug.Log(next_position);
            int times_moved = 0;
            while (true)
            {
                if (IsOccupied(next_position) == false) {
                    await Task.Delay(2000);
                    transform.position = next_position;
                    times_moved += 1;
                } else if (times_moved > 8) {
                    break;
                }
            }
            
        }
    }

    void getNextPos(Vector3 normal, Vector3 position) {
        Vector3 next_position = new Vector3(Mathf.Round(position.x + normal.x), 1, Mathf.Round(position.z + normal.z));
        return next_position;
    }

    void OnCollisionEnter(Collision collision) {
        moveBlock(collision);
    }

}
