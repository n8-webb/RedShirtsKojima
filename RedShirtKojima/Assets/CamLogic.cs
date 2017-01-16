//using UnityEngine;
//using System.Collections;

//public class CamLogic : MonoBehaviour {

//    Camera carCamera = new Camera();
//    Vector3 camTargetPos;
//    Vector3 camOffsetPos;
//    Quaternion camTargetRot;
//    Quaternion camOffsetRot;

//    // Use this for initialization
//    void Start ()
//    {
//        camOffsetPos = new Vector3(0.0f, 0.23f, 0.045f);
//        //transform.rotation = Quaternion.LookRotation(camTargetRot, Vector3.up);
//        transform.rotation = Quaternion.Euler(camTargetRot.x, 1.0f, camTargetRot.z);
//	}
	
//	// Update is called once per frame
//	void Update ()
//    {
//        CameraLogic();
//    }

//    void CameraLogic()
//    {
//        camTargetPos = transform.position - camOffsetPos;

//        transform.localPosition = camTargetPos;
        
//        //if (camTargetPos != transform.localPosition)
//        //{

//        //}
//        //camTargetRot = transform.rotation - camOffsetRot;
//    }
//}
