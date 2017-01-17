using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Singleton<TowerManager> {

	public TowerBtn towerBtnPressed{get; set;}

	private SpriteRenderer spriteRenderer;


	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
			if(hit.collider.tag == "BuildSite"){
				placeTower(hit);
			}
		}
		if(spriteRenderer.enabled){
				followMouse();
				handleEscape();
			}
	}

	public void selectedTower(TowerBtn towerSelected){
		towerBtnPressed = towerSelected;
		enableDragSprite(towerBtnPressed.DragSprite);

	}

	public void placeTower(RaycastHit2D hit){
		if(!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null){
			hit.collider.tag = "BuildSiteFull";
			GameObject newTower = Instantiate(towerBtnPressed.TowerObject);
			newTower.transform.position = hit.transform.position;
			disableDragSprite();
		}
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
