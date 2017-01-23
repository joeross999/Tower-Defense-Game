using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gameStatus{
	next, play, gameover, win
}

public class GameManager : Singleton<GameManager> {
	
	[SerializeField]
	private int totalWaves = 10;
	[SerializeField]
	private Text totalMoneyLbl;
	[SerializeField]
	private Text currentWaveLbl;
	[SerializeField]
	private Text playBtnLbl;
	[SerializeField]
	private Text totalEscapedLbl;
	[SerializeField]
	private Button playBtn;
	[SerializeField]
	private GameObject spawnPoint;
	[SerializeField]
	private Text textFieldChangeThisName;
	[SerializeField]
	private GameObject[] enemies;
	[SerializeField]
	private int totalEnemies = 3;
	[SerializeField]
	private int enemiesPerSpawn;
	[SerializeField]
	private float spawnDelay = 0.5f;
	
	private int waveNumber = 0;
	private int totalMoney = 10;
	private int totalEscaped = 0;
	private int roundEscaped = 0;
	private int totalKilled = 0;
	private int whichEnemiesToSpawn = 0;
	private int maxEscapedEnemies = 10;
	private gameStatus currentState = gameStatus.play;

	public List<Enemy> EnemyList = new List<Enemy>();
	
	public int TotalEscaped {
		get{
			return totalEscaped;
		}
		set{
			totalEscaped = value;
		}
	}

	public int RoundEscaped {
		get{
			return roundEscaped;
		}
		set{
			roundEscaped = value;
		}
	}

	public int TotalKilled {
		get {
			return totalKilled;
		}
		set{
			totalKilled = value;
		}
	}

	public int TotalMoney{
		get{
			return totalMoney;
		}
		set{
			totalMoney = value;
			totalMoneyLbl.text = totalMoney.ToString();
		}
	}

	// Use this for initialization
	void Start () {
		playBtn.gameObject.SetActive(false);
		showMenu();
	}
	
	


	IEnumerator spawn() {
		if(enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies){
			for(int i = 0; i < enemiesPerSpawn; i++){
				if(EnemyList.Count < totalEnemies){
					GameObject newEnemy = Instantiate(enemies[whichEnemiesToSpawn]) as GameObject;
					newEnemy.transform.position = spawnPoint.transform.position;
				}
			}
			yield return new WaitForSeconds(spawnDelay);
			StartCoroutine(spawn());
		}
	}

	public void RegisterEnemy(Enemy enemy){
		EnemyList.Add(enemy);
	}

	public void UnregisterEnemy(Enemy enemy){
		EnemyList.Remove(enemy);
		Destroy(enemy.gameObject);
	}

	public void DestroyAllEnemies(){
		foreach(Enemy enemy in EnemyList){
			Destroy(enemy.gameObject);
		}
		EnemyList.Clear();
	}

	public void addMoney(int amount){
		TotalMoney += amount;
	}

	public void subtractMoney (int amount){
		TotalMoney -= amount;
	}

	public void isWaveOver(){
		totalEscapedLbl.text = "Escaped " + TotalEscaped + "/" + maxEscapedEnemies;
		if((roundEscaped +  totalKilled) == totalEnemies){

			setCurrentGameState();
			showMenu();
		}
	}

	public void setCurrentGameState(){
		if(totalEscaped >= maxEscapedEnemies){
			currentState = gameStatus.gameover;
		} else if(waveNumber == 0 && (totalKilled + roundEscaped) == 0){
			currentState = gameStatus.play;
		}else if(waveNumber >= totalWaves){
			currentState = gameStatus.win;
		} else{
			currentState = gameStatus.next;
		}
	}

	public void showMenu(){
		switch(currentState){
			case gameStatus.gameover:
			playBtnLbl.text = "Play Again!";
			//TODO add game over state
			break;
			case gameStatus.next:
			playBtnLbl.text = "Next wave";
			break;
			case gameStatus.play:
			playBtnLbl.text = "Play";
			break;
			case gameStatus.win:
			playBtnLbl.text = "Play";
			break;
		}
		playBtn.gameObject.SetActive(true);
	}

	public void playBtnPressed(){
		switch(currentState){
			case gameStatus.next:
				waveNumber += 1;
				totalEnemies += waveNumber;
				break;
			default:
				totalEnemies = 3;
				totalEscaped = 0;
				totalMoney = 10;
				TowerManager.Instance.DestroyAllTower();
				TowerManager.Instance.RenameTagsBuildSites();
				totalMoneyLbl.text = TotalMoney.ToString();
				totalEscapedLbl.text = "Escaped " + TotalEscaped + "/" + maxEscapedEnemies;
				//TODO finish resetting game
				break;
		}
		DestroyAllEnemies();
		TotalKilled = 0;
		RoundEscaped = 0;
		currentWaveLbl.text = "Wave " + (waveNumber + 1);
		StartCoroutine(spawn());
		playBtn.gameObject.SetActive(false);
	}

}
