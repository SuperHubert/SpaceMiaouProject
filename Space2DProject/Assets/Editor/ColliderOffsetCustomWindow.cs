using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[CanEditMultipleObjects]
public class ColliderOffsetCustomWindow : EditorWindow
{
    private float levelOffsetInput = 0.12f;
    private float wallOffsetInput = 0.82f;
    
    [MenuItem("Window/Collider Offset")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<ColliderOffsetCustomWindow>("Collider Offset");
    }

    private void OnGUI()
    {
        GUILayout.Label("Edge Colliders :", EditorStyles.boldLabel);
        if (GUILayout.Button("Snap Edges and Offset", GUILayout.Width(200), GUILayout.Height(20)))
        {
            UpdatedAutoEdgeSnapperV2(Selection.gameObjects, wallOffsetInput, levelOffsetInput);
        }
        GUILayout.Space(20);
        
        GUILayout.Label("Polygon Colliders :", EditorStyles.boldLabel);
        if (GUILayout.Button("Snap Polygons and Offset", GUILayout.Width(200), GUILayout.Height(20)))
        {
            SnapPolyPathsV2(Selection.gameObjects,wallOffsetInput, levelOffsetInput);
        }
        
        if (GUILayout.Button("Snap Poly", GUILayout.Width(200), GUILayout.Height(20)))
        {
            SnapPolyPaths(Selection.gameObjects);
        }
        GUILayout.Space(20);
        
        GUILayout.Label("Fixes :", EditorStyles.boldLabel);
        if (GUILayout.Button("Invert Colliders", GUILayout.Width(200), GUILayout.Height(20)))
        {
            InvertColliders(Selection.gameObjects,wallOffsetInput, levelOffsetInput);
        }
        GUILayout.Space(20);
        
        levelOffsetInput = EditorGUILayout.Slider("Level Offset", levelOffsetInput,0f,1f);
        wallOffsetInput = EditorGUILayout.Slider("Wall Offset", wallOffsetInput,0f,1f);
        GUILayout.Space(20);
        
        GUILayout.Label("TileMap :", EditorStyles.boldLabel);
        if (GUILayout.Button("Bind Tilemap", GUILayout.Width(200), GUILayout.Height(20)))
        {
            BindTilemap(Selection.gameObjects);
        }
        if (GUILayout.Button("Fill Tilemap (Experimental)", GUILayout.Width(200), GUILayout.Height(20)))
        {
            FillTilemap(Selection.gameObjects);
        }
    }

    private static void BindTilemap(IEnumerable<GameObject> gos)
    {
        foreach (var go in gos)
        {
            var tilemap = go.GetComponentInChildren<Tilemap>(false);

            TileBase unwalkableTile = tilemap.GetTilesBlock(tilemap.cellBounds)[0];

            if (unwalkableTile == null)
            {
                if (tilemap.HasTile(new Vector3Int(-50, -5, 0)))
                {
                    unwalkableTile = tilemap.GetTile(new Vector3Int(-50, -5, 0));
                }
                else if (tilemap.HasTile(new Vector3Int(-50, 4, 0)))
                {
                    unwalkableTile = tilemap.GetTile(new Vector3Int(-50, 4, 0));
                }
                else if (tilemap.HasTile(new Vector3Int(49, -5, 0)))
                {
                    unwalkableTile = tilemap.GetTile(new Vector3Int(49, -5, 0));
                }
                else if (tilemap.HasTile(new Vector3Int(49, 4, 0)))
                {
                    unwalkableTile = tilemap.GetTile(new Vector3Int(49, 4, 0));
                }
                else if (tilemap.HasTile(new Vector3Int(-5, 49, 0)))
                {
                    unwalkableTile = tilemap.GetTile(new Vector3Int(-5, 49, 0));
                }
                else if (tilemap.HasTile(new Vector3Int(4, 49, 0)))
                {
                    unwalkableTile = tilemap.GetTile(new Vector3Int(4, 49, 0));
                }
                else if (tilemap.HasTile(new Vector3Int(-5, -50, 0)))
                {
                    unwalkableTile = tilemap.GetTile(new Vector3Int(-5, -50, 0));
                }
                else if (tilemap.HasTile(new Vector3Int(4, -50, 0)))
                {
                    unwalkableTile = tilemap.GetTile(new Vector3Int(4, -50, 0));
                }
            }


            tilemap.SetTile(new Vector3Int(-50, 49, 0), unwalkableTile);
            tilemap.SetTile(new Vector3Int(-50, -50, 0), unwalkableTile);
            tilemap.SetTile(new Vector3Int(49, 49, 0), unwalkableTile);
            tilemap.SetTile(new Vector3Int(49, -50, 0), unwalkableTile);
            
            tilemap.CompressBounds();
        }
    }
    
