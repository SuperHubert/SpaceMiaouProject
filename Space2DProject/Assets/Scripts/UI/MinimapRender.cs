using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapRender : MonoBehaviour
{
    public Vector2 CoverSize;
    
    [SerializeField] private List<Vector3> vertices = new List<Vector3>();
    [SerializeField] private List<int> triangles = new List<int>();
    private List<Vector2> uv = new List<Vector2>();
    
    void Start()
    {
        CreateMesh();
    }

    void CreateMesh()
    {
        Mesh mesh = new Mesh();
        
        vertices.Add(new Vector3(-CoverSize.x/2,-CoverSize.y/2,0));
        vertices.Add(new Vector3(-CoverSize.x/2,CoverSize.y/2,0));
        vertices.Add(new Vector3(CoverSize.x/2,CoverSize.y/2,0));
        vertices.Add(new Vector3(CoverSize.x/2,-CoverSize.y/2,0));

        for (int i = 0; i < vertices.Count; i++)
        {
            uv.Add(Vector2.zero);
        }
        
        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);
        triangles.Add(0);
        triangles.Add(2);
        triangles.Add(3);
        
        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 1000);
        
        GetComponent<MeshFilter>().mesh = mesh;
    }
    
}
