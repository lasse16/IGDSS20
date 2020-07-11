using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IGDSS20.Buildings;
using IGDSS20.Enums;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{ 
    [SerializeField] private GameManager gameManager;
    [SerializeField] private JobManager jobmanager;
    // resources
    [Tooltip("The text field corresponding to specific resource")]
    public GameObject wood, plank, wool, cloth, potato, sake, fish;
    private Text woodText, plankText, woolText, clothText, potatoText, sakeText, fishText;
    private IStorage storage;

    // money & workers
    
    [Tooltip("The corresponding text field")]
    public GameObject worker, money;
    private Text moneyText, totalWorkerText;


    // Start is called before the first frame update
    void Start()
    {
        // initalize text fields for resources
        storage = gameManager.GetComponent<IStorage>();
        woodText = wood.GetComponent<Text>();
        plankText = plank.GetComponent<Text>();
        woolText = wool.GetComponent<Text>();
        clothText = cloth.GetComponent<Text>();
        potatoText = potato.GetComponent<Text>();             
        sakeText = sake.GetComponent<Text>(); 
        fishText = fish.GetComponent<Text>();

        // initialise text fields for money & workers
        moneyText = money.GetComponent<Text>();
        totalWorkerText = worker.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // Show available resources
        woodText.text = $"{storage.GetCountOfResource(ResourceType.Wood)} wood";
        plankText.text = $"{storage.GetCountOfResource(ResourceType.Plank)} plank";
        woolText.text = $"{storage.GetCountOfResource(ResourceType.Wool)} wool";
        clothText.text = $"{storage.GetCountOfResource(ResourceType.Cloth)} cloth";
        potatoText.text = $"{storage.GetCountOfResource(ResourceType.Potato)} potato";
        sakeText.text = $"{storage.GetCountOfResource(ResourceType.Schnapps)} sake";
        fishText.text = $"{storage.GetCountOfResource(ResourceType.Fish)} fish";

        // show available money 
        moneyText.text = $"{gameManager.GetMoneyAmount()}";
        totalWorkerText.text = $"{jobmanager.GetAmountOfEmployedWorkers() + jobmanager.GetAmountOfUnemployedWorkers() + jobmanager.GetAmountOfRetirees()}";


    }
}
