using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Player {
    public GameObject chara, panel;
    public int id, ap;
    public Resource resource;
    public List<ActionCard> actionCard;

    public Player(int id) {     
        this.id = id;
        this.ap = 0;
        this.resource = new Resource();
        this.actionCard = new List<ActionCard>();
    }

    public void addActionCard(ActionCard actionCard) {
        this.actionCard.Add(actionCard);
    }
}

[System.Serializable]
public class Resource {
    public int food, intel, weapon;

    public Resource() {
        this.food = 0;
        this.intel = 0;
        this.weapon = 0;
    }
}

public enum GameState {
    PickCharacter,
    ScanCharacter,
    PickCharacterConfirm,
    PlaceToken,
    ShowToken,
    TurnOrder,
    TurnOrderRoll,
    GameStart,
    GameTurnChange,
    GamePlay,
    ShowActionCard,
    ShowActionCardDetail,
    UseActionCard,
    GameScanCard,
    GamePaused,
    GameEnd,
};

public class GameMenuController : MonoBehaviour {
    public GameObject mainCanvas, playerPanel, playerImagePanel, mainPanel, gamePanel, actionPanel, boardPanel, pausePanel, ARpanel,
                        actionCard, actionCardPanel, actionCardDetailPanel, pickCharaPanel, turnOrderPanel, turnRollButton, turnRollDoneButton,
                        rollButton, actionCardButton, scanButton, backButton, ActionCardTarget, CharacterCardTarget, ResourceCardTarget, TokenTarget;
    public GameObject[] chara;
    public GameObject[] grid;
    public List<Player> playerList = new List<Player>();
    public List<ActionCard> actionCardList = new List<ActionCard>();
    public List<GameObject> actionCardListPanel = new List<GameObject>();
    public Button mainPanelClick, actionCardUse;
    public Text gameText, diceText, apText, foodText, weaponText, intelText, tokenText, actionCardDetailName, actionCardDetailDescription, actionCardCost, factionRollValue, turnRollDiceValue;
    public Camera camera, ARcamera;

    public GameState gameState, lastGameState;

    private int currentPickPlayer = 0, tokenClaimed = 0;
    private Dictionary<string, int> playingFaction = new Dictionary<string, int>();
    private float ratio;

    void Start() {
        Data.playerCount = Data.playerCount != 0 ? Data.playerCount : 2;
        ratio = Screen.width / mainCanvas.GetComponent<CanvasScaler>().referenceResolution.x;
        initPlayer();
        initActionCard();
        gameText.text = "Player "+playerList[currentPickPlayer].id+" Pilih Karakter";
    }

