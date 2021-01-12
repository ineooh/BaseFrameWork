using UnityEngine;
using System.Collections;

public class SetRendererLayer : MonoBehaviour {
    public string LayerName;
    public SortingLayer layer;
    public int order;

    // Use this for initialization
    void Start()
    {
        if (LayerName != "")
        {
            Set(LayerName, order);
        }
        
	}
	public void Set( string name , int order)
    {
        Renderer[] rdererList = GetComponentsInChildren<Renderer>();
        foreach (Renderer rderer in rdererList)
            if (rderer != null)
            {
                rderer.sortingLayerName = LayerName;
                rderer.sortingOrder = order;
            }
    }
}
