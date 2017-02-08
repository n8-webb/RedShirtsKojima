using UnityEngine;
using System.Collections;

public class DraftDetector : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collide");
        string collidedName = collision.gameObject.name;
        //string thisObject = transform.parent.name;

        if (collision.gameObject.GetComponent<DraftTrigger>())
        {
            Debug.Log("Hit: " + collidedName);
        }
    }
}