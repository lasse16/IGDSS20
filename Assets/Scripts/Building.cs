using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tile))]
public class Building : MonoBehaviour
{
    [SerializeField] private GeneralBuildingStats GeneralBuildingStats;


    [HideInInspector]
    public Tile tile;

    private float generationState;
    private float effectiveGenerationTime;
    private bool productionRunning;


    // Start is called before the first frame update
    void Start()
    {
        //effectiveGenerationTime = (float) (2 - Efficiency()) * resourceGenerationInterval;
        effectiveGenerationTime = (2 - 0.5f) * GeneralBuildingStats.ResourceGenerationInterval;
    }

    // Update is called once per frame
    void Update()
    {

        if (productionRunning)
        {
            // working
            generationState += Time.deltaTime;

            // finished cycle
            if (generationState >= effectiveGenerationTime)
            {
                // create resource and reset cycle
                GameManager.winResource(GeneralBuildingStats.OutputResource, GeneralBuildingStats.OutputCount);
                generationState -= effectiveGenerationTime;
                productionRunning = false;

                // restart if possible
                tryToStartProduction();
            }
        }
        else
        {
            // try to start production            
            tryToStartProduction();
        }
    }

    /* 
     * checks if production can be started. 
     * If start is possible, the input res are taken from warehouse. 
     */
    private void tryToStartProduction()
    {
        if (CanProductionStart())
        {
            // start production
            productionRunning = true;
            GeneralBuildingStats.InputResources.ForEach(delegate (ResourceType it)
            {
                GameManager.removeResource(it, 1);
            });
        }
        else
        {
            // Production start failed aka GameManager said no. 
            productionRunning = false;
        }
    }

    /* 
     * calculates efficiency of the building respective to neighboring tiles 
     * and specific min/ max needed number of a specific tile type. 
     */
    float Efficiency()
    {
        // save neighbors. 
        List<GameObject> neighbors = new List<GameObject>();
        


        // neighbors = tile.getNeighboringTiles();
        // TODO: or however you saved the neighbors. + adapt next if
        // 




        // count 'efficient' neighbors
        int counter = 0;
        float efficiency;
        foreach (var item in tile.NeighbouringTiles)
        {

        }

        // not efficient/ min max not right
        if (counter == 0 || (GeneralBuildingStats.MaxEfficientNeighbor < GeneralBuildingStats.MinEfficientNeigbor))
            return 0;

        // clamp calculated efficiency into range [0,1]
        efficiency = (float) counter / (1 + GeneralBuildingStats.MaxEfficientNeighbor - GeneralBuildingStats.MinEfficientNeigbor);
        return Mathf.Clamp(efficiency, 0, 1);
    }

    /* asks GameManager if input resources are available --> production is startable
     */
    private bool CanProductionStart()
    {
        bool val = true;
        GeneralBuildingStats.InputResources.ForEach(delegate (ResourceType it)
        {
            if (!GameManager.checkAvailability(it, 1))
            {
                val = false;
            }
        });
        return val;
    }



}
