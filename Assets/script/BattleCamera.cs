using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour {

    // Use this for initialization
    Transform CameraPos;
	void Start () {
        if(GameObject.Find("CameraPos"))
        {
            CameraPos = GameObject.Find("CameraPos").transform;
        }
        transform.position = CameraPos.position;
        transform.forward = CameraPos.forward;
		
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position != CameraPos.position)
        {
            transform.position = CameraPos.position;
        }

    }
}
