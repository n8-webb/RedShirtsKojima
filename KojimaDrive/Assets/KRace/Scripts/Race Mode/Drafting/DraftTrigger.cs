using UnityEngine;
using System.Collections;

public class DraftTrigger : MonoBehaviour {

    float startScale;   //Start scale of draft trigger
    float triggerTime;  //Time it takes to scale to 0
    float scaleTime;    //Scale to transform object by

    bool dead;

	// Use this for initialization
	void Start () {
        //Time for trigger to fade away
        triggerTime = 3.0f;

        //Maths to calculate scale time
        startScale = transform.localScale.x;
        scaleTime = triggerTime / startScale;

        dead = false;

	}
	
	// Update is called once per frame
	void Update () {
        //If scale is <= 0 the object has shrunk to nothing, destroy.
        if(transform.localScale.x <= 0.0f)
        {
            dead = true;
        }
        else
        {
            //Calculate how much to scale this update
            float scaleBy = Time.deltaTime / scaleTime;

            //Scale
            transform.localScale -= new Vector3(scaleBy, scaleBy, scaleBy);
        }
    }

    public bool getDead()
    {
        return dead;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
