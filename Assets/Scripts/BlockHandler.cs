using UnityEngine;

public class BlockHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public GameObject player;


    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.transform == player.transform) {
            Vector3 normal = collision.GetContact(0).normal;
            transform.Translate(normal);
    }


    }
}
