using System.Collections.Generic;
using UnityEngine;

public class TileEdges : MonoBehaviour
{

    [SerializeField] private GameObject UpperLeftEdge;
    [SerializeField] private GameObject UpperCenterEdge;
    [SerializeField] private GameObject UpperRightEdge;
    [SerializeField] private GameObject LowerRightEdge;
    [SerializeField] private GameObject LowerCenterEdge;
    [SerializeField] private GameObject LowerLeftEdge;
    
    private Color _edgeColor;
    private List<Material> _materials;

    private void Awake()
    {
        var edgeMaterials = new List<Material>();
        foreach (Transform child in transform)
        {
            edgeMaterials.Add(child.gameObject.GetComponent<Renderer>().material);
        }

        _materials = edgeMaterials;
    }

    public void SetEdgeColor(Color color)
    {
        _edgeColor = color;
        foreach (var material in _materials)
        {
            material.color = color; 
        }
    }

    public Color GetedgeColor() => _edgeColor;

    public void DisableEdge(HexEdge edge)
    {
        switch (edge)
        {
            case HexEdge.UpperLeft:
                UpperLeftEdge.gameObject.SetActive(false);
                break;
            case HexEdge.UpperCenter:
                UpperCenterEdge.gameObject.SetActive(false);
                break;
            case HexEdge.UpperRight:
                UpperRightEdge.gameObject.SetActive(false);
                break;
            case HexEdge.LowerRight:
                LowerRightEdge.gameObject.SetActive(false);
                break;
            case HexEdge.LowerCenter:
                LowerCenterEdge.gameObject.SetActive(false);
                break;
            case HexEdge.LowerLeft:
                LowerLeftEdge.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
    

    public void EnableEdge(HexEdge edge)
    {
        switch (edge)
        {
            case HexEdge.UpperLeft:
                UpperLeftEdge.gameObject.SetActive(true);
                break;
            case HexEdge.UpperCenter:
                UpperCenterEdge.gameObject.SetActive(true);
                break;
            case HexEdge.UpperRight:
                UpperRightEdge.gameObject.SetActive(true);
                break;
            case HexEdge.LowerRight:
                LowerRightEdge.gameObject.SetActive(true);
                break;
            case HexEdge.LowerCenter:
                LowerCenterEdge.gameObject.SetActive(true);
                break;
            case HexEdge.LowerLeft:
                LowerLeftEdge.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}
