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
            StartCoroutine(WaitingForCreate(dtime,vec[i],base_pos +new Vector3(dx,-dx/20f,0)));
            dtime += 0.15f;
            dx += 3.0f/scale_num;
        }
        StartCoroutine(WaitingForDel(1.5f));

    }
    void CreateObject(int num,Vector3 pos)
    {
        GameObject obj = new GameObject();
        obj.transform.position = pos;
        obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        obj.AddComponent<NumberMesh>();
        obj.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Custom/NumberShader"));
        obj.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load(num_str[num]) as Texture;
        GameObject camera = GameObject.Find("CameraController").GetComponent<CameraControl>().get_current_camera();
        obj.transform.position = pos + new Vector3(0,0.8f,0);
        obj.transform.LookAt(camera.transform.position);
        obj.transform.Translate(Vector3.forward*1.0f);
    //    obj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        obj.transform.Rotate(Vector3.up, 180f);
        obj.transform.Rotate(Vector3.right, -90f);

        obj.AddComponent<TestScale>();
        num_list.Add(obj);
    }
    // Update is called once per frame
    IEnumerator WaitingForCreate(float _time,int num,Vector3 pos)
    {
        yield return new WaitForSeconds(_time);
        CreateObject(num, pos);
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
