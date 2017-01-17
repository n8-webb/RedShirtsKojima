using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    Camera carCamera;
    Vector3 camTargetPos;
    Vector3 camOffsetPos;
    Quaternion camTargetRot;
    Quaternion camOffsetRot;

    // Use this for initialization
    void Start () {
        CameraSetup();
	}
	
	// Update is called once per frame
	void Update () {
        CameraLogic();
    }

    void CameraSetup()
    {
        carCamera = gameObject.GetComponent<Camera>();
        carCamera.transform.Rotate(0.0f, 180.0f, 0.0f); //HERE
        camOffsetPos = new Vector3(0.0f, 0.23f, 0.045f);
        carCamera.transform.rotation = Quaternion.Euler(camTargetRot.x, 1.0f, camTargetRot.z);
    }

    void CameraLogic()
    {
        camTargetPos = transform.position - camOffsetPos;
        carCamera.transform.position = camTargetPos;
        

        //if (camTargetPos != carCamera.transform.position)
        //{

        //}
        ////camTargetRot = transform.rotation - camOffsetRot;
    }
}
