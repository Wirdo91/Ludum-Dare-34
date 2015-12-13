using UnityEngine;
using System.Collections;

public class TerrainHandler : MonoBehaviour
{
    Terrain terrain;
    int resolution = 512;

    int pickupsize;


    // Use this for initialization
    void Start ()
    {
        terrain = this.GetComponent<Terrain>();
        resolution = terrain.terrainData.alphamapResolution;
        pickupsize = (int)(terrain.terrainData.alphamapResolution / terrain.terrainData.size.x);
        StartCoroutine(LetitSnow());
    }

    public void ResetTerrain()
    {
        float[,,] alphas = terrain.terrainData.GetAlphamaps(0, 0, resolution, resolution);
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                alphas[i, j, 0] = 1;
                alphas[i, j, 1] = 0;
            }
        }
        terrain.terrainData.SetAlphamaps(0, 0, alphas);
    }

    public float GetSnow(Transform player)
    {
        if (player.position.x < 0 || player.position.x > terrain.terrainData.size.x || player.position.z < 0 || player.position.z > terrain.terrainData.size.z)
        {
            return 0.0f;
        }
        float snowpickup = 0;


        int size = (Mathf.CeilToInt(player.GetComponentInChildren<SnowBallMovement>().CurrentThickness / pickupsize) * 4 % 2 == 0) ?
            (Mathf.CeilToInt(player.GetComponentInChildren<SnowBallMovement>().CurrentThickness / pickupsize) * 4) + 1 :
            (Mathf.CeilToInt(player.GetComponentInChildren<SnowBallMovement>().CurrentThickness / pickupsize) * 4);

        float[,,] alphas = terrain.terrainData.GetAlphamaps(Mathf.RoundToInt((player.position.x * pickupsize) + -(size / 2)), Mathf.RoundToInt((player.position.z * pickupsize) + -(size / 2)), size, size);

        float mid = Mathf.Ceil((float)size / 2.0f);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float percentage = Mathf.Abs(1.0f - Vector2.Distance(new Vector2(i, j), new Vector2(mid, mid)) / mid) / 8.0f;
                snowpickup += alphas[i, j, 0] * percentage;

                alphas[i, j, 0] -= alphas[i, j, 0] * percentage;
                alphas[i, j, 1] = 1 - alphas[i, j, 0];

                if (alphas[i, j, 0] < 0.0f)
                {
                    alphas[i, j, 0] = 0;
                }
                if (alphas[i, j, 1] > 1.0f)
                {
                    alphas[i, j, 1] = 1;
                }
            }
        }
        terrain.terrainData.SetAlphamaps(Mathf.RoundToInt((player.position.x * pickupsize) + -(size / 2)), Mathf.RoundToInt((player.position.z * pickupsize) + -(size / 2)), alphas);

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
