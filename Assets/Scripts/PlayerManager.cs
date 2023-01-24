using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : NetworkBehaviour
{

    public GameObject Card;
    public GameObject Hero;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject DropZonePlayer;
    public GameObject DropZoneEnemy;
    public GameObject NetworkManager;
    public GameObject PickCharacter;
    public GameObject HeroPlayer;
    public GameObject HeroEnemy;
    public GameObject EndTurnButton;
    public GameObject ChatUI;
    public GameObject LoseScreen;
    public GameObject WinScreen;
    public GameObject GameScreen;
    public TextMeshProUGUI CurrentMoneyPlayer;
    public TextMeshProUGUI CurrentMoneyEnemy;
    public TextMeshProUGUI TurnText;

    public int deckSize;

    public CardDatabase CardDatabase;
    public HeroDatabase HeroDatabase;
    public int typeDeck;
    public int moneyThisTurn = 0;
    public int moneyAvaliable = 0;
    public int enemyMoneyThisTurn = 0;
    public int enemyMoneyAvaliable = 0;
    public bool isYourTurn = true;
    public bool hasDealtCards = false;
    public bool isWorking = false;
    public bool isFirstDraw = true;

    public readonly static List<PlayerManager> playersList = new List<PlayerManager>();
    public int thisIndex;

    public List<ThisCard> enemyCards = new List<ThisCard>();
    public List<ThisCard> playerCards = new List<ThisCard>();

    public ThisHero enemyHero;
    public ThisHero playerHero;

    public int turn = 0;

    public override void OnStartServer()
    {
        playersList.Add(this);
    }

    public void OnDisable()
    {
        playersList.Remove(this);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!isServer) playersList.Add(this);

        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("EnemyArea");
        DropZonePlayer = GameObject.Find("DropZonePlayer");
        DropZoneEnemy = GameObject.Find("DropZoneEnemy");
        NetworkManager = GameObject.Find("NetworkManager");
        CardDatabase = GameObject.Find("CardDatabase").GetComponent<CardDatabase>();
        HeroDatabase = GameObject.Find("HeroDatabase").GetComponent<HeroDatabase>();
        PickCharacter = GameObject.Find("PickADeckObjects");
        HeroEnemy = GameObject.Find("HeroEnemy");
        HeroPlayer = GameObject.Find("HeroPlayer");
        EndTurnButton = GameObject.Find("EndTurnButton");
        CurrentMoneyPlayer = GameObject.Find("CurrentMoneyPlayer").GetComponent<TextMeshProUGUI>();
        CurrentMoneyEnemy = GameObject.Find("CurrentMoneyEnemy").GetComponent<TextMeshProUGUI>();
        TurnText = GameObject.Find("TurnText").GetComponent<TextMeshProUGUI>();
        ChatUI = GameObject.Find("ChatUI");
        ChatUI.SetActive(false);
    }

    public void Update()
    {
        if (isWorking)
        {
            foreach (Transform child in DropZonePlayer.transform)
            {
                GameObject playerCard = child.gameObject;
                if (playerCard.GetComponent<ThisCard>().hp <= 0)
                {
                    playerCards.Remove(playerCard.GetComponent<ThisCard>());
                    Destroy(playerCard);
                }
            }
            foreach (Transform child in DropZoneEnemy.transform)
            {
                GameObject enemyCard = child.gameObject;
                if (enemyCard.GetComponent<ThisCard>().hp <= 0)
                {
                    enemyCards.Remove(enemyCard.GetComponent<ThisCard>());
                    Destroy(enemyCard);
                }
            }

            foreach (Transform child in HeroPlayer.transform)
            {
                GameObject hero = child.gameObject;
                if (hero.GetComponent<ThisHero>().hp <= 0)
                {
                    GameObject zoomHero = GameObject.Find("ZoomHero(Clone)");
                    GameObject zoomCard = GameObject.Find("ZoomCard(Clone)");
                    if (zoomHero != null)
                        Destroy(zoomHero);
                    if (zoomCard != null)
                        Destroy(zoomCard);
                    GameScreen.SetActive(false);
                    LoseScreen.SetActive(true);
                }
            }

            foreach (Transform child in HeroEnemy.transform)
            {
                GameObject enemy = child.gameObject;
                if (enemy.GetComponent<ThisHero>().hp <= 0)
                {
                    GameObject zoomHero = GameObject.Find("ZoomHero(Clone)");
                    GameObject zoomCard = GameObject.Find("ZoomCard(Clone)");
                    if (zoomHero != null)
                        Destroy(zoomHero);
                    if (zoomCard != null)
                        Destroy(zoomCard);
                    GameScreen.SetActive(false);
                    WinScreen.SetActive(true);
                }
            }
        }
    }

    public void UpdateEverything()
    {
        if (isWorking)
        {
            if (isYourTurn)
            {
                if (!hasDealtCards)
                {
                    hasDealtCards = true;
                    DealCards(typeDeck, thisIndex);
                }
                EndTurnButton.GetComponent<Button>().interactable = true;
                TurnText.text = "YOUR TURN!";
                CurrentMoneyPlayer.text = moneyAvaliable + "";
            }
            else
            {
                EndTurnButton.GetComponent<Button>().interactable = false;
                TurnText.text = "OPPONENT'S TURN!";
                CurrentMoneyEnemy.text = enemyMoneyAvaliable + "";
            }
        }
    }

    [Server]
    void UpdateTurnsPlayed()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.UpdateTurnsPlayed();
        RpcLogToClients("Turns Played: " + gm.TurnsPlayed);
    }

    [ClientRpc]
    void RpcLogToClients(string message)
    {
        Debug.Log(message);
    }

    public void CharacterPick(int typeChar)
    {
        CmdCharacterPick(typeChar);
    }

    [Command]
    void CmdCharacterPick(int typeChar)
    {
        GameObject hero = Instantiate(Hero, new Vector2(0, 0), Quaternion.identity);
        NetworkServer.Spawn(hero, connectionToClient);
        RpcCharacterPick(hero, typeChar);
    }

    [ClientRpc]
    void RpcCharacterPick(GameObject hero, int typeChar)
    {
        hero.GetComponent<ThisHero>().SetDataByHeroId(typeChar);

        if (hasAuthority)
        {
            int index = playersList.IndexOf(this);
            playersList[index].typeDeck = typeChar;
            playersList[index].PickCharacter.SetActive(false);
            hero.transform.SetParent(HeroPlayer.transform, false);
            if (index == 0)
            {
                playersList[index].thisIndex = 0;
                playersList[index].moneyThisTurn = 1;
                playersList[index].moneyAvaliable = 1;
                playersList[index].CurrentMoneyPlayer.text = "" + playersList[index].moneyAvaliable;
            }
            else
            {
                playersList[index].thisIndex = 1;
                playersList[index].isYourTurn = false;
                playersList[index].enemyMoneyThisTurn = 1;
                playersList[index].enemyMoneyAvaliable = 1;
                playersList[index].CurrentMoneyEnemy.text = "" + playersList[index].enemyMoneyAvaliable;
            }
            playersList[index].isWorking = true;
            playersList[index].UpdateEverything();
            ChatUI.SetActive(true);
            playersList[index].playerHero = HeroPlayer.GetComponentsInChildren<ThisHero>()[0];
            playersList[(index + 1) % 2].enemyHero = HeroPlayer.GetComponentsInChildren<ThisHero>()[0];
            LoseScreen = GameObject.Find("LoseScreen");
            LoseScreen.SetActive(false);
            WinScreen = GameObject.Find("WinScreen");
            WinScreen.SetActive(false);
            GameScreen = GameObject.Find("AllObjects");
        }
        else
        {
            int otherInd = playersList.IndexOf(this);
            List<GameObject> go = new List<GameObject>();
            foreach (Transform childT in playersList[otherInd].PickCharacter.transform)
            {
                go.Add(childT.gameObject);
            }
            foreach (GameObject child in go)
            {
                child.SetActive(true);
            }
            hero.transform.SetParent(HeroEnemy.transform, false);
            playersList[otherInd].enemyHero = HeroEnemy.GetComponentsInChildren<ThisHero>()[0];
            playersList[(otherInd + 1) % 2].playerHero = HeroEnemy.GetComponentsInChildren<ThisHero>()[0];
        }
    }

    public void DealCards(int type, int ind)
    {
        CmdDealCards(type, ind);
    }

    [Command(requiresAuthority = false)]
    public void CmdDealCards(int type, int ind)
    {
        GameObject card = Instantiate(Card, new Vector2(0, 0), Quaternion.identity);
        NetworkServer.Spawn(card, connectionToClient);
        int num = 3;
        if (type == 0)
        {
            num = Random.Range(0, CardDatabase.mafiaCardList.Count);
        }
        else if (type == 1)
        {
            num = Random.Range(0, CardDatabase.policeCardList.Count);
        }
        else if (type == 2)
        {
            num = Random.Range(0, CardDatabase.peopleCardList.Count);
        }
        else
        {
            Debug.Log("No type like this");
        }
        RpcSaveNumber(card, num, type);
        RpcDealCard(card, ind, type);
        if (playersList[ind].isFirstDraw)
        {
            for (int i = 0; i <= 3; i++)
            {
                GameObject card1 = Instantiate(Card, new Vector2(0, 0), Quaternion.identity);
                NetworkServer.Spawn(card1, connectionToClient);
                int num1 = 3;
                if (type == 0)
                {
                    num1 = Random.Range(0, CardDatabase.mafiaCardList.Count);
                }
                else if (type == 1)
                {
                    num1 = Random.Range(0, CardDatabase.policeCardList.Count);
                }
                else if (type == 2)
                {
                    num1 = Random.Range(0, CardDatabase.peopleCardList.Count);
                }
                else
                {
                    Debug.Log("No type like this");
                }
                RpcSaveNumber(card1, num1, type);
                RpcDealCard(card1, ind, type);
            }
            playersList[ind].isFirstDraw = false;
        }
    }

    [ClientRpc]
    public void RpcSaveNumber(GameObject card, int num, int type)
    {
        card.GetComponent<NumberKeeper>().number = num;
        card.GetComponent<NumberKeeper>().type = type;
    }

    [ClientRpc]
    void RpcDealCard(GameObject card, int ind, int typeDeck)
    {
        int cardId = card.GetComponent<NumberKeeper>().number;
        card.GetComponent<ThisCard>().SetDataByCardId(cardId, typeDeck);
        if (hasAuthority)
        {
            card.transform.SetParent(PlayerArea.transform, false);
            playersList[ind].deckSize--;
        }
        else
        {
            card.transform.SetParent(EnemyArea.transform, false);
            card.GetComponent<CardFlipper>().Flip();
        }
    }

    public void PlayCard(GameObject card, int ind, int cost)
    {
        CmdPlayCard(card, ind, cost);
    }

    [Command]
    void CmdPlayCard(GameObject card, int ind, int cost)
    {
        RpcPlayCard(card, ind, cost);
        GameObject card1 = Instantiate(Card, new Vector2(0, 0), Quaternion.identity);
        NetworkServer.Spawn(card1, connectionToClient);
        RpcCardEffects(card, ind, card1);
    }

    [ClientRpc]
    void RpcPlayCard(GameObject card, int ind, int cost)
    {
        if (playersList[ind].isYourTurn)
        {
            if (hasAuthority)
            {
                if (playersList[ind].moneyAvaliable - cost >= 0)
                {
                    card.transform.SetParent(DropZonePlayer.transform, false);
                    playersList[ind].moneyAvaliable -= cost;
                    playersList[ind].CurrentMoneyPlayer.text = "" + playersList[ind].moneyAvaliable;
                    playersList[ind].playerCards.Add(card.GetComponent<ThisCard>());
                    playersList[(ind + 1) % 2].enemyCards.Add(card.GetComponent<ThisCard>());
                }
                else
                {
                    card.transform.SetParent(PlayerArea.transform, false);
                }
            }
            else
            {
                int otherInd = (ind + 1) % 2;
                if (playersList[otherInd].enemyMoneyAvaliable - cost >= 0)
                {
                    card.GetComponent<CardFlipper>().Flip();
                    card.transform.SetParent(DropZoneEnemy.transform, false);
                    playersList[otherInd].enemyMoneyAvaliable -= cost;
                    playersList[otherInd].CurrentMoneyEnemy.text = "" + playersList[otherInd].enemyMoneyAvaliable;
                    playersList[otherInd].enemyCards.Add(card.GetComponent<ThisCard>());
                    playersList[ind].playerCards.Add(card.GetComponent<ThisCard>());
                }
                else
                {
                    card.transform.SetParent(EnemyArea.transform, false);
                }
            }
        }
        else
        {
            if (hasAuthority)
            {
                card.transform.SetParent(PlayerArea.transform, false);
            }
            else
            {
                card.transform.SetParent(EnemyArea.transform, false);
            }
        }
    }

    [ClientRpc]
    void RpcCardEffects(GameObject card, int ind, GameObject anotherCard)
    {
        ThisCard thisCard = card.GetComponent<ThisCard>();
        int idCard = thisCard.id;
        string minionCard = thisCard.minionType;
        int otherId = (ind + 1) % 2;
        if (hasAuthority)
        {
            if (minionCard == "Mafia")
            {
                if (idCard == 1)
                {
                    anotherCard.GetComponent<ThisCard>().SetDataByCardId(0, 3);
                    anotherCard.transform.SetParent(playersList[ind].DropZonePlayer.transform, false);
                    playersList[ind].playerCards.Add(anotherCard.GetComponent<ThisCard>());
                    playersList[otherId].enemyCards.Add(anotherCard.GetComponent<ThisCard>());
                }
                else if (idCard == 2)
                {
                    Destroy(anotherCard);
                    ThisCard[] allCards = playersList[ind].DropZonePlayer.GetComponentsInChildren<ThisCard>();
                    foreach (ThisCard cardData in allCards)
                    {
                        if (cardData != thisCard)
                        {
                            cardData.UpdateCardHpAndAttack(0, 1);
                        }
                    }
                }
            }
            else if (minionCard == "Police")
            {
                Destroy(anotherCard);
                if (idCard == 2)
                {
                    ThisCard[] allCards = playersList[ind].DropZoneEnemy.GetComponentsInChildren<ThisCard>();
                    foreach (ThisCard cardData in allCards)
                    {
                        if (cardData != thisCard)
                        {
                            cardData.UpdateCardHpAndAttack(0, -1);
                        }
                    }
                }
            }
            else if (minionCard == "The People")
            {
                Destroy(anotherCard);
                if (idCard == 2)
                {
                    ThisCard[] allCards = playersList[ind].DropZonePlayer.GetComponentsInChildren<ThisCard>();
                    foreach (ThisCard cardData in allCards)
                    {
                        if (cardData != thisCard)
                        {
                            cardData.UpdateCardHpAndAttack(2, 0);
                        }
                    }
                }
            }
            else
            {
                Destroy(anotherCard);
                Debug.Log("Something is wrong with the card");
            }
        }
        else
        {
            if (minionCard == "Mafia")
            {
                if (idCard == 1)
                {
                    anotherCard.GetComponent<ThisCard>().SetDataByCardId(0, 3);
                    anotherCard.transform.SetParent(playersList[otherId].DropZoneEnemy.transform, false);
                    playersList[ind].playerCards.Add(anotherCard.GetComponent<ThisCard>());
                    playersList[otherId].enemyCards.Add(anotherCard.GetComponent<ThisCard>());
                }
                else if (idCard == 2)
                {
                    Destroy(anotherCard);
                    ThisCard[] allCards = playersList[otherId].DropZoneEnemy.GetComponentsInChildren<ThisCard>();
                    foreach (ThisCard cardData in allCards)
                    {
                        if (cardData != thisCard)
                        {
                            cardData.UpdateCardHpAndAttack(0, 1);
                        }
                    }
                }
            }
            else if (minionCard == "Police")
            {
                Destroy(anotherCard);
                if (idCard == 2)
                {
                    ThisCard[] allCards = playersList[otherId].DropZonePlayer.GetComponentsInChildren<ThisCard>();
                    foreach (ThisCard cardData in allCards)
                    {
                        if (cardData != thisCard)
                        {
                            cardData.UpdateCardHpAndAttack(0, -1);
                        }
                    }
                }
            }
            else if (minionCard == "The People")
            {
                Destroy(anotherCard);
                if (idCard == 2)
                {
                    ThisCard[] allCards = playersList[otherId].DropZoneEnemy.GetComponentsInChildren<ThisCard>();
                    foreach (ThisCard cardData in allCards)
                    {
                        if (cardData != thisCard)
                        {
                            cardData.UpdateCardHpAndAttack(2, 0);
                        }
                    }
                }

            }
            else
            {
                Destroy(anotherCard);
                Debug.Log("Something is wrong with the card");
            }
        }
    }

    public void EndTurn(int ind1)
    {
        CmdEndTurn(ind1);
    }

    [Command]
    void CmdEndTurn(int ind1)
    {
        RpcEndTurn(ind1);
        int whoAttacksNow = Random.Range(0, 2);
        RpcFight(ind1, whoAttacksNow);
        if (isServer)
        {
            UpdateTurnsPlayed();
        }
    }

    [ClientRpc]
    void RpcEndTurn(int ind1)
    {
        playersList[ind1].isYourTurn = false;
        playersList[ind1].enemyMoneyThisTurn++;
        playersList[ind1].enemyMoneyAvaliable = playersList[ind1].enemyMoneyThisTurn;
        playersList[ind1].turn++;
        playersList[ind1].UpdateEverything();
        int ind2 = (ind1 + 1) % 2;

        playersList[ind2].isYourTurn = true;

        playersList[ind2].hasDealtCards = false;
        playersList[ind2].moneyThisTurn++;
        playersList[ind2].moneyAvaliable = playersList[ind2].moneyThisTurn;
        playersList[ind2].turn++;
        playersList[ind2].UpdateEverything();
    }

    [ClientRpc]
    void RpcFight(int ind1, int whoAttacksNow)
    {
        if (playersList[ind1].turn % 2 != 0)
        {
            return;
        }
        bool decreasedHealth = false;
        int ind2 = (ind1 + 1) % 2;
        List<ThisCard> enemyCardsForPlayer1 = playersList[ind1].enemyCards;
        List<ThisCard> playerCardsForPlayer1 = playersList[ind1].playerCards;
        List<ThisCard> playerCardsForPlayer2 = playersList[ind2].playerCards;
        List<ThisCard> enemyCardsForPlayer2 = playersList[ind2].enemyCards;
        if (whoAttacksNow == 1)
        {
            if (playerCardsForPlayer1.Count != 0)
            {
                if (enemyCardsForPlayer1.Count == 0)
                {
                    int i = 0;
                    foreach (ThisCard card in playerCardsForPlayer1)
                    {
                        if (card == null)
                        {
                            continue;
                        }
                        int power = card.power;
                        if (hasAuthority)
                        {
                            ThisHero player2 = HeroEnemy.GetComponentsInChildren<ThisHero>()[0];
                            int playerDamage = player2.heroPower.cost * 2;
                            if ((card.cardName != "Gun For Hire") && (card.cardName != "Trainee") && (card.cardName != "Peasant"))
                                card.UpdateCardHpAndAttack(-playerDamage, 0);                                
                            player2.UpdateHeroHp(-power);
                        }
                        else
                        {
                            ThisHero player1 = HeroPlayer.GetComponentsInChildren<ThisHero>()[0];
                            int playerDamage = player1.heroPower.cost * 2;
                            if ((card.cardName != "Gun For Hire") && (card.cardName != "Trainee") && (card.cardName != "Peasant"))
                                card.UpdateCardHpAndAttack(-playerDamage, 0);
                            player1.UpdateHeroHp(-power);
                        }
                        i++;
                    }
                }
                else
                {
                    if (!decreasedHealth)
                    {
                        foreach (ThisCard card in playerCardsForPlayer1)
                        {
                            card.UpdateCardHpAndAttack(-card.cost, 0);
                        }

                        foreach (ThisCard card in playerCardsForPlayer2)
                        {
                            card.UpdateCardHpAndAttack(-card.cost, 0);
                        }
                        decreasedHealth = true;
                    }
                }                
            }

            playerCardsForPlayer2 = playersList[ind2].playerCards;
            enemyCardsForPlayer2 = playersList[ind2].enemyCards;
            if (playerCardsForPlayer2.Count != 0)
            {
                if (enemyCardsForPlayer2.Count == 0)
                {
                    int i = 0;
                    foreach (ThisCard card in playerCardsForPlayer2)
                    {
                        if (card == null)
                        {
                            continue;
                        }
                        int power = card.power;
                        if (hasAuthority)
                        {
                            ThisHero player1 = HeroPlayer.GetComponentsInChildren<ThisHero>()[0];
                            int playerDamage = player1.heroPower.cost * 2;
                            if ((card.cardName != "Gun For Hire") && (card.cardName != "Trainee") && (card.cardName != "Peasant"))
                                card.UpdateCardHpAndAttack(-playerDamage, 0);
                            player1.UpdateHeroHp(-power);
                        }
                        else
                        {
                            ThisHero player2 = HeroEnemy.GetComponentsInChildren<ThisHero>()[0];
                            int playerDamage = player2.heroPower.cost * 2;
                            if ((card.cardName != "Gun For Hire") && (card.cardName != "Trainee") && (card.cardName != "Peasant"))
                                card.UpdateCardHpAndAttack(-playerDamage, 0);
                            player2.UpdateHeroHp(-power);
                        }
                        i++;
                    }
                }
                else
                {
                    if (!decreasedHealth)
                    {
                        foreach (ThisCard card in playerCardsForPlayer1)
                        {
                            card.UpdateCardHpAndAttack(-card.cost, 0);
                        }

                        foreach (ThisCard card in playerCardsForPlayer2)
                        {
                            card.UpdateCardHpAndAttack(-card.cost, 0);
                        }
                        decreasedHealth = true;
                    }
                }
            }
            return;
        }


        if (playerCardsForPlayer2.Count != 0)
        {
            if (enemyCardsForPlayer2.Count == 0)
            {
                int i = 0;
                foreach (ThisCard card in playerCardsForPlayer2)
                {
                    if (card == null)
                    {
                        continue;
                    }
                    int power = card.power;
                    if (hasAuthority)
                    {
                        ThisHero player1 = HeroPlayer.GetComponentsInChildren<ThisHero>()[0];
                        int playerDamage = player1.heroPower.cost * 2;
                        if ((card.cardName != "Gun For Hire") && (card.cardName != "Trainee") && (card.cardName != "Peasant"))
                            card.UpdateCardHpAndAttack(-playerDamage, 0);
                        player1.UpdateHeroHp(-power);
                    }
                    else
                    {
                        ThisHero player2 = HeroEnemy.GetComponentsInChildren<ThisHero>()[0];
                        int playerDamage = player2.heroPower.cost * 2;
                        if ((card.cardName != "Gun For Hire") && (card.cardName != "Trainee") && (card.cardName != "Peasant"))
                            card.UpdateCardHpAndAttack(-playerDamage, 0);
                        player2.UpdateHeroHp(-power);
                    }
                    i++;
                }
            }
            else
            {
                if (!decreasedHealth)
                {
                    foreach (ThisCard card in playerCardsForPlayer1)
                    {
                        card.UpdateCardHpAndAttack(-card.cost, 0);
                    }

                    foreach (ThisCard card in playerCardsForPlayer2)
                    {
                        card.UpdateCardHpAndAttack(-card.cost, 0);
                    }
                    decreasedHealth = true;
                }
            }
        }

        enemyCardsForPlayer1 = playersList[ind1].enemyCards;
        playerCardsForPlayer1 = playersList[ind1].playerCards;
        if (playerCardsForPlayer1.Count != 0)
        {
            if (enemyCardsForPlayer1.Count == 0)
            {
                int i = 0;
                foreach (ThisCard card in playerCardsForPlayer1)
                {
                    if (card == null)
                    {
                        continue;
                    }
                    Debug.Log(card.name);
                    int power = card.power;
                    if (hasAuthority)
                    {
                        ThisHero player2 = HeroEnemy.GetComponentsInChildren<ThisHero>()[0];
                        int playerDamage = player2.heroPower.cost * 2;
                        if ((card.cardName != "Gun For Hire") && (card.cardName != "Trainee") && (card.cardName != "Peasant"))
                            card.UpdateCardHpAndAttack(-playerDamage, 0);
                        player2.UpdateHeroHp(-power);
                    }
                    else
                    {
                        ThisHero player1 = HeroPlayer.GetComponentsInChildren<ThisHero>()[0];
                        int playerDamage = player1.heroPower.cost * 2;
                        if ((card.cardName != "Gun For Hire") && (card.cardName != "Trainee") && (card.cardName != "Peasant"))
                            card.UpdateCardHpAndAttack(-playerDamage, 0);
                        player1.UpdateHeroHp(-power);
                    }
                    i++;
                }
            }
            else
            {
                if (!decreasedHealth)
                {
                    foreach (ThisCard card in playerCardsForPlayer1)
                    {
                        card.UpdateCardHpAndAttack(-card.cost, 0);
                    }

                    foreach (ThisCard card in playerCardsForPlayer2)
                    {
                        card.UpdateCardHpAndAttack(-card.cost, 0);
                    }
                    decreasedHealth = true;
                }
            }
        }
    }

    public void TriggerHeroPower(int ind, int a1, int a2, int a3, int a4, int hpType, int cost)
    {
        CmdTriggerHeroPower(ind, a1, a2, a3, a4, hpType, cost);
    }

    [Command]
    void CmdTriggerHeroPower(int ind, int a1, int a2, int a3, int a4, int hpType, int cost)
    {
        RpcTriggerHeroPower(ind, a1, a2, a3, a4, hpType, cost);
    }

    [ClientRpc]
    void RpcTriggerHeroPower(int ind, int a1, int a2, int a3, int a4, int heroP, int cost)
    {
        if (heroP == 0)
        {
            if (hasAuthority)
            {
                ThisCard[] cardsEnemy = DropZoneEnemy.GetComponentsInChildren<ThisCard>();
                if (cardsEnemy.Length >= 2)
                {
                    cardsEnemy[a1].UpdateCardHpAndAttack(-cardsEnemy[a2].power, 0);
                    cardsEnemy[a2].UpdateCardHpAndAttack(-cardsEnemy[a1].power, 0);
                }
                playersList[ind].moneyAvaliable -= cost;
                playersList[ind].CurrentMoneyPlayer.text = "" + playersList[ind].moneyAvaliable;
            }
            else
            {
                ThisCard[] cardsPlayer = DropZonePlayer.GetComponentsInChildren<ThisCard>();
                if (cardsPlayer.Length >= 2)
                {
                    cardsPlayer[a1].UpdateCardHpAndAttack(-cardsPlayer[a2].power, 0);
                    cardsPlayer[a2].UpdateCardHpAndAttack(-cardsPlayer[a1].power, 0);
                }
                int otherInd = (ind + 1) % 2;
                playersList[otherInd].enemyMoneyAvaliable -= cost;
                playersList[otherInd].CurrentMoneyEnemy.text = "" + playersList[otherInd].enemyMoneyAvaliable;
            }
            return;
        }
        if (heroP == 1)
        {
            if (hasAuthority)
            {
                if (a1 == 0)
                {
                    ThisHero attacking = HeroEnemy.GetComponentsInChildren<ThisHero>()[0];
                    attacking.UpdateHeroHp(-3);
                }
                else
                {
                    ThisCard[] cardsEnemy = DropZoneEnemy.GetComponentsInChildren<ThisCard>();
                    cardsEnemy[a2].UpdateCardHpAndAttack(-3, 0);
                }
                playersList[ind].moneyAvaliable -= cost;
                playersList[ind].CurrentMoneyPlayer.text = "" + playersList[ind].moneyAvaliable;
            }
            else
            {
                if (a1 == 0)
                {
                    ThisHero attacking = HeroPlayer.GetComponentsInChildren<ThisHero>()[0];
                    attacking.UpdateHeroHp(-3);
                }
                else
                {
                    ThisCard[] cardsPlayer = DropZonePlayer.GetComponentsInChildren<ThisCard>();
                    cardsPlayer[a2].UpdateCardHpAndAttack(-3, 0);
                }
                int otherInd = (ind + 1) % 2;
                playersList[otherInd].enemyMoneyAvaliable -= cost;
                playersList[otherInd].CurrentMoneyEnemy.text = "" + playersList[otherInd].enemyMoneyAvaliable;
            }
            return;
        }
        if (heroP == 2)
        {
            if (hasAuthority)
            {
                ThisCard[] enemyCards = DropZoneEnemy.GetComponentsInChildren<ThisCard>();
                ThisCard[] playerCards = DropZonePlayer.GetComponentsInChildren<ThisCard>();
                enemyCards[a1].UpdateCardHpAndAttack(-1, 0);
                enemyCards[a2].UpdateCardHpAndAttack(-1, 0);
                enemyCards[a3].UpdateCardHpAndAttack(-1, 0);
                playerCards[a4].UpdateCardHpAndAttack(-1, 0);
                playersList[ind].moneyAvaliable -= cost;
                playersList[ind].CurrentMoneyPlayer.text = "" + playersList[ind].moneyAvaliable;
            }
            else
            {
                ThisCard[] enemyCards = DropZoneEnemy.GetComponentsInChildren<ThisCard>();
                ThisCard[] playerCards = DropZonePlayer.GetComponentsInChildren<ThisCard>();
                playerCards[a1].UpdateCardHpAndAttack(-1, 0);
                playerCards[a2].UpdateCardHpAndAttack(-1, 0);
                playerCards[a3].UpdateCardHpAndAttack(-1, 0);
                enemyCards[a4].UpdateCardHpAndAttack(-1, 0);
                int otherInd = (ind + 1) % 2;
                playersList[otherInd].enemyMoneyAvaliable -= cost;
                playersList[otherInd].CurrentMoneyEnemy.text = "" + playersList[otherInd].enemyMoneyAvaliable;
            }
            return;
        }
    }
}
