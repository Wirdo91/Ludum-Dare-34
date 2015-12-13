using UnityEngine;
using System.Collections;

public class TerrainHandler : MonoBehaviour
{
    [SerializeField]
    Transform player;
    Terrain terrain;
    int resolution = 256;

    int pickupsize = 2;


    // Use this for initialization
    void Start ()
    {
        terrain = this.GetComponent<Terrain>();
        StartCoroutine(LetitSnow());
    }

    // Update is called once per frame
    void Update()
    {
        float[,,] alphas = terrain.terrainData.GetAlphamaps(0, 0, resolution, resolution);

        int size = (Mathf.CeilToInt(player.GetComponentInChildren<SnowBallMovement>().CurrentThickness / pickupsize) % 2 == 0) ? (Mathf.CeilToInt(player.GetComponentInChildren<SnowBallMovement>().CurrentThickness / pickupsize) + 1) : (Mathf.CeilToInt(player.GetComponentInChildren<SnowBallMovement>().CurrentThickness / pickupsize));
        alphas = new float[size, size, 2];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                alphas[i, j, 0] = 0;
                alphas[i, j, 1] = 1;
            }
        }
        terrain.terrainData.SetAlphamaps(Mathf.RoundToInt((player.position.x * 2) + -size), Mathf.RoundToInt((player.position.z * 2) + -size), alphas);
    }

    public float GetSnow()
    {
        float snowpickup = 0;

        float[,,] alphas = terrain.terrainData.GetAlphamaps(0, 0, resolution, resolution);

        int size = (Mathf.CeilToInt(player.GetComponentInChildren<SnowBallMovement>().CurrentThickness / 4) % 2 == 0) ? (Mathf.CeilToInt(player.GetComponentInChildren<SnowBallMovement>().CurrentThickness / 4) + 1) : (Mathf.CeilToInt(player.GetComponentInChildren<SnowBallMovement>().CurrentThickness / 4));
        alphas = new float[size, size, 2];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                snowpickup += alphas[i, j, 0];
                alphas[i, j, 0] = 0;
                alphas[i, j, 1] = 1;
            }
        }
        terrain.terrainData.SetAlphamaps(Mathf.RoundToInt(player.position.x + -size) * 2, Mathf.RoundToInt(player.position.z - size) * 2, alphas);

        return snowpickup;
    }

    IEnumerator LetitSnow()
    {
        float[,,] alphas;

        int updatesize = 64;
        int updatemul = 0;
        int updatemulmod = resolution / updatesize;

        int updatemulmax = updatemulmod * updatemulmod;

        while (true)
        {
            alphas = terrain.terrainData.GetAlphamaps(Mathf.FloorToInt(updatemul / updatemulmod) * updatesize, (updatemul % updatemulmod) * updatesize, updatesize, updatesize);
            for (int i = 0; i < updatesize; i++)
            {
                for (int j = 0; j < updatesize; j++)
                {
                    alphas[i, j, 0] += GlobalVariables.instance.SnowRate * Time.deltaTime;
                    alphas[i, j, 1] -= GlobalVariables.instance.SnowRate * Time.deltaTime;
                    if (alphas[i, j, 0] > 1)
                    {
                        alphas[i, j, 0] = 1;
                    }
                    if (alphas[i, j, 1] < 0)
                    {
                        alphas[i, j, 1] = 0;
                    }
                }
            }

            terrain.terrainData.SetAlphamaps(Mathf.FloorToInt(updatemul / updatemulmod) * updatesize, (updatemul % updatemulmod) * updatesize, alphas);
            updatemul++;
            if (updatemul == updatemulmax)
            {
                updatemul = 0;
            }
            yield return null;
        }

    }
}
