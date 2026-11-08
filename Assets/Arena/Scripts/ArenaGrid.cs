using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public sealed class ArenaGrid : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float width = 49f;
    [SerializeField, Min(0.1f)] private float depth = 49f;
    [SerializeField, Min(0.1f)] private float cellSize = 1f;
    [SerializeField, Min(0.001f)] private float lineWidth = 0.025f;
    [SerializeField] private Color lineColor = new Color(0.12f, 0.16f, 0.2f, 1f);

    private Mesh gridMesh;
    private Material gridMaterial;

    private void Awake()
    {
        // Keep the grid procedural so it does not require a separate mesh or material asset.
        BuildGrid();
    }

    private void OnDestroy()
    {
        DestroyGeneratedObject(gridMesh);
        DestroyGeneratedObject(gridMaterial);
    }

    private void BuildGrid()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Shader unlitShader = Shader.Find("Universal Render Pipeline/Unlit");

        if (unlitShader == null)
        {
            Debug.LogError("The URP Unlit shader could not be found for the arena grid.", this);
            return;
        }

        gridMesh = CreateGridMesh();
        gridMaterial = CreateGridMaterial(unlitShader);

        meshFilter.sharedMesh = gridMesh;
        meshRenderer.sharedMaterial = gridMaterial;
    }

    private Mesh CreateGridMesh()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        float halfWidth = width * 0.5f;
        float halfDepth = depth * 0.5f;
        float cornerCut = Mathf.Min(halfWidth, halfDepth) * (Mathf.Sqrt(2f) - 1f);
        AddVerticalGridLines(vertices, triangles, halfWidth, halfDepth, cornerCut);
        AddHorizontalGridLines(vertices, triangles, halfWidth, halfDepth, cornerCut);

        Mesh mesh = new Mesh
        {
            name = "Arena Grid"
        };
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }

    private void AddVerticalGridLines(
        List<Vector3> vertices,
        List<int> triangles,
        float halfWidth,
        float halfDepth,
        float cornerCut)
    {
        int lineCount = Mathf.FloorToInt((halfWidth - lineWidth * 0.5f) / cellSize);
        for (int lineIndex = -lineCount; lineIndex <= lineCount; lineIndex++)
        {
            float x = lineIndex * cellSize;
            float halfLineLength = GetGridLineHalfLength(
                x,
                halfWidth,
                halfDepth,
                cornerCut);
            if (halfLineLength <= 0f)
            {
                continue;
            }

            AddQuad(
                vertices,
                triangles,
                x - lineWidth * 0.5f,
                -halfLineLength,
                x + lineWidth * 0.5f,
                halfLineLength);
        }
    }

    private void AddHorizontalGridLines(
        List<Vector3> vertices,
        List<int> triangles,
        float halfWidth,
        float halfDepth,
        float cornerCut)
    {
        int lineCount = Mathf.FloorToInt((halfDepth - lineWidth * 0.5f) / cellSize);
        for (int lineIndex = -lineCount; lineIndex <= lineCount; lineIndex++)
        {
            float z = lineIndex * cellSize;
            float halfLineLength = GetGridLineHalfLength(
                z,
                halfDepth,
                halfWidth,
                cornerCut);
            if (halfLineLength <= 0f)
            {
                continue;
            }

            AddQuad(
                vertices,
                triangles,
                -halfLineLength,
                z - lineWidth * 0.5f,
                halfLineLength,
                z + lineWidth * 0.5f);
        }
    }

    private float GetGridLineHalfLength(
        float linePosition,
        float halfLineAxis,
        float halfSegmentAxis,
        float cornerCut)
    {
        float halfLineWidth = lineWidth * 0.5f;
        float axisLimit = halfSegmentAxis - halfLineWidth;
        float octagonLimit = halfLineAxis + cornerCut - Mathf.Abs(linePosition) - lineWidth;
        return Mathf.Min(axisLimit, octagonLimit);
    }

    private Material CreateGridMaterial(Shader unlitShader)
    {
        Material material = new Material(unlitShader)
        {
            name = "Arena Grid (Runtime)"
        };
        material.SetColor("_BaseColor", lineColor);
        return material;
    }

    private static void AddQuad(
        List<Vector3> vertices,
        List<int> triangles,
        float minX,
        float minZ,
        float maxX,
        float maxZ)
    {
        int firstVertex = vertices.Count;
        const float localHeight = 0f;

        vertices.Add(new Vector3(minX, localHeight, minZ));
        vertices.Add(new Vector3(minX, localHeight, maxZ));
        vertices.Add(new Vector3(maxX, localHeight, maxZ));
        vertices.Add(new Vector3(maxX, localHeight, minZ));

        triangles.Add(firstVertex);
        triangles.Add(firstVertex + 1);
        triangles.Add(firstVertex + 2);
        triangles.Add(firstVertex);
        triangles.Add(firstVertex + 2);
        triangles.Add(firstVertex + 3);
    }

    private static void DestroyGeneratedObject(Object generatedObject)
    {
        if (generatedObject == null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            Destroy(generatedObject);
        }
        else
        {
            DestroyImmediate(generatedObject);
        }
    }
}
