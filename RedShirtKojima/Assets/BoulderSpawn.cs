using UnityEngine;
using System.Collections;

public class BoulderSpawn : MonoBehaviour {
    public GameObject Boulder;
   
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Instantiate(Boulder, transform.position, transform.rotation);
        Destroy(Boulder, 5.0f);
	}

   }