using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class ColliderOffset : MonoBehaviour
{
    #region PolygonColliders
    
    [MenuItem("Assets/SuperNami/PolygonCollider2D/1. Alignement")]
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

                        var x = v2.x;
                        var y = v2.y;
                        var rest = y % 1;
                        if (rest != 0)
                        {
                            x = Mathf.Round(v2.x);
                            y = Mathf.Round(v2.y);
                        }

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

    [MenuItem("Assets/SuperNami/PolygonCollider2D/2. Non Alignés +0.12")]
    private static void Offset12PolyPaths()
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
                        var pt = path[p];

                        var x = pt.x;
                        var y = pt.y;
                        var rest = pt.y % 1;

                        if (pt.y - 25f != 0 && pt.y + 25f != 0)
                        {
                            if (pt.y % 1 == 0)
                            {
                                y = pt.y + 0.82f;
                            }
                            else
                            {
                                x = Mathf.Round(pt.x);
                                y = Mathf.Round(pt.y);
                                y += 0.12f;
                            }
                        }

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
    
    [MenuItem("Assets/SuperNami/PolygonCollider2D/2. Non Alignés +0.82")]
    private static void Offset82PolyPaths()
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
                        var pt = path[p];

                        var x = pt.x;
                        var y = pt.y;
                        var rest = pt.y % 1;

                        if (pt.y - 25f != 0 && pt.y + 25f != 0)
                        {
                            if (pt.y % 1 == 0)
                            {
                                y = pt.y + 0.12f;
                            }
                            else
                            {
                                x = Mathf.Round(pt.x);
                                y = Mathf.Round(pt.y);
                                y += 0.82f;
                            }
                        }

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
    
    #endregion

    #region EdgeColliders
    
    [MenuItem("Assets/SuperNami/EdgeCollider2D/1. Alignement")]
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

                    var x = pt.x;
                    var y = pt.y;
                    var rest = pt.y % 1;
                    if (rest != 0 && rest != 0.82f && rest != 0.12f)
                    {
                        x = Mathf.Round(pt.x);
                        y = Mathf.Round(pt.y);
                    }

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

    [MenuItem("Assets/SuperNami/EdgeCollider2D/2. Non Alignés +0.12")]
    private static void Offset82EdgePoints2()
    {
        var gos = SceneManager.GetActiveScene().GetRootGameObjects();
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

                    var x = pt.x;
                    var y = pt.y;
                    var rest = pt.y % 1;

                    if (pt.y - 25f != 0 && pt.y + 25f != 0)
                    {
                        if (pt.y % 1 == 0)
                        {
                            y = pt.y + 0.82f;
                        }
                        else
                        {
                            x = Mathf.Round(pt.x);
                            y = Mathf.Round(pt.y);
                            y += 0.12f;
                        }
                    }

                    newPoints[pi] = new Vector2(x, y);
                }

                edge.points = newPoints;
            }
        }

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    [MenuItem("Assets/SuperNami/EdgeCollider2D/2. Non Alignés +0.82")]
    private static void Offset12EdgePoints2()
    {
        var gos = SceneManager.GetActiveScene().GetRootGameObjects();
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

                    var x = pt.x;
                    var y = pt.y;
                    var rest = pt.y % 1;

                    if (pt.y - 25f != 0 && pt.y + 25f != 0)
                    {
                        if (pt.y % 1 == 0)
                        {
                            y = pt.y + 0.12f;
                        }
                        else
                        {
                            x = Mathf.Round(pt.x);
                            y = Mathf.Round(pt.y);
                            y += 0.82f;
                        }
                    }

                    newPoints[pi] = new Vector2(x, y);
                }

                edge.points = newPoints;
            }
        }

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }
    #endregion
    
    #region BoxPoly
    
    [MenuItem("Assets/SuperNami/PolygonCollider2D/BOX +0.82 +0.12")]
    private static void OffsetBoxPolyPaths()
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
                        var pt = path[p];

                        var x = pt.x;
                        var y = pt.y;
                        var rest = pt.y % 1;

                        if (rest == 0)
                        {
                            if (y < 0)
                            {
                                y = pt.y + 0.82f;
                            }
                            else
                            {
                                y = pt.y + 0.12f;
                            }
                        }

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
    
    #endregion
}

