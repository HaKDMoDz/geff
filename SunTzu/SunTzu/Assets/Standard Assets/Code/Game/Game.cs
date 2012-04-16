using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public Transform transCard;
    public static List<Card> listCard;

    void Start()
    {
        int nbCard = 10;
        float leftLimit = -1.4f;
        float rightLimit = 1.5f;
        listCard = new List<Card>();

        for (int i = 0; i <= nbCard; i++)
        {
            Vector3 vec = new Vector3(leftLimit + (float)i * (rightLimit - leftLimit) / (float)(nbCard), 0.5f+ (float)i * 0.001f, -8f);
            Transform transNewCard = (Transform)GameObject.Instantiate(transCard, vec, Quaternion.Euler(-130f,0f,0f));
            Card card = transNewCard.gameObject.GetComponent<Card>();
            listCard.Add(card);
        }
    }

    void Update()
    {

    }
}
