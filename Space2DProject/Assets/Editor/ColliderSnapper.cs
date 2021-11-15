using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class MenuTest : MonoBehaviour
{
    [MenuItem("Assets/Simiancraft/PolygonCollider2D Snapper")]
    private static void SnapPolyPaths()
    {
        var gos = SceneManager.GetActiveScene().GetRootGameObjects();
        var polygonCount = 0;
        var pathCount = 0;
        foreach (var go in gos)
        {
            var polys = go.GetComponentsInChildren<PolygonCollider2D>(false);
            foreach (var poly in polys)
            {
                for (var n = 0; n < poly.pathCount; n++)
                {
                    var path = poly.GetPath(n);
                    for (var p = 0; p < path.Length; p++)
                    {
                        var v2 = path[p];
                        var x = Mathf.Round(v2.x * 2f) / 2f;
                        var y = Mathf.Round(v2.y * 2f) / 2f;
                        path[p] = new Vector2(x, y);
                        pathCount++;
                    }
                    poly.SetPath(n, path);
                }
                polygonCount++;
            }
        }
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        Debug.LogFormat("Snapped {0} paths across {1} poly colliders.", pathCount, polygonCount);
    }
 
    [MenuItem("Assets/Simiancraft/EdgeCollider2D Snapper")]
    private static void SnapEdgePoints()
    {
        var gos = SceneManager.GetActiveScene().GetRootGameObjects();
        var edgeColliderCount = 0;
        var pointCount = 0;
        foreach (var go in gos)
        {
            var edges = go.GetComponentsInChildren<EdgeCollider2D>(false);
            foreach (var edge in edges)
            {
                var points = edge.points;
                var newPoints = new Vector2[points.Length];
                for (var pi = 0; pi < points.Length; pi++)
                {
                    var pt = points[pi];
                    //var x = Mathf.Round(pt.x * 2f) / 2f;
                    //var y = Mathf.Round(pt.y * 2f) / 2f;
                    var x = Mathf.Round(pt.x);
                    var y = Mathf.Round(pt.y);
                    newPoints[pi] = new Vector2(x, y);
                    pointCount++;
                }
                edge.points = newPoints;
                edgeColliderCount++;
            }
        }
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        Debug.LogFormat("Snapped {0} points across {1} edge colliders.", pointCount, edgeColliderCount);
    }
}
