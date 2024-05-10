using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleArenaGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public List<Tile> tilePrefabs;

    // A dictionary to keep track of which tiles have been generated at which positions
    private Dictionary<Vector2Int, bool> generatedTiles = new Dictionary<Vector2Int, bool>();

    // Generating a tile at a specified position
    public void GenerateTile(int x, int y)
    {
        Vector2Int tilePosition = new Vector2Int(x, y);

        // Checking if a tile has already been generated at this position
        if (!generatedTiles.ContainsKey(tilePosition))
        {
            // Randomly select a tile prefab from the list of available tiles.
            Tile tileToPlace = tilePrefabs[Random.Range(0, tilePrefabs.Count)];

            // Placing the selected tile on the tilemap
            tilemap.SetTile((Vector3Int)tilePosition, tileToPlace);

            // Marking this tile position as generated in the dictionary
            generatedTiles.Add(tilePosition, true);
        }
    }
}
