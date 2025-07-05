using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    [Header("Wall")]
    public Tilemap wallTilemap;
    public TileBase wallTile;

    [Header("Ground")]
    public Tilemap backgroundTilemap;
    public TileBase backgroundTile;

    [Header("Destructible")]
    public Tilemap destructibleTilemap;
    public TileBase destructibleTile;
    public int numDestructibleTiles = 50;

    [Header("Indestructible")]
    public Tilemap indestructibleTilemap;
    public TileBase indestructibleTile;
    public int numIndestructibleTiles = 20;

    public Vector2 mapBoundaryMin = new Vector2(-10, -5);
    public Vector2 mapBoundaryMax = new Vector2(10, 5);

    HashSet<Vector3Int> blankMap = new HashSet<Vector3Int>();

    void Start()
    {
        DefineBlankMap();
        GenerateMap();
    }

    void DefineBlankMap()
    {
        int minX = Mathf.FloorToInt(mapBoundaryMin.x);
        int minY = Mathf.FloorToInt(mapBoundaryMin.y);
        int maxX = Mathf.CeilToInt(mapBoundaryMax.x) - 1;
        int maxY = Mathf.CeilToInt(mapBoundaryMax.y) - 1;

        for (int x = minX; x < minX + 2; x++)
        {
            for (int y = maxY; y > maxY - 2; y--)
            {
                blankMap.Add(new Vector3Int(x, y, 0));
            }
        }

        for (int x = maxX; x > maxX - 2; x--)
        {
            for (int y = maxY; y > maxY - 2; y--)
            {
                blankMap.Add(new Vector3Int(x, y, 0));
            }
        }

        for (int x = minX; x < minX + 2; x++)
        {
            for (int y = minY; y < minY + 2; y++)
            {
                blankMap.Add(new Vector3Int(x, y, 0));
            }
        }

        for (int x = maxX; x > maxX - 2; x--)
        {
            for (int y = minY; y < minY + 2; y++)
            {
                blankMap.Add(new Vector3Int(x, y, 0));
            }
        }
    }

    void GenerateMap()
    {
        if (destructibleTilemap != null) destructibleTilemap.ClearAllTiles();
        if (indestructibleTilemap != null) indestructibleTilemap.ClearAllTiles();
        if (wallTilemap != null) wallTilemap.ClearAllTiles();
        if (backgroundTilemap != null) backgroundTilemap.ClearAllTiles();

        HashSet<Vector3Int> occupiedPositions = new HashSet<Vector3Int>();

        foreach (Vector3Int pos in blankMap)
        {
            occupiedPositions.Add(pos);
        }

        DrawBackground();

        DrawOuterWalls();

        PlaceTiles(indestructibleTilemap, indestructibleTile, numIndestructibleTiles, occupiedPositions, 1);

        PlaceTiles(destructibleTilemap, destructibleTile, numDestructibleTiles, occupiedPositions, 0);
    }

    void PlaceTiles(Tilemap targetTilemap, TileBase tileToPlace, int numberOfTiles, HashSet<Vector3Int> occupiedPositions, int offSet)
    {
        if (targetTilemap == null || tileToPlace == null)
        {
            Debug.LogWarning("Tilemap or Tile is not assigned for placing tiles. Skipping placement for " + targetTilemap?.name);
            return;
        }

        int placedCount = 0;
        int maxAttempts = numberOfTiles * 10;

        Vector2 playableMin = new Vector2(mapBoundaryMin.x, mapBoundaryMin.y);
        Vector2 playableMax = new Vector2(mapBoundaryMax.x, mapBoundaryMax.y);

        for (int i = 0; i < maxAttempts && placedCount < numberOfTiles; i++)
        {
            float randomX = Random.Range(playableMin.x + offSet, playableMax.x - offSet);
            float randomY = Random.Range(playableMin.y + offSet, playableMax.y - offSet);

            Vector3 worldPos = new Vector3(randomX, randomY, 0);
            Vector3Int cellPos = targetTilemap.WorldToCell(worldPos);

            if (cellPos.x >= Mathf.RoundToInt(playableMin.x) && cellPos.x <= Mathf.RoundToInt(playableMax.x - 0.5f) &&
                cellPos.y >= Mathf.RoundToInt(playableMin.y) && cellPos.y <= Mathf.RoundToInt(playableMax.y - 0.5f) &&
                !occupiedPositions.Contains(cellPos) &&
                (wallTilemap == null || !wallTilemap.HasTile(cellPos)))
            {
                targetTilemap.SetTile(cellPos, tileToPlace);
                occupiedPositions.Add(cellPos);
                placedCount++;
            }
        }

        if (placedCount < numberOfTiles)
        {
            Debug.LogWarning($"Could not place all {numberOfTiles} tiles of type {tileToPlace.name}. Placed {placedCount}. Consider increasing boundary or decreasing tile count.");
        }
    }

    void DrawOuterWalls()
    {
        if (wallTilemap == null || wallTile == null)
        {
            Debug.LogWarning("Wall Tilemap or Wall Tile is not assigned. Cannot draw outer walls.");
            return;
        }

        int minX = Mathf.FloorToInt(mapBoundaryMin.x);
        int minY = Mathf.FloorToInt(mapBoundaryMin.y);
        int maxX = Mathf.CeilToInt(mapBoundaryMax.x);
        int maxY = Mathf.CeilToInt(mapBoundaryMax.y);

        for (int x = minX - 1; x <= maxX; x++)
        {
            wallTilemap.SetTile(new Vector3Int(x, minY - 1, 0), wallTile);
            wallTilemap.SetTile(new Vector3Int(x, maxY, 0), wallTile);
        }

        for (int y = minY - 1; y <= maxY; y++)
        {
            wallTilemap.SetTile(new Vector3Int(minX - 1, y, 0), wallTile);
            wallTilemap.SetTile(new Vector3Int(maxX, y, 0), wallTile);
        }
    }

    void DrawBackground()
    {
        if (backgroundTilemap == null || backgroundTile == null)
        {
            Debug.LogWarning("Background Tilemap or Background Tile is not assigned. Cannot draw background.");
            return;
        }

        int minX = Mathf.FloorToInt(mapBoundaryMin.x) - 1;
        int minY = Mathf.FloorToInt(mapBoundaryMin.y) - 1;
        int maxX = Mathf.CeilToInt(mapBoundaryMax.x);
        int maxY = Mathf.CeilToInt(mapBoundaryMax.y);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                backgroundTilemap.SetTile(new Vector3Int(x, y, 0), backgroundTile);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 center = new Vector3(
            (mapBoundaryMin.x + mapBoundaryMax.x) / 2f,
            (mapBoundaryMin.y + mapBoundaryMax.y) / 2f,
            0f
        );

        Vector3 size = new Vector3(
            Mathf.Abs(mapBoundaryMax.x - mapBoundaryMin.x) + 2,
            Mathf.Abs(mapBoundaryMax.y - mapBoundaryMin.y) + 2,
            0.1f
        );

        Gizmos.DrawWireCube(center, size);
    }
}