    private static void FillTilemap(IEnumerable<GameObject> gos)
    {
        foreach (var go in gos)
        {
            var tilemap = go.GetComponentInChildren<Tilemap>(false);

            TileBase unwalkableTile = tilemap.GetTilesBlock(tilemap.cellBounds)[0];

            if (unwalkableTile == null)
            {
                if (tilemap.HasTile(new Vector3Int(-50,-5,0)))
                {
                    unwalkableTile = tilemap.GetTile(new Vector3Int(-50, -5, 0));
                }
                else if(tilemap.HasTile(new Vector3Int(-50,4,0)))
                {
                    unwalkableTile = tilemap.GetTile(new Vector3Int(-50,4,0));
                }
                else if(tilemap.HasTile(new Vector3Int(49,-5,0)))
                {
                    unwalkableTile = tilemap.GetTile(new Vector3Int(49,-5,0));
                }
                else if(tilemap.HasTile(new Vector3Int(49,4,0)))
                {
                    unwalkableTile = tilemap.GetTile(new Vector3Int(49,4,0));
                }
                else if(tilemap.HasTile(new Vector3Int(-5,49,0)))
                {
                    unwalkableTile = tilemap.GetTile(new Vector3Int(-5,49,0));
                }
                else if(tilemap.HasTile(new Vector3Int(4,49,0)))
                {
                    unwalkableTile = tilemap.GetTile(new Vector3Int(4,49,0));
                }
                else if(tilemap.HasTile(new Vector3Int(-5,-50,0)))
                {
                    unwalkableTile = tilemap.GetTile(new Vector3Int(-5,-50,0));
                }
                else if(tilemap.HasTile(new Vector3Int(4,-50,0)))
                {
                    unwalkableTile = tilemap.GetTile(new Vector3Int(4,-50,0));
                }
            }
            
            tilemap.SetTile(new Vector3Int(-50,49,0), unwalkableTile);
            tilemap.SetTile(new Vector3Int(-50,-50,0), unwalkableTile);
            tilemap.SetTile(new Vector3Int(49,49,0), unwalkableTile);
            tilemap.SetTile(new Vector3Int(49,-50,0), unwalkableTile);
            
            tilemap.CompressBounds();

            tilemap.FloodFill(new Vector3Int(-49,-49,0), unwalkableTile);
            tilemap.FloodFill(new Vector3Int(-49,48,0), unwalkableTile);
            tilemap.FloodFill(new Vector3Int(48,48,0), unwalkableTile);
            tilemap.FloodFill(new Vector3Int(48,-49,0), unwalkableTile);
            
        }
    }

