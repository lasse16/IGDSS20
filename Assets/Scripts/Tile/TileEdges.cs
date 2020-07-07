using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    public void DisableEdge(TileEdge edge)
    {
        switch (edge)
        {
            case TileEdge.UpperLeft:
                UpperLeftEdge.gameObject.SetActive(false);
                break;
            case TileEdge.UpperCenter:
                UpperCenterEdge.gameObject.SetActive(false);
                break;
            case TileEdge.UpperRight:
                UpperRightEdge.gameObject.SetActive(false);
                break;
            case TileEdge.LowerRight:
                LowerRightEdge.gameObject.SetActive(false);
                break;
            case TileEdge.LowerCenter:
                LowerCenterEdge.gameObject.SetActive(false);
                break;
            case TileEdge.LowerLeft:
                LowerLeftEdge.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
    

    public void EnableEdge(TileEdge edge)
    {
        switch (edge)
        {
            case TileEdge.UpperLeft:
                UpperLeftEdge.gameObject.SetActive(true);
                break;
            case TileEdge.UpperCenter:
                UpperCenterEdge.gameObject.SetActive(true);
                break;
            case TileEdge.UpperRight:
                UpperRightEdge.gameObject.SetActive(true);
                break;
            case TileEdge.LowerRight:
                LowerRightEdge.gameObject.SetActive(true);
                break;
            case TileEdge.LowerCenter:
                LowerCenterEdge.gameObject.SetActive(true);
                break;
            case TileEdge.LowerLeft:
                LowerLeftEdge.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }


}

public enum TileEdge
{
    UpperLeft = 0,
    UpperCenter = 1,
    UpperRight = 2,
    LowerRight = 3,
    LowerCenter = 4,
    LowerLeft = 5,
}