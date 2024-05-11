using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AsteroidGenerator : MonoBehaviour
{
    Point[,,] map = null;

    [SerializeField] float isoLevel = 0.5f;
    [SerializeField] float noiseScale = 0.01f;

    [SerializeField] [Min(10)] int tiles = 30;
    [SerializeField] int tileSize = 5;

    float radius = 0;


    Mesh mesh;

    List<Vector3> verts = new List<Vector3>();
    List<Color> colors = new List<Color>();
    List<Vector2> uvs = new List<Vector2>();
    List<int> tris = new List<int>();
    int buffer = 0;

    void Start()
    {
        radius = Random.Range(10.0f, (float)tiles - 2);

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        map = new Point[tiles, tiles, tiles];
        CreateMap();
        MarchingCubes();
        UpdateMesh();

    }


    public void RemoveBlock(Vector3 hit)
    {
        Vector3 localPos = hit - transform.position;
        
        Point closestSpace = new Point();
        for (int x = 0; x < tiles; x++)
        {
            for (int y = 0; y < tiles; y++)
            {
                for (int z = 0; z < tiles; z++)
                {
                    if (map[x, y, z].active)
                    {
                        if (Vector3.Distance(localPos, map[x, y, z].position) < Vector3.Distance(localPos, closestSpace.position))
                        {
                            closestSpace = map[x, y, z];
                        }
                    }
        
                }
            }
        }
        
        map[closestSpace.x, closestSpace.y, closestSpace.z].active = false;
        

        if (BlocksGone())
        {
            Destroy(gameObject);
        }
        else
        {
            uvs.Clear();
            verts.Clear();
            tris.Clear();
            colors.Clear();
            buffer = 0;
            MarchingCubes();
            UpdateMesh();
        }


    }
    
    void CreateMap()
    {
        for (int x = 0; x < tiles; x++)
        {
            for (int y = 0; y < tiles; y++)
            {
                for (int z = 0; z < tiles; z++)
                {
                    map[x, y, z] = new Point();
                    map[x, y, z].x = x;
                    map[x, y, z].y = y;
                    map[x, y, z].z = z;

                    map[x, y, z].position = new Vector3(x - tiles / 2, y - tiles / 2, z - tiles / 2) * (tileSize * 2);
                    float distanceToCenter = Vector3.Distance(transform.position + map[x, y, z].position, transform.position);

                    map[x, y, z].value = SolarSystem.noise.Evaluate((transform.position + map[x, y, z].position) * noiseScale);
                    if (distanceToCenter < radius * tileSize)
                    {
                        if (map[x, y, z].value >= isoLevel)
                        {
                            map[x, y, z].active = true;
                        }
                        if (y == tiles - 2 || y == 0 || x == 0 || x == tiles - 2 || z == 0 || z == tiles - 2)
                        {
                            map[x, y, z].active = false;
                        }
                    }

                }
            }
        }
    }
    
    void MarchingCubes()
    {
        for(int x = tiles; x > 0; x--)
        {
            for(int y = tiles; y > 0; y--)
            {
                for (int z = tiles; z > 0; z--)
                {
                    if(x < tiles - 1 && y < tiles - 1 && z < tiles - 1)
                    {
                        Point[] points = new Point[]
                        {
                            map[x,y,z-1],
                            map[x-1,y,z-1],
                            map[x-1,y,z],
                            map[x,y,z],
                            map[x,y-1,z-1],
                            map[x-1,y-1,z-1],
                            map[x-1,y-1,z],
                            map[x,y-1,z],
                        };
                        int cubeIndex = Point.GetState(points);

                        int[] triangulation = MarchingCubesTables.triTable[cubeIndex];
                        foreach(int edgeIndex in triangulation)
                        {
                            if(edgeIndex > -1)
                            {
                                int a = MarchingCubesTables.edgeConnections[edgeIndex][0];
                                int b = MarchingCubesTables.edgeConnections[edgeIndex][1];

                                Vector3 vertexPos = Point.GetMidPoint(points[a], points[b]);

                                verts.Add(vertexPos);
                                tris.Add(buffer);
                                buffer++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
    
    bool BlocksGone()
    {
        for (int x = 0; x < tiles; x++)
        {
            for (int y = 0; y < tiles; y++)
            {
                for (int z = 0; z < tiles; z++)
                {
                    if (map[x, y, z].active) return false;
                }
            }
        }

        return true;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.colors = colors.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
