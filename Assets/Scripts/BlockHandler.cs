using UnityEngine;

public class BlockHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public GameObject player;

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "Player") {
            Vector3 normal = collision.GetContact(0).normal;
            transform.Translate(normal);
    }

    // Update is called once per frame
    void Update()
    {

    }

    }
}
