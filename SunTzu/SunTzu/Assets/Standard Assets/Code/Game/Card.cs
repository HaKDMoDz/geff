using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour
{
    public CardType CardType = CardType.PawnValue;
    public int CardValue;
    public Vector3 initialLocation;
    bool isSelected = false;
    Collider colBoard;

    /*
    public Card(CardType cardType, int cardValue)
    {
        this.CardType = CardType;
        this.CardValue = CardValue;
    }
    */

    void Start()
    {
        initialLocation = this.transform.position;
        colBoard = GameObject.Find("Board").GetComponent<BoxCollider>();
    }

    void Update()
    {
        
        if (isSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                /*
                if (hit != null && hit.collider == boardCollider)
                {
                    this.transform
                }*/
            }
        }
    }

    public void OnMouseEnter()
    {
        this.transform.position = new Vector3(this.initialLocation.x, this.initialLocation.y + 0.05f, this.initialLocation.z);
    }

    public void OnMouseExit()
    {
        this.transform.position = this.initialLocation;
    }

    public void OnMouseDown()
    {
        isSelected = true;
    }

    public void OnMouseUp()
    {
        isSelected = false;
    }
}

public enum CardType
{
    PawnValue,
    PawnMinusValue,
    PawnBonusValue,
    Sick
}

