using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public enum DrawMode {Mesh}
    public DrawMode drawMode;
    public int gridSizeX;
    public int gridSizeY;

    public int gridCentreX;
    public int gridCentreY;

    public TerrainData terrainData;
    public NoiseData noiseData;
    public TextureData textureData;

    public Material terrainMaterial;

    [Range(0, 6)]
    public int editorLOD;

    public bool autoUpdate;

    public bool updateDisplay;

    float[,] falloffMap;

    MapChunk[,] mapChunks;

    void OnValuesUpdated()
    {
        if (!Application.isPlaying)
        {   
            DrawMapInEditor();
        }
    }

    public void DrawMapInEditor()
    {
        if (!Application.isPlaying)
        {
                
            

            MapData mapData = GenerateMapData(Vector2.zero);
            MapDisplay display = FindObjectOfType<MapDisplay>();
            //display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));

            if (updateDisplay)
            {
                display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, editorLOD, terrainData.useFlatShading));
            }
            else
            {
                SetupFullGridInEditor();
            }
            
        }
        
    }

    public void SetupFullGridInEditor()
    {

        for (int i = transform.childCount - 1; i > 0; i--)
        {
            Transform child = transform.GetChild(i);
            DestroyImmediate(child.GetComponent<MeshRenderer>());
            DestroyImmediate(child.GetComponent<MeshFilter>());
            DestroyImmediate(child.GetComponent<MeshCollider>());
            DestroyImmediate(child.gameObject);
        }




        mapChunks = new MapChunk[this.gridSizeX, this.gridSizeY];

        for (int x = 0; x < gridSizeX; x++) 
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                //Debug.Log("Generating " +  x.ToString() + " : " + y.ToString());
                mapChunks[x, y] = new MapChunk(new Vector2(x - gridCentreX, y - gridCentreY), mapChunkSize, this, this.terrainMaterial);
            }
        }
    }

    void OnTextureValuesUpdated()
    {
        textureData.ApplyToMaterial(terrainMaterial);
    }

    

    public int mapChunkSize
    {
        get
        {
            if (terrainData.useFlatShading)
            {
                return 95;
            }
            else
            {
                return 237;
            }
        }
    }

    public MapData GenerateMapData(Vector2 centre)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize + 4, mapChunkSize + 4, noiseData.seed, noiseData.noiseScale, noiseData.octaves, noiseData.persistance, noiseData.lacunarity, centre + noiseData.offset, noiseData.normalizeMode);

        textureData.UpdateMeshHeights(terrainMaterial, terrainData.minHeight, terrainData.maxHeight);

        return new MapData(noiseMap);
    }

    private void OnValidate()
    {
        Debug.Log("Validating Map Generator");
        if (terrainData != null)
        {
            terrainData.OnValuesUpdated -= OnValuesUpdated;
            terrainData.OnValuesUpdated += OnValuesUpdated;
        }

        if (noiseData != null)
        {
            noiseData.OnValuesUpdated -= OnValuesUpdated;
            noiseData.OnValuesUpdated += OnValuesUpdated;
        }

        if (textureData != null)
        {
            textureData.OnValuesUpdated -= OnTextureValuesUpdated;
            textureData.OnValuesUpdated += OnTextureValuesUpdated;
        }
    }

}

public struct MapData
{
    public readonly float[,] heightMap;

    public MapData(float[,] heightMap)
    {
        this.heightMap = heightMap;
    }
}

public class MapChunk
{
    public GameObject meshObject;
    Vector2 position;
    Bounds bounds;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    MapData mapData;
    bool mapDataReceived;

    public MapChunk(Vector2 coord, int size, MapGenerator generator, Material material) 
    {
        position = coord * size;
        bounds = new Bounds(position, Vector2.one * size);
        Vector3 positionV3 = new Vector3(position.x, 0, position.y);

        meshObject = new GameObject("Terrain Chunk");
        meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshFilter = meshObject.AddComponent<MeshFilter>();
        meshCollider = meshObject.AddComponent<MeshCollider>();

        meshRenderer.material = material;

        meshObject.transform.position = positionV3 * generator.terrainData.uniformScale;
        meshObject.transform.parent = generator.transform;
        meshObject.transform.localScale = Vector3.one * generator.terrainData.uniformScale;

        this.mapData = generator.GenerateMapData(position);

        MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, generator.terrainData.meshHeightMultiplier, generator.terrainData.meshHeightCurve, 0, generator.terrainData.useFlatShading);
        Mesh mesh = meshData.CreateMesh();
        meshCollider.sharedMesh = mesh;
        meshFilter.mesh = mesh;

    }

    public void ClearComponents() 
    {
        GameObject.Destroy(meshRenderer);
        GameObject.Destroy(meshFilter);
        GameObject.Destroy(meshCollider);
    }

}
