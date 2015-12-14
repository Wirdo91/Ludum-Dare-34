using UnityEngine;
using System.Collections;

public class TerrainHandler : MonoBehaviour
{
    [SerializeField]
    Terrain terrain;
    int resolution = 512;

    int pickupsize;

    [SerializeField]
    SnowHandler _snowhandler;

    // Use this for initialization
    void Start ()
    {
        terrain = this.GetComponent<Terrain>();
        resolution = terrain.terrainData.alphamapResolution;
        pickupsize = (int)(terrain.terrainData.alphamapResolution / terrain.terrainData.size.x);
        StartCoroutine(LetitSnow());
    }

    //TODO: Add RenderSettings Fog and snow drop rate.
    public void NoSnow()
    {
        GlobalVariables.instance.SnowRate = 0;
        _snowhandler.NoSnow();
        _snowhandler.NoSnowFog();
        _snowhandler.NoGroundFog();
    }

    public void MildSnow()
    {
        _snowhandler.MildSnow();
        _snowhandler.NoSnowFog();
        _snowhandler.NoGroundFog();
    }

    public void MediumSnow()
    {
        _snowhandler.MediumSnow();
        _snowhandler.NoSnowFog();
        _snowhandler.NoGroundFog();
    }

    public void HeavySnow()
    {
        _snowhandler.HeavySnow();
        _snowhandler.NoSnowFog();
        _snowhandler.NoGroundFog();
    }

    public void MildStorm()
    {
        _snowhandler.NoSnow();
        _snowhandler.MildSnowFog();
        _snowhandler.NoGroundFog();
    }

    public void MediumStorm()
    {
        _snowhandler.NoSnow();
        _snowhandler.MediumSnowFog();
        _snowhandler.GroundFog();
    }

    public void HeavyStorm()
    {
        _snowhandler.NoSnow();
        _snowhandler.HeavySnowFog();
        _snowhandler.HeavyGroundFog();
    }

    public void MildSnowStorm()
    {
        _snowhandler.MildSnow();
        _snowhandler.MildSnowFog();
        _snowhandler.NoGroundFog();
    }

    public void MediumSnowStorm()
    {
        _snowhandler.MediumSnow();
        _snowhandler.MediumSnowFog();
        _snowhandler.GroundFog();
    }

    public void HeavySnowStorm()
    {
        _snowhandler.HeavySnow();
        _snowhandler.HeavySnowFog();
        _snowhandler.HeavyGroundFog();
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