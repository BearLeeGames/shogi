using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Highlight : MonoBehaviour {
    private Color startColor;

    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }
	// Use this for initialization
	void OnMouseEnter()
    {
        startColor = rend.material.color;
        rend.material.color = Color.yellow;
	}
	
	// Update is called once per frame
	void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}
