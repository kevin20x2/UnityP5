using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScale : MonoBehaviour {

    // Use this for initialization
    Vector3 current_scale;
    enum NumberStatus
    {
        Bigger = 0,
        Smaller,
        Normal

    };
    float step = 0.1f;
    float scale_num = 10.0f;
    NumberStatus m_status;
	void Start () {
        current_scale = new Vector3(1f / scale_num, 1f / scale_num, 1f / scale_num);
        transform.localScale = current_scale;
        m_status = NumberStatus.Bigger;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_status == NumberStatus.Bigger)
        {
            if (current_scale.x < 1.2f / scale_num)
            {
                current_scale += new Vector3(step / 5 / scale_num, step / scale_num, step / scale_num);
                transform.localScale = current_scale;
                transform.position += new Vector3(0f, 0f, 0.05f / scale_num);

            }
            else m_status = NumberStatus.Smaller;
        }
        if (m_status == NumberStatus.Smaller)
        {
            if (current_scale.x > 1.0f / scale_num)
            {
                current_scale -= new Vector3(step / 5 / scale_num, step / scale_num, step / scale_num);
                transform.localScale = current_scale;
                transform.position -= new Vector3(0f, 0f, 0.05f / scale_num);
            }
            else m_status = NumberStatus.Normal;
        }
}
}
