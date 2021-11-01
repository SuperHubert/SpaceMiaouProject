using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapRender : MonoBehaviour
{
    public List<Vector3> vertices;
    public List<int> triangles;
    private List<Vector2> uv = new List<Vector2>();
    
    void Start()
    {
        Mesh mesh = new Mesh();

        for (int i = 0; i < vertices.Count; i++)
        {
            uv.Add(Vector2.zero);
        }
        
        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();
        

        GetComponent<MeshFilter>().mesh = mesh;
    }
    
}
