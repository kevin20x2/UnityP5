using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    // Use this for initialization
    GameObject[] CameraList;
    int current_index;
    enum CameraStatus
    {
        Moving = 0,
        Normal
    }
    CameraStatus m_status;
    int stop_number;
    int current_number;
    Vector3 init_pos;
    Vector3 target_pos;
    Vector3 default_transform;
    Quaternion default_forward;
	void Start () {
        CameraList = GameObject.FindGameObjectsWithTag("Camera");
        current_index = 0;
        stop_number = 0;
        m_status = CameraStatus.Normal;
	}
    public void changeCamera(string object_name)
    {
        for(int i = 0;i<CameraList.Length;++i)
        {
            if (CameraList[i].name == object_name)
            {
                CameraList[i].SetActive(true);
                current_index = i;
                default_transform = CameraList[i].transform.position;
                default_forward = CameraList[i].transform.rotation;
            }
            else CameraList[i].SetActive(false);
        }
    }
    public GameObject get_current_camera()
    {
        return CameraList[current_index];

    }
    public void CloseToPoint(Vector3 pos , float dis,int frameNumber)
    {
        CameraList[current_index].transform.LookAt(pos);
        init_pos = CameraList[current_index].transform.position;
        target_pos = Vector3.Lerp(init_pos,pos,dis/Vector3.Distance(init_pos,pos));
        stop_number = frameNumber;
        current_number = 0;
        m_status = CameraStatus.Moving;
        
    }
    public void reset_position()
    {
        CameraList[current_index].transform.position = default_transform;
        CameraList[current_index].transform.rotation = default_forward;

        Debug.Log("reset position");

    }
	
	// Update is called once per frame
	void Update () {
        if(m_status == CameraStatus.Moving)
        {
            if (current_number < stop_number)
            {
                float tt = 1.0f * current_number / stop_number;
                CameraList[current_index].transform.position =
                    Vector3.Lerp(init_pos, target_pos, tt);
                current_number++;
            }
            else m_status = CameraStatus.Normal;
        }
		
	}
}
