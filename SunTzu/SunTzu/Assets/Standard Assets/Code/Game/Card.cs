using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour
{
    public CardType CardType = CardType.PawnValue;
    public int CardValue;
    public Vector3 initialLocation;
    bool isSelected = false;
    Collider colBoard;
    Collider colCardPlan;

    private Vector3 vecInitialSelection = Vector3.zero;

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
        colCardPlan = GameObject.Find("CardPlan").GetComponent<BoxCollider>();
    }

    void Update()
    {

        if (isSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (colCardPlan.Raycast(ray, out hit, 100f))
            {
                if (vecInitialSelection == Vector3.zero)
                    vecInitialSelection = hit.point;

                this.transform.position = this.initialLocation + hit.point - vecInitialSelection;
            }
        }
    }

    public void OnMouseEnter()
    {
        if (Game.GameState == GameState.PickCardInHand)
        {
            this.transform.position = new Vector3(this.initialLocation.x, this.initialLocation.y + 0.05f, this.initialLocation.z);
        }
    }

    public void OnMouseExit()
    {
        if (Game.GameState == GameState.PickCardInHand)
        {
            this.transform.position = this.initialLocation;
        }
    }

    public void OnMouseDown()
    {
        if (Game.GameState == GameState.PickCardInHand)
        {
            isSelected = true;
            vecInitialSelection = Vector3.zero;
            Game.GameState = GameState.CardPickedInHand;
        }
    }

    public void OnMouseUp()
    {
        if (Game.GameState == GameState.CardPickedInHand)
        {
            isSelected = false;
            vecInitialSelection = Vector3.zero;
            Game.GameState = GameState.PickCardInHand;
        }
    }
}

public enum CardType
{
    PawnValue,
    PawnMinusValue,
    PawnBonusValue,
    Sick
}