    private static void InvertColliders(IEnumerable<GameObject> gos, float inWallOffset, float inLevelOffset)
    {
        foreach (var go in gos)
        {
            var polys = go.GetComponentsInChildren<PolygonCollider2D>(false);

            foreach (var poly in polys)
            {
                poly.gameObject.layer = 13;
                if (poly.gameObject.transform.parent.gameObject.layer != 13)
                {
                    poly.gameObject.transform.parent.gameObject.layer = 13;
                }

                for (var n = 0; n < poly.pathCount; n++)
                {
                    var path = poly.GetPath(n);

                    float rest;
                    for (var p = 0; p < path.Length; p++)
                    {
                        
                        rest = path[p].y % 1;
                        if (inWallOffset - 0.01f < rest && rest < inWallOffset + 0.01f )
                        {
                            path[p].y = Mathf.RoundToInt(path[p].y);
                            path[p].y -= 1;
                            path[p].y += inLevelOffset;
                        }
                        else if (inLevelOffset - 0.01f < rest && rest < inLevelOffset + 0.01f )
                        {
                            path[p].y = Mathf.RoundToInt(path[p].y);
                            path[p].y += inWallOffset;
                        }
                        else
                        {
                            Debug.Log(-1*rest);
                            rest *= -1;

                            if (1 - inLevelOffset - 0.01f < rest && rest < 1 - inLevelOffset + 0.01f)
                            {
                                path[p].y = Mathf.RoundToInt(path[p].y);
                                path[p].y += inWallOffset;
                            }
                            else if (1 - inWallOffset - 0.01f < rest && rest < 1 - inWallOffset + 0.01f)
                            {
                                path[p].y = Mathf.RoundToInt(path[p].y);
                                path[p].y -= 1;
                                path[p].y += inLevelOffset;
                            }
                            
                        }
                    }

                    if (path[0] == path[1])
                    {
                        path[0].y += 1;
                    }
                    
                    poly.SetPath(n, path);
                    
                }
            }
            
            var edges = go.GetComponentsInChildren<EdgeCollider2D>(false);

            foreach (var edge in edges)
            {
                edge.gameObject.layer = 13;
                if (edge.gameObject.transform.parent.gameObject.layer != 13)
                {
                    edge.gameObject.transform.parent.gameObject.layer = 13;
                }
                
                var points = edge.points;

                float rest;
                for (var pi = 0; pi < points.Length; pi++)
                {
                    rest = points[pi].y % 1;
                    if (inWallOffset - 0.01f < rest && rest < inWallOffset + 0.01f )
                    {
                        points[pi].y = Mathf.RoundToInt(points[pi].y);
                        points[pi].y -= 1;
                        points[pi].y += inLevelOffset;
                    }
                    else if (inLevelOffset - 0.01f < rest && rest < inLevelOffset + 0.01f )
                    {
                        points[pi].y = Mathf.RoundToInt(points[pi].y);
                        points[pi].y += inWallOffset;
                    }
                    else
                    {
                        Debug.Log(-1*rest);
                        rest *= -1;

                        if (1 - inLevelOffset - 0.01f < rest && rest < 1 - inLevelOffset + 0.01f)
                        {
                            points[pi].y = Mathf.RoundToInt(points[pi].y);
                            points[pi].y += inWallOffset;
                        }
                        else if (1 - inWallOffset - 0.01f < rest && rest < 1 - inWallOffset + 0.01f)
                        {
                            points[pi].y = Mathf.RoundToInt(points[pi].y);
                            points[pi].y -= 1;
                            points[pi].y += inLevelOffset;
                        }
                            
                    }
                }
                
                edge.points = points; 
            }
        }
    }
    
