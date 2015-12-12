using UnityEngine;
using System.Collections;

public class TerrainTest : MonoBehaviour
{
    [SerializeField]
    Transform player;
    Terrain terrain;
    int resolution = 128;
	// Use this for initialization
	void Start ()
    {
        terrain = this.GetComponent<Terrain>();
	}

    // Update is called once per frame
    void Update()
    {
        float[,,] alphas = terrain.terrainData.GetAlphamaps(0, 0, resolution, resolution);
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                if (alphas[i, j, 0] < 1)
                {
                    alphas[i, j, 0] += GlobalVariables.instance.SnowRate * Time.deltaTime;
                }
                if (alphas[i, j, 1] > 0)
                {
                    alphas[i, j, 1] -= GlobalVariables.instance.SnowRate * Time.deltaTime;
                }
            }
        }
        terrain.terrainData.SetAlphamaps(0, 0, alphas);
        Debug.Log(alphas[0, 0, 0]);
        alphas = new float[1, 1, 2];
        // make sure every grid on the terrain is modified
        for (int i = 0; i < alphas.GetLength(0); i++)
        {
            for (int j = 0; j < alphas.GetLength(1); j++)
            {
                alphas[i, j, 0] = 0;
                alphas[i, j, 1] = 1;
            }
        }
        terrain.terrainData.SetAlphamaps(Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.z), alphas);
	}
}
