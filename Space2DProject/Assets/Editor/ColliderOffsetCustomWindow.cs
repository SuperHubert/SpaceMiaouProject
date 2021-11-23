using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
public class ColliderOffsetCustomWindow : EditorWindow
{
    //private List<PolygonCollider2D> PolyColList = new List<PolygonCollider2D>();
    //private List<EdgeCollider2D> EdgeColList = new List<EdgeCollider2D>();

    [MenuItem("Window/Collider Offset")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<ColliderOffsetCustomWindow>("Collider Offset");
    }

    private void OnGUI()
    {
        GUILayout.Label("Colliders :", EditorStyles.boldLabel);

        if (GUILayout.Button("Snap Colliders and Offset", GUILayout.Width(200), GUILayout.Height(20)))
        {
            UpdatedAutoEdgeSnapper(Selection.gameObjects, 0.82f, 0.12f);
        }

        GUILayout.Label("Polygon Colliders :", EditorStyles.boldLabel);

        if (GUILayout.Button("Snap Polygons and Offset", GUILayout.Width(200), GUILayout.Height(20)))
        {
            SnapPolyPaths(Selection.gameObjects,0.82f, 0.12f);
        }
    }


    private static void SnapPolyPaths(IEnumerable<GameObject> gos, float inWallOffset, float inLevelOffset)
    {
        foreach (var go in gos)
        {
            var polys = go.GetComponentsInChildren<PolygonCollider2D>(false);

            foreach (var poly in polys)
            {
                var pos = poly.gameObject.transform.position;
                poly.gameObject.transform.position = new Vector3(Mathf.Round(pos.x),Mathf.Round(pos.y),0f);
            }
            
            #region alignement

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
                    }

                    poly.SetPath(n, path);
                }
            }

            #endregion

            
            foreach (var poly in polys)
            {
                
                for (var n = 0; n < poly.pathCount; n++)
                {
                    
                    var path = poly.GetPath(n);
                    
                    var offset = inLevelOffset;
                    var offset2 = inWallOffset;
                    
                    #region main offset

                    for (var p = 1; p < path.Length-1; p++)
                    {
                        var currentPoint = path[p];
                        var previousPoint = path[p - 1];

                        if (!(currentPoint.x > previousPoint.x)) continue;
                        path[p].y = currentPoint.y + offset;
                        path[p-1].y = currentPoint.y + offset;
                    }
                    
                    poly.SetPath(n, path);

                    #endregion

                    #region second offset

                    for (var pi = 0; pi < path.Length; pi++)
                    {
                        var currentPoint = path[pi];
                        
                        if (currentPoint.y % 1f == 0 && currentPoint.y + 25f != 0 && currentPoint.y - 25f != 0)
                        {
                            path[pi].y = currentPoint.y + offset2;
                        }
                    }
                    
                    poly.SetPath(n, path);

                    #endregion
                    
                }
            }

        }
    }

    private static void UpdatedAutoEdgeSnapper(IEnumerable<GameObject> gos, float inWallOffset, float inLevelOffset)
    {
        foreach (var go in gos)
        {
            var edges = go.GetComponentsInChildren<EdgeCollider2D>(false);

            #region alignement

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
                    if (rest != 0)
                    {
                        x = Mathf.Round(pt.x);
                        y = Mathf.Round(pt.y);
                    }

                    newPoints[pi] = new Vector2(x, y);
                }

                edge.points = newPoints;
            }


            #endregion


            foreach (var edge in edges)
            {
                var points = edge.points;
                var newPoints = new Vector2[points.Length];

                #region make Edges start from Left

                if (!(points[0].x + 25f == 0 || points[0].x - 2f == 0))
                {
                    for (var pi = 0; pi < points.Length; pi++)
                    {
                        var pt = points[pi];
                        var x = pt.x;
                        var y = pt.y;

                        newPoints[pi] = new Vector2(x, y);
                    }

                    System.Array.Reverse(newPoints);

                    edge.points = newPoints;
                }

                #endregion

                points = edge.points;
                newPoints[0] = points[0];

                #region change base offset

                var offset = inLevelOffset;
                var offset2 = inWallOffset;

                if (newPoints[0].y > 0)
                {
                    offset = inWallOffset;
                    offset2 = inLevelOffset;
                }

                #endregion

                #region main offset

                for (var pi = 1; pi < points.Length; pi++)
                {
                    var currentPoint = points[pi];
                    var previousPoint = points[pi - 1];

                    var x = currentPoint.x;
                    var y = currentPoint.y;

                    if (x > previousPoint.x)
                    {
                        y = currentPoint.y + offset;
                        newPoints[pi - 1] = new Vector2(previousPoint.x, y);
                    }

                    newPoints[pi] = new Vector2(x, y);
                }

                points = newPoints;

                #endregion

                #region opposite offset

                newPoints = new Vector2[points.Length];

                for (var pi = 0; pi < points.Length; pi++)
                {
                    var currentPoint = points[pi];

                    var x = currentPoint.x;
                    var y = currentPoint.y;

                    if (currentPoint.y % 1f == 0 && currentPoint.y + 25f != 0 && currentPoint.y - 25f != 0)
                    {
                        y = currentPoint.y + offset2;
                    }

                    newPoints[pi] = new Vector2(x, y);
                }

                edge.points = newPoints;

                #endregion

            }
        }
    }
}
