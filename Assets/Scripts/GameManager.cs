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
	private int maxEnemiesOnScreen;
	[SerializeField]
	private int totalEnemies;
	[SerializeField]
	private int enemiesPerSpawn;
	[SerializeField]
	private float spawnDelay = 0.5f;
	
	private int waveNumber = 0;
	private int totalMoney = 10;
	private int totalEscaped = 0;
	private int roundEscaped = 0;
	private int totalkilled = 0;
	private int whichEnemiesToSpawn = 0;
	private gameStatus currentState = gameStatus.play;

	public List<Enemy> EnemyList = new List<Enemy>();
	
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
				if(EnemyList.Count < maxEnemiesOnScreen){
					GameObject newEnemy = Instantiate(enemies[1]) as GameObject;
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
		TotalMoney += amount;
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

	// private void handleEscape(){
	// 	if(Input.GetKeyDown(KeyCode.Escape)){
	// 		TowerManager.Instance.disableDragSprite();
	// 		TowerManager.Instance.towerBtnPressed = null;
	// 	}
	// }

}
