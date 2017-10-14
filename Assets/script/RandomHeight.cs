using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHeight : MonoBehaviour {

    // Use this for initialization
    private Material mat;
	void Start () {
        mat = GetComponent<MeshRenderer>().material;
        mat.SetTexture("_Height",get_height_mp());
		
	}
	Texture2D get_height_mp()
    {
        int width = 300;
        Texture2D ans = new Texture2D(width,1,TextureFormat.RGBAFloat,false);
        ans.filterMode = FilterMode.Point;

        for(int i = 0;i<width;++i)
        {
            float height = (float)Random.Range(0,100)/100;
            ans.SetPixel(i, 0,new Color(height,1.0f,1.0f));

        }
        ans.Apply();
        return ans;



    }
	// Update is called once per frame
	void Update () {
		
	}
}