    private static void SnapPolyPathsV2(IEnumerable<GameObject> gos, float inWallOffset, float inLevelOffset)
    {
        foreach (var go in gos)
        {
            var polys = go.GetComponentsInChildren<PolygonCollider2D>(false);
            
            foreach (var poly in polys)
            {
                poly.gameObject.layer = 13;
                if (poly.gameObject.transform.parent.gameObject.layer != 13)
                {
                    poly.gameObject.transform.parent.gameObject.layer = 13;
                }
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
                    
                    var highestPoint = 0;
                    for (var p = 0; p < path.Length; p++)
                    {
                        if (path[p].y > path[highestPoint].y)
                        {
                            highestPoint = p;
                        } 
                        
                    }

                    var newPoints = new Vector2[path.Length];
                    
                    for (int i = highestPoint; i < path.Length; i++)
                    {
                        newPoints[i - highestPoint] = path[i];
                    }
                    for (int i = 0 ; i < highestPoint; i++)
                    {
                        newPoints[path.Length - highestPoint + i] = path[i];
                    }
                    
                    poly.SetPath(n, newPoints);
                    
                    path = poly.GetPath(n);
                    
                    if (Mathf.RoundToInt(path[0].y) != Mathf.RoundToInt(path[1].y))
                    {
                        for (int i = 1; i < path.Length; i++)
                        {
                            newPoints[i - 1] = path[i];
                        }
                        for (int i = 0 ; i < 1; i++)
                        {
                            newPoints[path.Length - 1 + i] = path[i];
                        }

                        System.Array.Reverse(newPoints);
                        poly.SetPath(n, newPoints);
                    }

                    if (Mathf.RoundToInt(path[0].x) > Mathf.RoundToInt(path[1].x))
                    {
                        offset = inWallOffset;
                        offset2 = inLevelOffset;
                    }
                    
                    for (var pi = 1; pi < path.Length; pi++)
                    {
                        var currentPoint = path[pi];
                        var previousPoint = path[pi - 1];

                        var x = currentPoint.x;
                        float y;
                        if (x > previousPoint.x)
                        {
                            y = currentPoint.y + offset;
                            newPoints[pi] = new Vector2(x, y);
                            newPoints[pi - 1] = new Vector2(previousPoint.x, y);
                        }
                        else
                        {
                            y = currentPoint.y + offset2;
                            if (y < 25)
                            {
                                newPoints[pi] = new Vector2(x, y); 
                            }
                        }
                    }

                    newPoints[0].y = newPoints[1].y;
                    
                    poly.SetPath(n, newPoints);
                    
                }
            }
        }
    }
    
    private static void SnapPolyPaths(IEnumerable<GameObject> gos)
    {
        foreach (var go in gos)
        {
            var polys = go.GetComponentsInChildren<PolygonCollider2D>(false);
            
            foreach (var poly in polys)
            {
                poly.gameObject.layer = 13;
                poly.isTrigger = true;
                if (poly.gameObject.transform.parent.gameObject.layer != 13)
                {
                    poly.gameObject.transform.parent.gameObject.layer = 13;
                }
            }
            
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
        }
    }
    
    private static void UpdatedAutoEdgeSnapperV2(IEnumerable<GameObject> gos, float inWallOffset, float inLevelOffset)
    {
        foreach (var go in gos)
        {
            var edges = go.GetComponentsInChildren<EdgeCollider2D>(false);

            foreach (var edge in edges)
            {
                edge.gameObject.layer = 13;
                if (edge.gameObject.transform.parent.gameObject.layer != 13)
                {
                    edge.gameObject.transform.parent.gameObject.layer = 13;
                }
            }

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

                var offset = inLevelOffset;
                var offset2 = inWallOffset;

                var firstX = (int) Mathf.Round(points[0].x); 
                var lastX = (int) Mathf.Round(points[points.Length - 1].x);
                
                if (firstX != lastX)
                {
                    Debug.Log("First X != Last X");
                    if (lastX < firstX)
                    {
                        Debug.Log("First X > Last X, inverting");
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
                    
                    points = edge.points;

                    var startsMidLeft = (points[0].y > 0 && points[0].y < 5 && points[0].x < 0);
                    var startsBotLeft = (points[0].y < -24 && points[0].x < 0);
                    var startsTopRight = (points[0].y > 24 && points[0].x > 0);

                    if (startsMidLeft || startsBotLeft || startsTopRight)
                    {
                        offset = inWallOffset;
                        offset2 = inLevelOffset;
                    }

                }
                else
                {
                    var firstY = (int) Mathf.Round(points[0].y); 
                    var lastY = (int) Mathf.Round(points[points.Length - 1].y);
                    if (lastX < firstX)
                    {
                        Debug.Log("First Y > Last Y, inverting");
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
                    
                    points = edge.points;

                    var startsMidRight = (0 < points[0].y && points[0].y < 5 && points[0].x > 0);
                    var startsBotLeft = (points[0].y < -24 && points[0].x < 0);

                    if (startsMidRight || startsBotLeft )
                    {
                        offset = inWallOffset;
                        offset2 = inLevelOffset;
                    }
                }
                
                points = edge.points;
                newPoints = points;
                
                for (var pi = 1; pi < points.Length; pi++)
                {
                    var currentPoint = points[pi];
                    var previousPoint = points[pi - 1];

                    var x = currentPoint.x;
                    float y;
                    if (x > previousPoint.x)
                    {
                        y = currentPoint.y + offset;
                        newPoints[pi] = new Vector2(x, y);
                        newPoints[pi - 1] = new Vector2(previousPoint.x, y);
                    }
                    else
                    {
                        y = currentPoint.y + offset2;
                        if (y < 25)
                        {
                            newPoints[pi] = new Vector2(x, y); 
                        }
                    }
                }
                
                edge.points = newPoints;
            }
        }
    }
}
