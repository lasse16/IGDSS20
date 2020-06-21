
using Assets.Scripts;
using IGDSS20.Buildings;
using IGDSS20.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Worker))]
public class RessourceHappinessRequirements : MonoBehaviour, IHappinessRequirement
{
    [SerializeField] private List<ResourceType> _requiredRessources;
    [SerializeField] private int _importance;
    
    public IStorage Storage;

    private const float _resourceRequestIntervalInSeconds = 60;
    private float _timeSinceLastResourceRequest;

    private HashSet<ResourceType> _missingResources = new HashSet<ResourceType>();
    private int _fulfillment;

    private void Awake()
    {
        //TODO MANDATORY Create better storage lookup
        var gameManager = GameObject.Find("GameManager");
        Storage = gameManager.GetComponent<IStorage>();
    }

    private void Update()
    {
        _timeSinceLastResourceRequest += Time.deltaTime;

        if (_timeSinceLastResourceRequest > _resourceRequestIntervalInSeconds)
        {
            _timeSinceLastResourceRequest -= _resourceRequestIntervalInSeconds;
            RequestAllResources();
        }

        CalculateRessourceHappiness(_missingResources.ToList());
    }


    public float CheckFulfillment() => _fulfillment;

    public int GetImportance() => _importance;

    private void RequestAllResources()
    {
        _missingResources = new HashSet<ResourceType>(_requiredRessources);
    }


    private void CalculateRessourceHappiness(List<ResourceType> resources)
    {
        _ = resources ?? throw new ArgumentException();

        foreach (var item in resources)
        {
            var happinessChange = 1 / _requiredRessources.Count;
            var available = Storage.GetResourceIfAvailable(item, 1);
            if (available)
            {
                _missingResources.Remove(item);
                _fulfillment += happinessChange;
            }
            else
            {
                if (_missingResources.Add(item))
                {
                    //Only decrease happiness if resource was not missing prior
                    _fulfillment -= happinessChange;
                }
            }
        }
    }

}
