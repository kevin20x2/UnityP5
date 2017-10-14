using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FrustumMesh : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        GetComponent<MeshFilter>().mesh = CreateFrustumMesh(2.0f,1.0f,1.0f,360,10);
	}
    List<Vector3> Vertex;
    List<Vector2> uv;
    List<int> triangles;
    Mesh CreateFrustumMesh(float uprad,float downrad,float height,int point_num,int ceil_num)
    {
        Mesh m = new Mesh();
        Vertex = new List<Vector3>();
        uv = new List<Vector2>();
        float tan_x = (uprad - downrad) / height;
        triangles = new List<int>();
        int current = 0;
        for(int i = 0;i<ceil_num;++i)
        {
            float dy = i * height / (ceil_num-1) - height / 2;
            float rad = (dy + height / 2) * tan_x + downrad;
            for (int j = 0;j<point_num;++j)
            {
                float angle = j*(2 * 3.141592653f) / point_num;
                Vertex.Add(new Vector3(rad*Mathf.Sin(angle),dy,rad*Mathf.Cos(angle)));
                uv.Add(new Vector2(1.0f*j/point_num,1.0f*i/ceil_num));
                if(i!= ceil_num-1)
                {
                    triangles.Add(current);
                    if (j == point_num - 1)
                    {
                        triangles.Add(current + 1 - point_num);
                        triangles.Add(current + 1);
                        triangles.Add(current);
                        triangles.Add(current + 1);
                        triangles.Add(current + point_num);
                    }
                    else
                    {
                        triangles.Add(current + 1);
                        triangles.Add(current+1+point_num);
                        triangles.Add(current);
                        triangles.Add(current+1+point_num);
                        triangles.Add(current+point_num);
                    }
                }
                current++;
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
