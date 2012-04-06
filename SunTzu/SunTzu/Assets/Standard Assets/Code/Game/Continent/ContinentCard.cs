using UnityEngine;
using System.Collections;

public class ContinentCard : MonoBehaviour
{
    public Continent continent;

    void Start()
    {
        continent = (Continent)this.transform.parent.gameObject.GetComponent(typeof(Continent));
    }

    void Update()
    {

    }

    public void OnMouseEnter()
    {
        continent.transform.position = new Vector3(continent.initialLocation.x, 0.2f, continent.initialLocation.z);
    }

    public void OnMouseExit()
    {
        continent.transform.position = continent.initialLocation;
    }
}
