using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotManager : MonoBehaviour {
    private GameObject Dot;
	// Use this for initialization
	void Start ()
	{
	    Dot=GameObject.Find("Tip");
    }
	
	// Update is called once per frame
	void Update ()
	{
		
	}

    public void TipOnOff()
    {
        Dot.GetComponent<Renderer>().enabled=!Dot.GetComponent<Renderer>().enabled;
    }

    public void TipScaleUp()
    {
        Dot.transform.localScale = Dot.transform.localScale * 1.5f;
    }
    public void TipScaleDown()
    {
        Dot.transform.localScale = Dot.transform.localScale * (1.0f/1.5f);
    }
}
