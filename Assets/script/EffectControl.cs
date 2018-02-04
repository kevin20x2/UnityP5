using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectControl : MonoBehaviour {

    // Use this for initialization
    Material mat;
    int Frame_number = 30;
    const int image_number = 5;
    GameObject effect_mesh;
    int current_frame = 0;
	void Start () {
		
	}
    public void show_damage_effect(Vector3 position)
    {
         effect_mesh = GameObject.CreatePrimitive(PrimitiveType.Quad);
        GameObject current_camera = GameObject.Find("CameraController").GetComponent<CameraControl>().get_current_camera();
        effect_mesh.transform.localScale = new Vector3(4,4,4);
        effect_mesh.transform.position = position + new Vector3(0,1.5f,0);
        effect_mesh.transform.Translate(  0.6f*Vector3.Normalize(current_camera.transform.position - effect_mesh.transform.position));
       // effect_mesh.transform.LookAt(current_camera.transform);
       // effect_mesh.transform.Rotate(effect_mesh.transform.up, 180f);
      //  effect_mesh.transform.Translate(-effect_mesh.transform.forward);
      //  effect_mesh.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Custom/DamageEffect"));
        effect_mesh.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Custom/BillBoard"));
        //effect_mesh.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        mat = effect_mesh.GetComponent<MeshRenderer>().material;
        mat.mainTexture = Resources.Load("final") as Texture2D;
        current_frame = 0;
        //StartCoroutine(WaitingDel(effect_mesh,1.5f));


    }
    IEnumerator WaitingDel(GameObject mesh, float _time)
    {
        yield return new WaitForSeconds(_time);
        Destroy(mesh);

    }

    // Update is called once per frame
    void Update () {
        if (current_frame < Frame_number)
        {
            float tt = 1.0f * current_frame / Frame_number;
            if (mat != null)
                mat.SetFloat("_Uv", tt);

            current_frame++;
        }
        else if (effect_mesh != null) Destroy(effect_mesh);
		
	}
}
