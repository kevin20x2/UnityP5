using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberMesh : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        GetComponent<MeshFilter>().mesh = CreateNumberMesh(12.0f,0.5f,2.0f,0.1f);
	}
    private void Start()
    {
        Awake();
    }
    List<Vector3> Vertex;
    List<Vector2> uv;
    List<int> triangles;
    Mesh CreateNumberMesh(float upwidth ,float downwidth,float height,float step)
    {
        Mesh m = new Mesh();
        m.name = "ScriptNumberMesh";
        Vertex = new List<Vector3>();
        uv = new List<Vector2>();
        triangles = new List<int>();
        float tana = (upwidth - downwidth) / 2 / height;
        int width_number =(int) (downwidth / step);
        int number = 0;
        for(float i = -height/2;i<height/2;i+= step)
        {
            float init_x = -downwidth / 2 - (tana*(i+height/2));
            float x_step = -2 * init_x/width_number;
            for(int j = 0;j<width_number;++j)
            {
                Vertex.Add(new Vector3(init_x+x_step*j,0,i));
                uv.Add(new Vector2(1.0f*j/width_number, (i+height/2)/height));
                if (number> width_number)
                {
                    if(j>0)
                    {
                        triangles.Add(number-1);
                        triangles.Add(number-width_number);
                        triangles.Add(number-1-width_number);
                        triangles.Add(number);
                        triangles.Add(number-width_number);
                        triangles.Add(number-1);
                        
                    }

                }
                number++;
            }

        }
    
        m.vertices = Vertex.ToArray();
        m.uv = uv.ToArray();
        m.triangles = triangles.ToArray();
        m.RecalculateNormals();
        m.RecalculateBounds();
        return m;
    }


	
	// Update is called once per frame
	void Update () {
		
	}
}
