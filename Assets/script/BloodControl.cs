using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodControl : MonoBehaviour {

    // Use this for initialization
    public int maxi_blood = 100;
    public int target_blood;
    public int current_blood;
    Material mat;
    //CameraControl camera_control;
    int frame_number = 30;
    public int current_frame = 0;
	void Start () {
        mat = GetComponent<MeshRenderer>().material;
        current_blood = maxi_blood;
     //   camera_control = GameObject.Find("CameraController").GetComponent<CameraControl>();
       
      //  current_frame = frame_number;
     //   set_blood(50);
        
	}
    public void set_blood(int hp)
    {
        target_blood = hp;

    }
    public void change_object(GameObject enermy)
    {
        BattleUnit bu = enermy.GetComponent<BattleUnit>();
        maxi_blood = bu.init_hp;
        current_blood = bu.HP;
        target_blood = bu.HP;
        float percent = 1.0f * current_blood / maxi_blood;
        current_frame = 0;
        mat.SetFloat("_Percent", percent);


    }
    //public void show_damage(GameObject battle_object,int maxihp,int initp,int top)
    //{
    //    GameObject blood =  GameObject.CreatePrimitive(PrimitiveType.Plane);
    //    blood.transform.position = battle_object.transform.position + new Vector3(0, 1.0f, 0);
    //    blood.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    //    blood.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Custom/BloodShader"));
    //    blood.AddComponent<BloodControl>();
    //    BloodControl bc = blood.GetComponent<BloodControl>();
    //    Material mat = blood.GetComponent<MeshRenderer>().material;
        
    //    bc.maxi_blood = maxihp;
    //    bc.current_blood = initp;
    //    bc.target_blood = top：;
    //    mat.SetFloat("_Percent",1.0f*initp/maxihp);
    //    current_frame = 0;
        


    //}

    // Update is called once per frame
    void Update () {
        //if(current_blood  != target_blood)
        //{
        //    float percent = 1.0f * current_blood /maxi_blood;
        //    mat.SetFloat("_Percent",percent);
        //    if (current_blood < target_blood) current_blood += 1;
        //    else current_blood -= 1;
        //}
        if(current_frame < frame_number)
        {
            float minn = 1.0f * target_blood / maxi_blood;
            float maxn = 1.0f * current_blood / maxi_blood;
            mat.SetFloat("_Percent",Mathf.Lerp(maxn,minn,1.0f*current_frame/frame_number));
            current_frame++;

        }
		
	}
}
