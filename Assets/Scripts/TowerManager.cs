using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TowerManager : Singleton<TowerManager> {

	public TowerBtn towerBtnPressed{get; set;}

	private SpriteRenderer spriteRenderer;
	private List<Tower> TowerList = new List<Tower>();
	private List<Collider2D>  buildList = new List<Collider2D>();
	//private Collider2D buildTile;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		//buildTile = GetComponent<Collider2D>();
	}
	


	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
			if(hit.collider.tag == "BuildSite"){
				RegisterBuildSite(hit.collider);
				placeTower(hit);
			}
		}
		if(spriteRenderer.enabled){
				followMouse();
				handleEscape();
			}
	}

	public void RegisterBuildSite(Collider2D buildTag){
		buildList.Add(buildTag);
	}

	public void RegisterTower (Tower tower){
		TowerList.Add(tower);
	}

	public void RenameTagsBuildSites(){
		foreach(Collider2D buildTag in buildList){
			buildTag.tag = "BuildSite";
		}
		buildList.Clear();
	}

	public void DestroyAllTower(){
		foreach(Tower tower in TowerList){
			Destroy(tower.gameObject);
		}
		TowerList.Clear();
	}

	public void selectedTower(TowerBtn towerSelected){
		if(towerSelected.TowerPrice <=GameManager.Instance.TotalMoney){
			towerBtnPressed = towerSelected;
			enableDragSprite(towerBtnPressed.DragSprite);
		}

	}

	public void placeTower(RaycastHit2D hit){
		if(!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null){
			hit.collider.tag = "BuildSiteFull";
			Tower newTower = Instantiate(towerBtnPressed.TowerObject);
			newTower.transform.position = hit.transform.position;
			buyTower(towerBtnPressed.TowerPrice);
			RegisterTower(newTower);
			disableDragSprite();
		}
	}

	public void buyTower(int price){
		GameManager.Instance.subtractMoney(price);
		Debug.Log("BUY TOWER " + price);
	}

	public void followMouse(){
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector2(transform.position.x, transform.position.y);
	}

	public void enableDragSprite(Sprite sprite){
		spriteRenderer.enabled = true;
		spriteRenderer.sprite = sprite;
	}

	public void disableDragSprite(){
		spriteRenderer.enabled = false;
		towerBtnPressed = null;
	}

	private void handleEscape(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			disableDragSprite();
			towerBtnPressed = null;
		}
	}
}
