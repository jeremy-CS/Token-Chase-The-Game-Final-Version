using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenAlert : MonoBehaviour
{
    private GameObject[] _chasers;
    

    // Start is called before the first frame update
    void Start()
    {
        _chasers = GameObject.FindGameObjectsWithTag("Chaser");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Seeker")
        {
            
            //Alert closest chaser of token being taken
            Chaser_AI chaser_ref = FindClosestChaser().GetComponent<Chaser_AI>();
            //chaser_ref;

            Destroy(this);
        }
    }

    public GameObject FindClosestChaser()
    {
        GameObject closestChaser = _chasers[0];
        
        foreach (GameObject chaser in _chasers)
        {
            if (Vector3.Distance(transform.position, chaser.transform.position) < Vector3.Distance(transform.position, chaser.transform.position))
            {
                closestChaser = chaser;
            }
        }
        return closestChaser;
    }
}