    void Update() {
        if (gameState == GameState.PickCharacter) {
            if (Input.GetMouseButtonUp(0)) {
                pickCharacter();
            }
            else if (Input.GetKeyDown(KeyCode.Escape)) {
                pause();
            }
            else if (Input.GetKeyDown(KeyCode.A)) {
            }
        }
        else if (gameState == GameState.ScanCharacter) {
            if (Input.GetMouseButtonUp(0)) {
                //pickChara(chara[Random.Range(0, chara.Length)]);
            }
            else if (Input.GetKeyUp(KeyCode.Space)) {
                pickChara(chara[Random.Range(0, chara.Length)]);
                foreach (KeyValuePair<string, int> entry in playingFaction) {
                    Debug.Log(entry.Key + ": " + entry.Value);
                }
            }
        }
        else if (gameState == GameState.PlaceToken) {
            if (Input.GetMouseButtonUp(0)) {
                showToken();
            }
            else if (Input.GetKeyDown(KeyCode.Escape)) {
                pause();
            }
        }
        else if (gameState == GameState.ShowToken) {
            if (Input.GetMouseButtonUp(0)) {
                if(tokenClaimed == 3) {
                    gameStart();
                }
                else {
                    turnOrder();
                }                
            }
            else if (Input.GetKeyDown(KeyCode.Escape)) {
                pause();
            }
        }
        else if (gameState == GameState.TurnOrder) {
            if (Input.GetMouseButtonUp(0)) {
                turnOrderRoll();
            }
            else if (Input.GetKeyDown(KeyCode.Escape)) {
                pause();
            }
        }
        else if (gameState == GameState.GameStart) {
            if (Input.GetMouseButtonUp(0)) {
                turnChange();
            }
            else if (Input.GetKeyDown(KeyCode.Escape)) {
                pause();
            }
        }
        else if (gameState == GameState.GameTurnChange) {
            if (Input.GetMouseButtonDown(0)) {
                gamePlay();
            }
            else if (Input.GetKeyDown(KeyCode.Escape)) {
                pause();
            }
        }
        else if (gameState == GameState.GameScanCard) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                cancelCardScan();
            }
            else if (Input.GetKeyDown(KeyCode.A)) {
                cardScan(1);
            }
            else if (Input.GetKeyDown(KeyCode.S)) {
                cardScan(2);
            }
            else if (Input.GetKeyDown(KeyCode.D)) {
                cardScan(3);
            }
            else if (Input.GetKeyDown(KeyCode.W)) {
                cardScan(4);
            }
            else if (Input.GetKeyDown(KeyCode.Q)) {
                cardScan(5);
            }
        }
        else if (gameState == GameState.GamePlay) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                pause();
            }
        }
        else if (gameState == GameState.ShowActionCard) {
            if (Input.GetMouseButtonUp(0)) {
            }
            else if (Input.GetKeyDown(KeyCode.Escape)) {
                gameState = GameState.GamePlay;
                clearActionCard();
                actionCardPanel.SetActive(false);
            }
        }
        else if (gameState == GameState.ShowActionCardDetail) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                gameState = GameState.ShowActionCard;
                actionCardDetailPanel.SetActive(false);
            }
        }
    }

    private void initPlayer() {
        for(int i = 0; i < Data.playerCount; i++){
            Player player = new Player(i+1);
            player.panel = Instantiate(playerImagePanel, playerPanel.transform) as GameObject;
            player.panel.transform.position = new Vector3(player.panel.transform.position.x+(135*ratio*i), player.panel.transform.position.y, player.panel.transform.position.z);
            player.panel.GetComponentInChildren<Text>().text = "P"+player.id;
            player.panel.SetActive(true);
            playerList.Add(player);
        }
    }

    private void initActionCard() {
        ActionCard stealCard = new Steal();
        ActionCard FireTrapCard = new FireTrap();
        ActionCard pushTrapCard = new PushTrap();
        ActionCard tacticCard = new Tactic();
        ActionCard rallyCard = new Rally();
        actionCardList.Add(stealCard);
        actionCardList.Add(FireTrapCard);
        actionCardList.Add(pushTrapCard);
        actionCardList.Add(tacticCard);
        actionCardList.Add(rallyCard);
    }

    public void pickChara(GameObject chara) {
        gameState = GameState.PickCharacterConfirm;
        pickCharaPanel.GetComponentInChildren<Text>().text = chara.GetComponent<CharacterScript>().charaName;
        pickCharaPanel.GetComponentInChildren<RawImage>().texture = chara.GetComponent<CharacterScript>().charaImage;
        pickCharaPanel.SetActive(true);
        playerList[currentPickPlayer].chara = chara;
        playerList[currentPickPlayer].resource = chara.GetComponent<CharacterScript>().startingResource;
        updatePlayerResource();
    }

    private void changeCamera(Camera cam) {
    	if(cam == camera) {
    		camera.gameObject.SetActive(true);
        	ARcamera.gameObject.SetActive(false);
		} else {
			camera.gameObject.SetActive(false);
        	ARcamera.gameObject.SetActive(true);
		}
    }

    private void pickCharacter() {
        gameState = GameState.ScanCharacter;
        changeCamera(ARcamera);
    }

    private void pickCharacterOverview() {
        gameState = GameState.PickCharacter;
        changeCamera(camera);
    }

    public void onClickPickCharacterConfirm(bool pick) {
        if(pick) {
            if(!checkChara()) {
                gameState = GameState.ScanCharacter;
                pickCharaPanel.SetActive(false);
            }
            else {
                playerList[currentPickPlayer].panel.GetComponent<RawImage>().enabled = true;
                playerList[currentPickPlayer].panel.GetComponent<RawImage>().texture = playerList[currentPickPlayer].chara.GetComponent<CharacterScript>().charaIcon;
                currentPickPlayer++;
                pickCharaPanel.SetActive(false);
                if (currentPickPlayer != playerList.Count) {
                    gameText.text = "Player " + playerList[currentPickPlayer].id + " Pilih Karakter";
                    pickCharacterOverview();
                }
                else {
                    changeCamera(camera);
                    placeToken();
                }
            }            
        }
        else {
            gameState = GameState.ScanCharacter;
            pickCharaPanel.SetActive(false);
        }
    }

    private bool checkChara() {

        for(int i = 0; i< currentPickPlayer; i++) {
            if(playerList[currentPickPlayer].chara.GetComponent<CharacterScript>().charaName == playerList[i].chara.GetComponent<CharacterScript>().charaName) {
                noticeAR("Karakter sudah terpilih!");
                return false;
            }
        }

        int allowedPlayingFaction = 0, allowedPlayingCharacterPerFaction = 0;

        if (playerList.Count == 2) {
            allowedPlayingFaction = 2;
            allowedPlayingCharacterPerFaction = 1;
        }
        else if (playerList.Count == 3) {
            allowedPlayingFaction = 3;
            allowedPlayingCharacterPerFaction = 1;
        }
        else if (playerList.Count == 4) {
            allowedPlayingFaction = 2;
            allowedPlayingCharacterPerFaction = 2;
        }
        else if (playerList.Count == 6) {
            allowedPlayingFaction = 3;
            allowedPlayingCharacterPerFaction = 2;
        }

        if (playingFaction.ContainsKey(playerList[currentPickPlayer].chara.GetComponent<CharacterScript>().faction.name)) {
            if(playingFaction[playerList[currentPickPlayer].chara.GetComponent<CharacterScript>().faction.name] == allowedPlayingCharacterPerFaction) {
                noticeAR("Silahkan pilih faksi lain!");
                return false;
            }
            playingFaction[playerList[currentPickPlayer].chara.GetComponent<CharacterScript>().faction.name]++;
        }
        else {
            if(playingFaction.Count == allowedPlayingFaction) {
                noticeAR("Silahkan pilih faksi lain!");
                return false;
            }
            playingFaction.Add(playerList[currentPickPlayer].chara.GetComponent<CharacterScript>().faction.name, 1);
        }

        return true;
    }

    private void placeToken() {
        gameState = GameState.PlaceToken;
        CharacterCardTarget.SetActive(false);
        backButton.SetActive(true);
        gameText.text = "Letakkan Bintang Majapahit!";
    }

    private void showToken() {
        gameState = GameState.ShowToken;
        for(int i = 0; i < 3; i++) {
            int random = Random.Range(0, 19);
            if(!grid[random].activeSelf) {
                grid[random].SetActive(true);
            } else {
                i--;
            }
        }
        mainPanel.SetActive(false);
        boardPanel.SetActive(true);
    }

    private void turnOrder() {
        gameState = GameState.TurnOrder;
        for(int i=0; i< playingFaction.Count; i++) {
            playingFaction[playingFaction.Keys.ElementAt(i)] = 0;
        }
        boardPanel.SetActive(false);
        currentPickPlayer = 0;
        notice("Penentuan Giliran!");
    }

    private void turnOrderRoll() {
        gameState = GameState.TurnOrderRoll;
        notice("Faksi "+playingFaction.Keys.ElementAt(currentPickPlayer)+" Putar Dadu!");
        turnOrderPanel.SetActive(true);
    }

    public void onClickTurnRoll() {
        int dice;
        dice = Random.Range(1, 7);
        turnRollDiceValue.text = dice.ToString();
        factionRollValue.text = "";    
        if (playingFaction.ContainsValue(dice)) {
            notice("Dadu sama!\nFaksi " + playingFaction.Keys.ElementAt(currentPickPlayer) + " Putar Dadu Ulang!");
        }
        else {
            turnRollButton.SetActive(false);
            playingFaction[playingFaction.Keys.ElementAt(currentPickPlayer)] = dice;
            turnRollDoneButton.SetActive(true);
        }
        for (int i = 0; i < currentPickPlayer + 1; i++) {
            factionRollValue.text += playingFaction.Keys.ElementAt(i) + ": " + playingFaction.Values.ElementAt(i) + "\n";
        }
    }

    public void onClickTurnRollDone() {
        turnRollDiceValue.text = "0";
        currentPickPlayer++;
        if (currentPickPlayer == playingFaction.Count) {
            setTurnOrder();
        }
        else {
            notice("Faksi " + playingFaction.Keys.ElementAt(currentPickPlayer) + " Putar Dadu!");
            turnRollButton.SetActive(true);
            turnRollDoneButton.SetActive(false);
        }
    }

    private void setTurnOrder() {
        float xPos = playerList[0].panel.transform.position.x;
        List<int> factionRollOrder = new List<int>();
        List<string> factionOrder = new List<string>();
        turnOrderPanel.SetActive(false);
        foreach (KeyValuePair<string, int> entry in playingFaction) {
            factionRollOrder.Add(entry.Value);
        }
        factionRollOrder.Sort((a, b) => -1 * a.CompareTo(b));
        for (int i = 0; i < factionRollOrder.Count; i++) {
            for (int j = 0; j < playingFaction.Count; j++) {
                if (playingFaction.Values.ElementAt(j) == factionRollOrder[i]) {
                    factionOrder.Add(playingFaction.Keys.ElementAt(j));
                }
            }
        }
        int allowedPlayingCharacterPerFaction = 0;
        if (playerList.Count == 2) {
            allowedPlayingCharacterPerFaction = 1;
        }
        else if (playerList.Count == 3) {
            allowedPlayingCharacterPerFaction = 1;
        }
        else if (playerList.Count == 4) {
            allowedPlayingCharacterPerFaction = 2;
        }
        else if (playerList.Count == 6) {
            allowedPlayingCharacterPerFaction = 2;
        }

        Debug.Log(playerList.Count);
        for (int a = 0; a < allowedPlayingCharacterPerFaction; a++) {
            for (int i = 0; i < factionOrder.Count; i++) {
                for (int j = 0; j < playerList.Count; j++) {
                    if (playerList[j].chara.GetComponent<CharacterScript>().faction.name == factionOrder[i]) {
                        playerList.Add(playerList[j]);
                        playerList.RemoveAt(j);
                    }
                    break;
                }
            }
        }
        Debug.Log(playerList.Count);
        updatePlayerPanel(xPos);
        updatePlayerResource();
        gameStart();
    }

    private void gameStart() {
        gameState = GameState.GameStart;
        tokenClaimed = 0;
        boardPanel.SetActive(false);
        foreach (GameObject g in grid) {
            if (g.activeSelf) {
                g.SetActive(false);
            }
        }
        notice("Game Mulai!");
    }

    private void turnChange() {
        gameState = GameState.GameTurnChange;
        gameText.text = "Giliran Player "+playerList[0].id;
        playerList[0].panel.GetComponentInChildren<Text>().color = Color.green;
        diceText.text = "0";
    }

    private void gamePlay() {
        gameState = GameState.GamePlay;
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);
        
    }

    public void onClickRoll() {
        rollButton.SetActive(false);
        int dice;        
        dice = Random.Range(1, 7);
        diceText.text = dice.ToString();
        playerList[0].ap += dice;
        updatePlayerResource();
        actionPanel.SetActive(true);
        showActionCardButton();
    }

    private void showActionCardButton() {
        if(playerList[0].actionCard.Count > 0) {
            actionCardButton.SetActive(true);
        } else {
            actionCardButton.SetActive(false);
        }
    }

    public void onClickScanCard() {
    	gameState = GameState.GameScanCard;
        ActionCardTarget.SetActive(false);
        ResourceCardTarget.SetActive(false);
        TokenTarget.SetActive(false);
        scanButton.SetActive(true);
        changeCamera(ARcamera);
    }

    public void onClickEndTurn() {
        endTurn();
        if(tokenClaimed == 3) {
            placeToken();
        }
        else {
            turnChange();
        }        
    }

    private void endTurn() {
    	float xPos = playerList[0].panel.transform.position.x;
        playerList[0].panel.GetComponentInChildren<Text>().color = Color.white;
        playerList.Add(playerList[0]);
        playerList.RemoveAt(0);
        updatePlayerPanel(xPos);
        updatePlayerResource();        
        mainPanel.SetActive(true);
        gamePanel.SetActive(false);
        rollButton.SetActive(true);
        actionPanel.SetActive(false);
    }

    private void updatePlayerPanel(float xPos) {
        for(int i = 0; i < playerList.Count; i++){
            playerList[i].panel.transform.position = new Vector3(xPos+(135*ratio*i), playerList[i].panel.transform.position.y, playerList[i].panel.transform.position.z);
        }
    }

    private void updatePlayerResource() {
        apText.text = playerList[0].ap.ToString();
        foodText.text = playerList[0].resource.food.ToString();
        weaponText.text = playerList[0].resource.weapon.ToString();
        intelText.text = playerList[0].resource.intel.ToString();
        tokenText.text = playerList[0].chara.GetComponent<CharacterScript>().faction.token.ToString();
    }

    public void cardScan(int a) {
        if(a == 1) {
            getActionCard();
            showActionCardButton();
            changeCamera(camera);
        }
        else if(a == 2) {
            getResource();
            changeCamera(camera);
        }
        else if(a == 3) {
            getToken();
        }
        else if(a == 4) {
            triggerTrap("PushTrap");
        }
        else {
            triggerTrap("FireTrap");
        }
        Debug.Log("Get AC");
        gameState = GameState.GamePlay;
    }

    public void getResource(Resource newResource) {
        if(newResource.food != 0) {
            playerList[0].resource.food += newResource.food;
            string prefix = newResource.food > 0 ? "" : "- ";
            notice("Mendapatkan Sumber Daya Makanan " + prefix + newResource.food.ToString());
        }
        else if(newResource.intel != 0) {
            playerList[0].resource.intel += newResource.intel;
            string prefix = newResource.intel > 0 ? "" : "- ";
            notice("Mendapatkan Sumber Daya Pengetahuan " + prefix + newResource.intel.ToString());
        }
        else if(newResource.weapon != 0) {
            playerList[0].resource.weapon += newResource.weapon;
            string prefix = newResource.weapon > 0 ? "" : "- ";
            notice("Mendapatkan Sumber Daya Senjata " + prefix + newResource.weapon.ToString());
        }
        mainPanel.SetActive(true);
        if(playerList[0].resource.food < 0) {
            playerList[0].resource.food = 0;
        }
        else if (playerList[0].resource.weapon < 0) {
            playerList[0].resource.weapon = 0;
        }
        else if (playerList[0].resource.intel < 0) {
            playerList[0].resource.intel = 0;
        }
        updatePlayerResource();
        cancelCardScan();
    }

    private void getResource() {
        int get, random = Random.Range(0, 2);
        if(random == 1) {
            random = Random.Range(0, 3);
            get = Random.Range(1, 5);
            if(random == 1) {
                playerList[0].resource.food += get;
                notice("Mendapatkan Sumber Daya Makanan " + get.ToString());
            } else if(random == 2) {
                playerList[0].resource.weapon += get;
                notice("Mendapatkan Sumber Daya Senjata " + get.ToString());
            } else {
                playerList[0].resource.intel += get;
                notice("Mendapatkan Sumber Daya Pengetahuan " + get.ToString());
            }
        } else {
            random = Random.Range(0, 3);
            get = Random.Range(1, 3);
            if(random == 1) {
                playerList[0].resource.food = (playerList[0].resource.food -= get) >= 0 ? playerList[0].resource.food : 0;
                notice("Mendapatkan Sumber Daya Makanan -" + get.ToString());
            } else if(random == 2) {
                playerList[0].resource.weapon = (playerList[0].resource.weapon -= get) >= 0 ? playerList[0].resource.weapon : 0;
                notice("Mendapatkan Sumber Daya Senjata -" + get.ToString());
            } else {
                playerList[0].resource.intel = (playerList[0].resource.intel -= get) >= 0 ? playerList[0].resource.intel : 0;
                notice("Mendapatkan Sumber Daya Pengetahuan -" + get.ToString());
            }
        }
        updatePlayerResource();
    }

    public void getActionCard(ActionCard actionCard) {
        playerList[0].addActionCard(actionCard);
        notice("Mendapatkan Kartu Aksi " + actionCard.name());
        showActionCardButton();
        cancelCardScan();
    }

    private void getActionCard() {
        int random = Random.Range(0, actionCardList.Count);
        playerList[0].addActionCard(actionCardList[random]);
        notice("Mendapatkan Kartu Aksi " + actionCardList[random].name());
        showActionCardButton();
    }

    public void getToken() {
        playerList[0].resource.food += 10;
        playerList[0].resource.intel += 10;
        playerList[0].resource.weapon += 10;
        if (playerList[0].resource.food < 10 || playerList[0].resource.intel < 10 || playerList[0].resource.weapon < 10) {
            notice("Sumber Daya tidak mencukupi untuk\nmengambil Koin Majapahit!");
            cancelCardScan();
        }
        else {
            playerList[0].resource.food -= 10;
            playerList[0].resource.intel -= 10;
            playerList[0].resource.weapon -= 10;
            playerList[0].chara.GetComponent<CharacterScript>().faction.token++;
            updatePlayerResource();
            tokenClaimed++;
            cancelCardScan();
            FactionScript currentPlayerFaction = playerList[0].chara.GetComponent<CharacterScript>().faction;
            GetComponent<VideoMenuController>().Play(currentPlayerFaction.video[currentPlayerFaction.token - 1]);
            if(currentPlayerFaction.token == 3) {
                notice("Faksi " + currentPlayerFaction.name + " Menang!");
                mainPanelClick.onClick.AddListener(EndGame);
            } 
            else {
                notice("Mendapatkan Koin Majapahit");
            }        
        }
    }

    private void EndGame() {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void triggerTrap(string trap) {
        if(trap == "FireTrap") {
            if (playerList[0].resource.food > 0) {
                playerList[0].resource.food--;
            }
            if(playerList[0].resource.intel > 0) {
                playerList[0].resource.intel--;
            }
            if (playerList[0].resource.weapon > 0) {
                playerList[0].resource.weapon--;
            }
            updatePlayerResource();
            notice("Terkena Jebakan Api!\nSumber Daya terbakar!");
        }
        else if(trap == "PushTrap") {
            notice("Terkenda Jebakan Kayu!\nTerdorong 5 petak!");
        }
        cancelCardScan();
    }

    public void cancelCardScan() {
    	gameState = GameState.GamePlay;
		changeCamera(camera);
        ResourceCardTarget.SetActive(false);
        ActionCardTarget.SetActive(false);
        TokenTarget.SetActive(false);
    }

    public void enableCardScan() {
        ResourceCardTarget.SetActive(true);
        ActionCardTarget.SetActive(true);
        TokenTarget.SetActive(true);
        scanButton.SetActive(false);
    }

    private void notice(string notice) {
        gameText.text = notice;
        mainPanelClick.enabled = true;
        mainPanel.SetActive(true);
    }

    private void noticeAR(string notice) {
        ARpanel.GetComponentInChildren<Text>().text = notice;
        ARpanel.SetActive(true);
    }

    private void pause() {
        lastGameState = gameState;
        gameState = GameState.GamePaused;
        pausePanel.SetActive(true);
    }

    public void onClickResume() {
        gameState = lastGameState;
        pausePanel.SetActive(false);
    }

    public void onClickExit() {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void onClickActionCard() {
        gameState = GameState.ShowActionCard;
        for(int i = 0; i < playerList[0].actionCard.Count; i++) {
            GameObject card = Instantiate(actionCard, actionCardPanel.transform) as GameObject;
            card.transform.position = new Vector3(card.transform.position.x+(250*ratio*i), card.transform.position.y, card.transform.position.z);
            card.transform.SetAsFirstSibling();
            card.GetComponent<RawImage>().texture = playerList[0].actionCard[i].image();
            ActionCard actionCardClick = playerList[0].actionCard[i];
            card.GetComponent<Button>().onClick.AddListener(() => showActionCardDetail(actionCardClick));
            card.SetActive(true);
            actionCardListPanel.Add(card);
        }
        
        actionCardPanel.SetActive(true);
    }

    private void showActionCardDetail(ActionCard actionCard) {
        gameState = GameState.ShowActionCardDetail;
        actionCardDetailName.text = actionCard.name();
        actionCardCost.text = "Biaya: " + actionCard.cost().ToString() + " AP";
        actionCardDetailDescription.text = actionCard.description();
        actionCardUse.onClick.RemoveAllListeners();
        actionCardUse.onClick.AddListener(() => onClickUseActionCard(actionCard));
        actionCardDetailPanel.SetActive(true);
    }

    private void clearActionCard() {
        for(int i = 0; i < actionCardListPanel.Count; i++) {
            Destroy(actionCardListPanel[i]);
        }
        actionCardListPanel.Clear();
    }

    private void onClickUseActionCard(ActionCard actionCard) {
        if (playerList[0].ap < actionCard.cost()) {
            Debug.Log("Player: " + playerList[0].ap);
            Debug.Log("Cost: " + actionCard.cost());
            notice("AP tidak mencukupi!");
        }
        else {
            playerList[0].ap -= actionCard.cost();
            playerList[0].actionCard.Remove(actionCard);
            if(actionCard.name() == "Curi") {
                int i = 1;
                while (i < playerList.Count) {
                    if (playerList[0].chara.GetComponent<CharacterScript>().faction.name != playerList[i].chara.GetComponent<CharacterScript>().faction.name) {
                        actionCard.effect(playerList[0], playerList[i]);
                        break;
                    }
                    i++;
                }
                notice("Mencuri Sumber Daya Player " + playerList[i].id);
            }
            else if(actionCard.name() == "Strategi") {
                notice("Mendapatkan dadu tambahan");
            }
            else if (actionCard.name() == "Berkumpul") {
                notice("Silahkan melompat menuju Surya Majapahit terdekat");
            }
            else if (actionCard.name() == "Perangkap Kayu") {
                notice("Silahkan pasang Perangkap Kayu");
            }
            else if (actionCard.name() == "Perangkap Api") {
                notice("Silahkan pasang Perangkap Api");
            }
            updatePlayerResource();
        }
        clearActionCard();
        gameState = GameState.GamePlay;
        actionCardDetailPanel.SetActive(false);
        actionCardPanel.SetActive(false);
    }

}