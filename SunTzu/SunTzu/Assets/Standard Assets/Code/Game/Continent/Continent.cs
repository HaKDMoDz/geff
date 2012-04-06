using UnityEngine;
using System.Collections;

public class Continent : MonoBehaviour
{
    public ContinentCard continentCard;
    public Vector3 initialLocation;

    void Start()
    {
        continentCard = (ContinentCard)this.GetComponentInChildren(typeof(ContinentCard));
        initialLocation = this.transform.position;
    }

    void Update()
    {

    }

    public void OnMouseEnter()
    {
        transform.position = new Vector3(initialLocation.x, initialLocation.y, 15);
    }
}
