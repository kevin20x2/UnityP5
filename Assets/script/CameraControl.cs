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
	void Awake () {
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
    public void set_radio_blur(Vector3 position)
    {
        GameObject current_ca = get_current_camera();
        DepthTexture dt;
        if ((dt =current_ca.GetComponent<DepthTexture>()) != null)
        {
            //current_ca.GetComponent<Camera>().
            //DepthTexture dt = current_ca.AddComponent<DepthTexture>();
            dt.blurLevel = 10;
            Vector3 pos = current_ca.GetComponent<Camera>().WorldToViewportPoint(position);
            dt._du = pos.x;
            dt._dv = pos.y;
            Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>x:"+pos.x +" y:"+pos.y+" z:"+pos.z);
            if(dt.material == null)
            {
                dt.material = new Material(Shader.Find("Custom/Radio Blur"));

            }
          //  dt.enabled = false;
            //1dt.shader = Shader.Find("custom/Radio Blur");
           // dt.enabled = true;
        }
    }
    public void unset_radio_blur()
    {
        GameObject current_ca = get_current_camera();
        DepthTexture dt;
        if ((dt =current_ca.GetComponent<DepthTexture>()) != null)
        {
            dt.blurLevel = 1;
        }
 
    }
    public void changeCamera(GameObject obj)
    {
        for(int i =0;i<CameraList.Length;++i)
        {
            if(obj.Equals(CameraList[i]))
            {
                obj.SetActive(true);
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
