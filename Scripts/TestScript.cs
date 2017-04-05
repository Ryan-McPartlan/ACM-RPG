using UnityEngine;
using System.Collections;


//This script is used to test various effects 
public class TestScript : MonoBehaviour {

    public GameObject testEffect1;
    public GameObject testEffect2;
    public GameObject testEffect3;
    bool initialized;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (!initialized)
        {
            Grid.grid.tiles[14, 10].AddEffect(testEffect1);
            Grid.grid.tiles[14, 11].AddEffect(testEffect2);
            Grid.grid.tiles[14, 12].AddEffect(testEffect3);
            initialized = true;
        }
    }
}
