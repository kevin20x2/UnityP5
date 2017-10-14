using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusController : MonoBehaviour {

    // Use this for initialization
    BloodControl bc;
    Material redmat;
    Material bulemat;
    float max_out = 3.0f;
    float min_out = -1.0f;
    float normal_out = 0f;
    float current_out = 0.0f;
    float step = 0.4f;
    enum FocusStatus
    {
        Bigger = 0,
        Smaller ,
        Normal
    }
    FocusStatus m_status;
	void Start () {
        //  gameObject.SetActive(false);
        bc = GameObject.Find("BloodBar").GetComponent<BloodControl>();
        redmat = GetComponent<MeshRenderer>().material;
        bulemat = GameObject.Find("bule").GetComponent<MeshRenderer>().material;
        m_status = FocusStatus.Normal;

		
	}
    public void Foucus(GameObject Enermy)
    {
        gameObject.SetActive(true);
        GameObject camera = GameObject.Find("CameraController").GetComponent<CameraControl>().get_current_camera();
        transform.position = Enermy.transform.position + new Vector3(0,0.8f,0);
        transform.LookAt(camera.transform.position);
        transform.Translate(Vector3.forward*1.0f);

        transform.Rotate(Vector3.forward, 90f);
        transform.Rotate(Vector3.right, 90f);
        m_status = FocusStatus.Bigger;
        bc.change_object(Enermy);
      //  transform.localScale = new Vector3(0.5f,0.5f,0.5f);

    }
    public void ClearFoucs()
    {
        gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
        if(m_status == FocusStatus.Bigger)
        {
            if (current_out < max_out)
            {
                current_out += step;
                redmat.SetFloat("_Inrad", current_out);
                redmat.SetFloat("_Outrad", current_out);
                redmat.SetFloat("_Outheight", 5.0f);
                bulemat.SetFloat("_Inrad", current_out);
                bulemat.SetFloat("_Outrad", current_out);
                bulemat.SetFloat("_Outheight", 5.0f);
            }
            else m_status = FocusStatus.Smaller;
        }
        if(m_status == FocusStatus.Smaller)
        {
            if (current_out > min_out)
            {
                current_out -= step;
                redmat.SetFloat("_Inrad", current_out);
                redmat.SetFloat("_Outrad", current_out);
                redmat.SetFloat("_Outheight", current_out);
                bulemat.SetFloat("_Inrad", current_out);
                bulemat.SetFloat("_Outrad", current_out);
                bulemat.SetFloat("_Outheight", current_out);
            }
            else m_status = FocusStatus.Normal;
        }
        if(m_status == FocusStatus.Normal)
        {
            if(current_out <normal_out)
            {
                current_out += 0.1f;
                redmat.SetFloat("_Inrad", current_out);
                redmat.SetFloat("_Outrad", current_out);
                redmat.SetFloat("_Outheight", 1.0f);
                bulemat.SetFloat("_Inrad", current_out);
                bulemat.SetFloat("_Outrad", current_out);
                bulemat.SetFloat("_Outheight", 1.0f);

            }
        }

		
	}
}
