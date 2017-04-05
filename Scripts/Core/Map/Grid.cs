using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

    public static Grid grid;

    //The dimmensions of the current tilemap
    Vector2 dimmensions = new Vector2(40,50);
    public Tile[,] tiles;

    [SerializeField]
    GameObject tile;

    //Singleton
    void Awake()
    {
        if(grid == null)
        {
            grid = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

	// Use this for initialization
	void Start () {
        //Set the dimmensions of the tile array
        tiles = new Tile[(int)dimmensions.x, (int)dimmensions.y];

        //Fill the map with tiles TODO: PH, remove when we start making maps
        for(int i = 0; i < dimmensions.x; i++)
        {
            for (int j = 0; j < dimmensions.y; j++)
            {
                GameObject newTile = Instantiate(tile, new Vector3(transform.position.x + i, transform.position.y + j), Quaternion.identity) as GameObject;

                tiles[i, j] = newTile.GetComponent<Tile>();
                newTile.transform.parent = transform;

                //If the tile is on the edge, it blocks movement
                if (j == 0 || j == dimmensions.y - 1 || i == 0 || i == dimmensions.x - 1)
                {
                    newTile.GetComponent<Tile>().Setup(new Vector2(i, j), true);
                }
                else
                {
                    newTile.GetComponent<Tile>().Setup(new Vector2(i, j), false);
                }
            }
        }

        //Set the players starting tile TODO: TEST: Remove thius
        PlayerController.playerController.Spawn(tiles[20,11]);
	}
	
    public List<Tile> GetTile(Tile start, string type)
    {
        List<Tile> returnTiles = new List<Tile>();

        switch (type)
        {
            case "up": returnTiles.Add(IndexToTile(new Vector2 (start.matrixPosition.x, start.matrixPosition.y + 1)));
                break;
            default:
                break;
        }
        return returnTiles;
    }

    Tile IndexToTile(Vector2 coordinate)
    {
        return tiles[(int)coordinate.x, (int)coordinate.y];
    }
}
