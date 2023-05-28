using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _redHighlight;
    [SerializeField] private GameObject _attackZone;
    public bool isBlock;
    private Vector2 location;
    public FieldManager fields;

    public int idX;
    public int idY;

    private void Awake() {
        fields = FieldManager.Instance;    
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale=1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeId(int x, int y)
    {
        this.idX = x;
        this.idY = y;

        name = "TILE" + " (" + x + ", " + y + ")";
    }

    private void OnMouseEnter() {
        if(GameFlowManager.Instance.isGameOver==false){
            if(isBlock == true){
                _redHighlight.SetActive(true);
            }else{
                _highlight.SetActive(true);
                fields.TileFinder(this.name);
            }
        }
    }

    private void OnMouseExit() {
        _redHighlight.SetActive(false);
        _highlight.SetActive(false);
        fields.TileFinderHideAttackZone(this.name);
    }

    private void OnMouseDown() {
        if(GameFlowManager.Instance.isGameOver==false){
            if(isBlock == true){
                return;
            }else{
                _highlight.SetActive(false);
                _redHighlight.SetActive(true);
                fields.TileFinderHideAttackZone(this.name);
                fields.TileFinderCheckAttackZone(this.name);
                if(GameFlowManager.Instance.IsGameOver==false){
                    isBlock = true;
                    location = new Vector2(this.transform.position.x, this.transform.position.y);
                    fields.putBlockOnField(location);
                    Destroy(fields.blockSlot);
                    fields.SlotRandomizer();
                }
            }
        }
    }

    public void AttackZone(){
        _redHighlight.SetActive(true);
    }

    public void AttackZoneHide(){
        _redHighlight.SetActive(false);
    }

    public void GameOverChecker(){
        if(isBlock){
            GameFlowManager.Instance.GameOver();
        }
    }
    
    public Vector2 GetLocation(){
        location = new Vector2(this.transform.position.x, this.transform.position.y);
        return location;
    }

}
