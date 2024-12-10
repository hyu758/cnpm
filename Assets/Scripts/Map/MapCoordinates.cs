using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCoordinates : MonoBehaviour
{
    public Tilemap groundMap;
    public Tilemap destructibleMap;
    public Tilemap indestructibleMap;

    public TileBase destructibleBox;

    [SerializeField] protected int maximumNumberOfBox = 50;
    [SerializeField] protected int numberOfAddedBox = 10;
    [SerializeField] protected int numberOfObstacle = 10;
    [SerializeField] protected int timeBetweenTileChange = 10;

    public GameObject obstacle;

    public List<Vector3Int> destructiblePositions;
    public List<Vector3Int> indestructiblePositions;

    private void Start()
    {
        destructiblePositions = GetBoxPositions(destructibleMap);
        indestructiblePositions = GetBoxPositions(indestructibleMap);

        GenerateObstacle();
        InvokeRepeating("GenerateRandomPositions", 0f, timeBetweenTileChange);
    }

    void GenerateObstacle()
    {
        BoundsInt groundBounds = groundMap.cellBounds;

        for (int i = 0; i < numberOfObstacle; i++)
        {
            Vector3Int randomGroundPosition = GetRandomGroundPosition(groundBounds);

            Vector3 worldPosition = groundMap.GetCellCenterWorld(randomGroundPosition);

            Instantiate(obstacle, worldPosition, Quaternion.identity);

            indestructiblePositions.Add(randomGroundPosition);
        }
    }

    void GenerateRandomPositions()
    {
        destructiblePositions = GetBoxPositions(destructibleMap);

        if (destructiblePositions.Count >= maximumNumberOfBox) return;

        BoundsInt groundBounds = groundMap.cellBounds;

        for (int i = 0; i < numberOfAddedBox; i++)
        {
            Vector3Int randomGroundPosition = GetRandomGroundPosition(groundBounds);

            destructibleMap.SetTile(randomGroundPosition, destructibleBox);

            destructiblePositions.Add(randomGroundPosition);

        }
    }

    List<Vector3Int> GetBoxPositions(Tilemap boxMap)
    {
        List<Vector3Int> boxPositions = new List<Vector3Int>();

        foreach (Vector3Int pos in boxMap.cellBounds.allPositionsWithin)
        {
            if (boxMap.HasTile(pos))
            {
                boxPositions.Add(pos);
            }
        }

        return boxPositions;
    }

    Vector3Int GetRandomGroundPosition(BoundsInt groundBounds)
    {
        Vector3Int randomPosition;
        int attempts = 0;
        int maxAttempts = 100; // Avoid infinite loops

        do
        {
            randomPosition = new Vector3Int(
                Random.Range(groundBounds.xMin, groundBounds.xMax),
                Random.Range(groundBounds.yMin, groundBounds.yMax),
                0
            );

            attempts++;
        } while (!IsRandomPositionValid(randomPosition) && attempts < maxAttempts);

        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("Failed to find a valid ground position after many attempts.");
        }

        return randomPosition;
    }

    protected bool IsRandomPositionValid(Vector3Int randomPosition)
    {
        if (destructiblePositions.Contains(randomPosition)) return false;
        if (indestructiblePositions.Contains(randomPosition)) return false;
        if (!groundMap.HasTile(randomPosition)) return false;

        return true;
    }
}
