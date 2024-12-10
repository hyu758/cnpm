using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemPositionsPrinter : MonoBehaviour
{
    public Tilemap tilemap; 

    void Start()
    {
        PrintItemPositions();
    }

    void PrintItemPositions()
    {
        BoundsInt bounds = tilemap.cellBounds;  
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds); 

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);  

                TileBase tile = tilemap.GetTile(pos);
             
                if (tile != null)
                {
                    Debug.Log("Item found at position: " + pos);
                }
            }
        }
    }
}
