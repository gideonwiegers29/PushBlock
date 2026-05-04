using UnityEngine;
using System.Collections;

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
    public bool moving = false;
    public int moveDelay = 200;
    public int maxMoves = 17;


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
    IEnumerator moveBlock(Collision collision) {
        moving = true;
        Vector3 normal = collision.GetContact(0).normal;
        int times_moved = 0;
        while (times_moved <= maxMoves) {
            Vector3 position = cube.transform.position;
            Vector3 next_pos = getNextPos(normal, position);
            Debug.Log(next_pos);
            if (IsOccupied(next_pos) == false) {
                cube.transform.position = next_pos;
            } else {
                moving = false;
                Debug.Log("stopping");
                break;
            }
            yield return new WaitForSeconds(0.2f);
            times_moved += 1;
        }
        moving = false;
        
    }

    Vector3 getNextPos(Vector3 normal, Vector3 position) {
        Vector3 next_position = new Vector3(Mathf.Round(position.x + normal.x), 1, Mathf.Round(position.z + normal.z));
        return next_position;
    }

    bool IsOccupied(Vector3 position) {
        Collider[] hitColliders = Physics.OverlapSphere(position, 0.1f);
        return hitColliders.Length > 0;
    }


    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.transform == player.transform) {
            if (moving == false) {
                StartCoroutine(moveBlock(collision));
            } else {Debug.Log("already moving!");}
        } else {Debug.Log("not player!");}
    }

}
