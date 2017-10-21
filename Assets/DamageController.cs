using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour {

    // Use this for initialization
    string[] num_str = { "00","01","02","03","04","05","06","07","08","09"};
    List<GameObject> num_list;
	void Start () {
        //  showNumber(234,new Vector3 (0,0,0));
        num_list = new List<GameObject>();
      
	}
    float scale_num = 10.0f;
	public void showNumber(int number,Vector3 base_pos)
    {

        List<int> vec = new List<int>();
        while(number != 0)
        {
            int mod = number % 10;
            number /= 10;
            vec.Add(mod);
        }
        float dx = 0.0f;
        float dtime = 0.0f;
        for(int i = vec.Count -1;i>=0;--i)
        {
            //CreateObject(vec[i],new Vector3(dx,0,0));
            StartCoroutine(WaitingForCreate(dtime,vec[i],base_pos,vec.Count-i));
            dtime += 0.15f;
            dx += 3.0f/scale_num;
        }
        StartCoroutine(WaitingForDel(1.5f));

    }
    void CreateObject(int num,Vector3 pos,int number)
    {
        GameObject obj = new GameObject();
        obj.transform.position = pos;
        obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        obj.AddComponent<NumberMesh>();
        //  obj.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Mobile/Particles/Alpha Blended"));
        //mat.mainTexture = Resources.Load(num_str[num]) as Texture;
    //    obj.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("blood_empty") as Texture;
        GameObject camera = GameObject.Find("CameraController").GetComponent<CameraControl>().get_current_camera();
        obj.transform.position = pos + new Vector3(0,0.8f,0);
        obj.transform.LookAt(camera.transform.position);
        obj.transform.Translate(Vector3.forward*1.0f + Vector3.forward*number*0.1f);
        obj.transform.Translate(Vector3.left*number*0.2f);
    //    obj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        obj.transform.Rotate(Vector3.up, 180f);
        obj.transform.Rotate(Vector3.right, -90f);

        obj.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Custom/BloodShader"));
        Material mat = obj.GetComponent<MeshRenderer>().material;
        mat.SetTexture("_MainTex",Resources.Load(num_str[num]) as Texture);
        obj.AddComponent<TestScale>();
        num_list.Add(obj);
    }
    public void show_damage(GameObject battle_object,int maxihp,int initp,int top)
    {
        GameObject blood =  GameObject.CreatePrimitive(PrimitiveType.Plane);
        GameObject camera = GameObject.Find("CameraController").GetComponent<CameraControl>().get_current_camera();
        //obj.transform.position = pos + new Vector3(0,0.8f,0);
        blood.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
        blood.transform.position = battle_object.transform.position + new Vector3(0, 0.8f, 0);
        blood.transform.LookAt(camera.transform.position);
        blood.transform.Translate(Vector3.forward*1.1f);
        blood.transform.Translate(Vector3.down*0.3f);
        blood.transform.Translate(Vector3.left*0.7f);
        blood.transform.Rotate(new Vector3(90,0,0));
        blood.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Custom/BloodShader"));
        blood.AddComponent<BloodControl>();
        BloodControl bc = blood.GetComponent<BloodControl>();
        Material mat = blood.GetComponent<MeshRenderer>().material;
        mat.SetColor("_Color",new Color(0.0f,1.0f,192.0f/255));
        mat.SetFloat("_LineWidth",0.0266f);
        mat.mainTexture = Resources.Load("blood_empty") as Texture;
        bc.maxi_blood = maxihp;
        bc.current_blood = initp;
        bc.target_blood = top;
        mat.SetFloat("_Percent",1.0f*initp/maxihp);
        bc.current_frame = 0;
        StartCoroutine(WaitingBloodDel(blood,1.5f));

        


    }
    // Update is called once per frame
    IEnumerator WaitingBloodDel(GameObject blood,float _time)
    {
        yield return new WaitForSeconds(_time);
        Destroy(blood);
    }
    IEnumerator WaitingForCreate(float _time,int num,Vector3 pos,int number)
    {
        yield return new WaitForSeconds(_time);
        CreateObject(num, pos,number);
        //ani.SetBool("Moving", true);


    }
    IEnumerator WaitingForDel(float _time)
    {
        yield return new WaitForSeconds(_time);
        
        for(int i = num_list.Count-1;i>=0;--i)
        {
            Destroy(num_list[i]);
            num_list.Remove(num_list[i]);
        }
        num_list.Clear();

    }
	void Update () {
		
	}
}
