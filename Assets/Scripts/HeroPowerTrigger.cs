using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HeroPowerTrigger : NetworkBehaviour
{
    public PlayerManager PlayerManager;
    public void OnClick()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();

        int card1 = -1;
        int card2 = -1;
        List<PlayerManager> playersList = PlayerManager.playersList;
        int ind = PlayerManager.thisIndex;
        if (!playersList[ind].isYourTurn)
        {
            return;
        }
        int cost = HeroPowerDatabase.powerList[PlayerManager.typeDeck].cost;
        if (playersList[ind].moneyAvaliable - cost >= 0)
        {
            if (playersList[ind].typeDeck == 0)
            {
                List<ThisCard> cardsEnemy = playersList[ind].enemyCards;
                int cnt = cardsEnemy.Count;
                Debug.Log(cnt);
                if (cnt >= 2)
                {
                    card1 = Random.Range(0, cnt);
                    card2 = card1;
                    while (card2 == card1)
                    {
                        card2 = Random.Range(0, cnt);
                    }
                    PlayerManager.TriggerHeroPower(PlayerManager.thisIndex, card1, card2, -1, -1, 0, cost);
                }
                return;
            }
            if (playersList[ind].typeDeck == 1)
            {
                int attacking = Random.Range(0, 2);
                int cardId = -1;
                if (attacking == 1)
                {
                    List<ThisCard> cardsEnemy = playersList[ind].enemyCards;
                    int cnt = cardsEnemy.Count;
                    if (cnt >= 1)
                    {
                        cardId = Random.Range(0, cnt);
                    }
                    else
                        attacking = 0;
                }
                PlayerManager.TriggerHeroPower(PlayerManager.thisIndex, attacking, cardId, -1, -1, 1, cost);
                return;
            }
            if (playersList[ind].typeDeck == 2)
            {
                List<ThisCard> cardsEnemy = playersList[ind].enemyCards;
                List<ThisCard> cardsPlayer = playersList[ind].playerCards;
                int cnt1 = cardsEnemy.Count;
                int cnt2 = cardsPlayer.Count;
                if ((cnt1 >= 1) && (cnt2 >= 1))
                {
                    int a1 = Random.Range(0, cnt1);
                    int a2 = Random.Range(0, cnt1);
                    int a3 = Random.Range(0, cnt1);
                    int a4 = Random.Range(0, cnt2);
                    PlayerManager.TriggerHeroPower(PlayerManager.thisIndex, a1, a2, a3, a4, 2, cost);
                }
                return;
            }
        }
    }
}
