using UnityEngine;

public class GoalBlockManager : MonoBehaviour
{
    public GameObject goalBlock;
    public GameObject playerBlock;

    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        if (IsOccupied(goalBlock.transform.position, playerBlock))
        {
        Debug.Log("Level Complete");
        }
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
