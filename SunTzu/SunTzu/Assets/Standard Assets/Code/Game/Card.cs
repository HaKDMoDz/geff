using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour
{
    public CardType CardType = CardType.PawnValue;
    public int CardValue;
    public int Index;

    //public float LastTimeAnimation = 0f;
    //public Vector3 Location;
    //public Vector3 StartLocation;
    //public float DurationAnimation = 1f;
    public LerpVector3 LerpAnimation;

    public bool isSelected = false;
    Collider colBoard;
    Collider colCardPlan;
    List<BoxCollider> listColContinentCard;

    private Vector3 vecInitialSelection = Vector3.zero;
    public ContinentCard ContinentCard = null;
    public ContinentCard NextContinentCard = null;


    void Start()
    {
        //Location = this.transform.position;
        colBoard = GameObject.Find("Board").GetComponent<BoxCollider>();
        colCardPlan = GameObject.Find("CardPlan").GetComponent<BoxCollider>();

        listColContinentCard = new List<BoxCollider>();

        Object[] objContinentCards = GameObject.FindObjectsOfType(typeof(ContinentCard));

        foreach (Object continentCard in objContinentCards)
        {
            listColContinentCard.Add((BoxCollider)((ContinentCard)continentCard).gameObject.collider);
        }
    }

    void Update()
    {
        //if ((!isSelected && NextContinentCard == null) && Game.GameState == GameState.PickCardInHand && Mathf.Abs(this.StartLocation.sqrMagnitude - this.Location.sqrMagnitude) > 0.05f)
        if (LerpAnimation != null && !LerpAnimation.IsFinished())
        {
            this.transform.position = LerpAnimation.Eval();
        }
        else if (isSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (colCardPlan.Raycast(ray, out hit, 100f))
            {
                if (vecInitialSelection == Vector3.zero)
                    vecInitialSelection = hit.point;

                Vector3 cardPosition = LerpAnimation.EndValue + hit.point - vecInitialSelection;

                Quaternion quat = Quaternion.AngleAxis(90, Vector3.up);
                Vector3 vect = quat * this.transform.up;

                ray = new Ray(cardPosition, vect);

                Debug.DrawRay(ray.origin, ray.direction);

                NextContinentCard = null;

                foreach (BoxCollider colContinentCard in listColContinentCard)
                {
                    //ContinentCard continentCard = colContinentCard.gameObject.GetComponent<ContinentCard>();

                    if (colContinentCard.Raycast(ray, out hit, 100f))
                    {
                        NextContinentCard = colContinentCard.gameObject.GetComponent<ContinentCard>();
                        
                        colContinentCard.gameObject.renderer.material.color = Color.yellow;
                    }
                    else
                    {
                        colContinentCard.gameObject.renderer.material.color = Color.white;
                    }
                }

                //if (cardPosition.y >= 0.36f && cardPosition.y <= 1.05f && cardPosition.x >= -2f && cardPosition.x <= 1.9f)

                //if (NextContinentCard == null)
                    this.transform.position = cardPosition;


            }
        }
    }

    public void OnMouseEnter()
    {
        //if (Game.GameState == GameState.PickCardInHand && !isPicked)
        //{
        //    //this.transform.position = new Vector3(this.Location.x, this.Location.y + 0.05f, this.Location.z);
        //}
    }

    public void OnMouseExit()
    {
        //if (Game.GameState == GameState.PickCardInHand && !isPicked)
        //{
        //    //this.transform.position = this.Location;
        //}
    }

    public void OnMouseDown()
    {
        if (Game.GameState == GameState.PickCardInHand)
        {
            isSelected = true;
            vecInitialSelection = Vector3.zero;
            //Game.GameState = GameState.CardPickedInHand;

            if (ContinentCard == null)
            {
                Game.Player.ListCardInHand.Remove(this);
                Game.Player.SortPileInHand(false);
            }
        }
    }

    public void OnMouseUp()
    {
        if (Game.GameState == GameState.PickCardInHand)
        {
            isSelected = false;
            vecInitialSelection = Vector3.zero;

            if (NextContinentCard != null)
            {
                //---> Déplacement d'une carte par dessus une autre carte
                if (NextContinentCard.Card != null)
                {
                    //---> Déplacement d'une carte issue de la main sur une carte déja posée
                    if (ContinentCard == null)
                    {
                        Game.Player.ListCardInHand.Add(NextContinentCard.Card);
                        Game.Player.SortPileInHand(false);

                        NextContinentCard.Card.ContinentCard = null;
                    }
                    //---> Déplacement d'une carte déja posée sur une carte déja posée
                    else
                    {
                        NextContinentCard.Card.LerpAnimation = new LerpVector3(NextContinentCard.Card.transform.position, Tools.TransYVector(ContinentCard.transform.position), false, false, 0.2f);

                        NextContinentCard.Card.ContinentCard = ContinentCard;
                        ContinentCard.Card = NextContinentCard.Card;
                    }
                }
                else if (ContinentCard != null)
                {
                    ContinentCard.Card = null;
                }

                LerpAnimation = new LerpVector3(this.transform.position, new Vector3(NextContinentCard.transform.position.x,NextContinentCard.transform.position.y+0.01f,NextContinentCard.transform.position.z), false, false, 0.2f);

                NextContinentCard.Card = this;
            }
            else
            {
                if (ContinentCard != null)
                    ContinentCard.Card = null;

                Game.Player.ListCardInHand.Add(this);

                this.LerpAnimation.StartValue = this.transform.position;

                Game.Player.SortPileInHand(false);
            }

            ContinentCard = NextContinentCard;
            NextContinentCard = null;
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

