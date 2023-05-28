using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    // #region Singleton
    private static FieldManager _instance = null;
    public static FieldManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FieldManager>();
                if (_instance == null)
                {
                Debug.LogError("Fatal Error: BoardManager not Found");
                }
            }
            return _instance;
        }
    }
    // #endregion

    [Header("Board")]
    public Vector2Int size;
    public Vector2 offsetTile;
    public Vector2 offsetBoard;

    [Header("Tile")]
    public GameObject tilePrefab;

    [Header("Block Piece")]
    public GameObject bishop;
    public GameObject knight;
    public GameObject rock;
    public GameObject dragon;
    public GameObject blockSlot;

    [Header("Block Counter")]
    public int bishopCounter=0;
    public int knightCounter=0;
    public int rockCounter=0;
    public int dragonCounter=0;
    
    public GameObject slot;
    public int blockInSlot;

    private Vector2 startPosition;
    private Vector2 endPosition;
    private TileController[,] tiles;
    private BlockController[,] block;

    

    // #region Create Field
    void Start()
    {
        Vector2 tileSize = tilePrefab.GetComponent<SpriteRenderer>().size;
        CreateField(tileSize);
        SlotRandomizer();
        block = new BlockController[4,3];
    }

    private void CreateField(Vector2 tileSize)
    {
        tiles = new TileController[size.x, size.y];

        Vector2 totalSize = (tileSize + offsetTile) * (size - Vector2.one);

        startPosition = (Vector2)transform.position - (totalSize / 2) + offsetBoard;
        endPosition = startPosition + totalSize;
        

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                TileController newTile = Instantiate(tilePrefab, new Vector2(startPosition.x + ((tileSize.x + offsetTile.x) * x), startPosition.y + ((tileSize.y + offsetTile.y) * y)), tilePrefab.transform.rotation, transform).GetComponent<TileController>();
                tiles[x, y] = newTile;
                newTile.ChangeId(x, y);
            }
        }
    }
    // #endregion Create Field

    // #region Block Slot
    public void SlotRandomizer(){
        blockInSlot = Random.Range(0,4);
        if(blockInSlot == 0){
            blockSlot = Instantiate(bishop, new Vector3(6,0,-1),Quaternion.identity);
        }
        if(blockInSlot == 1){
            blockSlot = Instantiate(knight, new Vector3(6,0,-1),Quaternion.identity);
        }
        if(blockInSlot == 2){
            blockSlot = Instantiate(rock, new Vector3(6,0,-1),Quaternion.identity);
        }
        if(blockInSlot == 3){
            blockSlot = Instantiate(dragon, new Vector3(6,0,-1),Quaternion.identity);
        }
    }
    // #end region Block Slot

    // #region Put Block On Fields
    public void putBlockOnField(Vector2 fieldLocation){
        if(blockInSlot == 0){
            BlockController newBlock = Instantiate(bishop, new Vector3(fieldLocation.x,fieldLocation.y,-1),Quaternion.identity).GetComponent<BlockController>();
            block[blockInSlot, bishopCounter]=newBlock;
            newBlock.ChangeId(blockInSlot,bishopCounter);
            bishopCounter++;
        }
        if(blockInSlot == 1){
            BlockController newBlock = Instantiate(knight, new Vector3(fieldLocation.x,fieldLocation.y,-1),Quaternion.identity).GetComponent<BlockController>();
            block[blockInSlot, knightCounter]=newBlock;
            newBlock.ChangeId(blockInSlot,knightCounter);
            knightCounter++;
        }
        if(blockInSlot == 2){
            BlockController newBlock = Instantiate(rock, new Vector3(fieldLocation.x,fieldLocation.y,-1),Quaternion.identity).GetComponent<BlockController>();
            block[blockInSlot,rockCounter]=newBlock;
            newBlock.ChangeId(blockInSlot,rockCounter);
            rockCounter++;
        }
        if(blockInSlot == 3){
            BlockController newBlock = Instantiate(dragon, new Vector3(fieldLocation.x,fieldLocation.y,-1),Quaternion.identity).GetComponent<BlockController>();
            block[blockInSlot,dragonCounter]=newBlock;
            newBlock.ChangeId(blockInSlot,dragonCounter);
            dragonCounter++;
        }
        if(knightCounter==3 || bishopCounter==3 || rockCounter==3 || dragonCounter==3){
            DestroyCountedBlock(blockInSlot);
        }
        ScoreManager.Instance.IncrementCurrentScore(blockInSlot);
        UITime.Instance.ResetTimer();
        return;
    }

    public void TileFinder(string tileActive){
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if(tiles[x,y].name==tileActive){
                    ShowAttackZone(x,y);
                }
            }
        }
    }

    public void TileFinderHideAttackZone(string tileActive){
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if(tiles[x,y].name==tileActive){
                    HideAttackZone(x,y);
                }
            }
        }
    }

    public void ShowAttackZone(int x, int y){
        if(blockInSlot==0){
            for (int a = 1; a < size.x-x&&a<size.y-y; a++)
            {
                tiles[x+a,y+a].AttackZone();
            }
            for (int a = 1; a < size.x-x&&y-a>=0; a++)
            {
                tiles[x+a,y-a].AttackZone();
            }
            for (int a = 1; x-a>=0 && a<size.y-y; a++)
            {
                tiles[x-a,y+a].AttackZone();
            }
            for (int a = 1; x-a>=0 && y-a>=0; a++)
            {
                tiles[x-a,y-a].AttackZone();
            }
        }
        if(blockInSlot==1){
            for(int a=1; a<=2; a++){
                int b = 3-a;
                if(x+a<size.x&&y+b<size.y){
                    tiles[x+a,y+b].AttackZone();
                }
                if(x+a<size.x&&y-b>=0){
                    tiles[x+a,y-b].AttackZone();
                }
                if(x-a>=0&&y+b<size.y){
                    tiles[x-a,y+b].AttackZone();
                }
                if(x-a>=0&&y-b>=0){
                    tiles[x-a,y-b].AttackZone();
                }
            }
        }
        if(blockInSlot==2){
            for (int a = 1; a < size.x-x; a++)
            {
                tiles[x+a,y].AttackZone();
            }
            for (int a = 1; a<size.y-y; a++)
            {
                tiles[x,y+a].AttackZone();
            }
            for (int a = 1; x-a>=0 ; a++)
            {
                tiles[x-a,y].AttackZone();
            }
            for (int a = 1;  y-a>=0; a++)
            {
                tiles[x,y-a].AttackZone();
            }
        }
        if(blockInSlot==3){
            for(int a=-1;a<2;a++){
                for(int b=-1;b<2;b++){
                    if(a!=0 || b!=0){
                        if(x+a>=0 && y+b>=0 && x+a<size.x && y+b<size.y){
                            tiles[x+a,y+b].AttackZone();
                        }
                    }
                }
            }
        }
    }

    public void HideAttackZone(int x, int y){
        if(blockInSlot==0){
            for (int a = 1; a < size.x-x&&a<size.y-y; a++)
            {
                tiles[x+a,y+a].AttackZoneHide();
            }
            for (int a = 1; a < size.x-x&&y-a>=0; a++)
            {
                tiles[x+a,y-a].AttackZoneHide();
            }
            for (int a = 1; x-a>=0 && a<size.y-y; a++)
            {
                tiles[x-a,y+a].AttackZoneHide();
            }
            for (int a = 1; x-a>=0 && y-a>=0; a++)
            {
                tiles[x-a,y-a].AttackZoneHide();
            }
        }
        if(blockInSlot==1){
            for(int a=1; a<=2; a++){
                int b = 3-a;
                if(x+a<size.x&&y+b<size.y){
                    tiles[x+a,y+b].AttackZoneHide();
                }
                if(x+a<size.x&&y-b>=0){
                    tiles[x+a,y-b].AttackZoneHide();
                }
                if(x-a>=0&&y+b<size.y){
                    tiles[x-a,y+b].AttackZoneHide();
                }
                if(x-a>=0&&y-b>=0){
                    tiles[x-a,y-b].AttackZoneHide();
                }
            }
        }
        if(blockInSlot==2){
            for (int a = 1; a < size.x-x; a++)
            {
                tiles[x+a,y].AttackZoneHide();
            }
            for (int a = 1; a<size.y-y; a++)
            {
                tiles[x,y+a].AttackZoneHide();
            }
            for (int a = 1; x-a>=0 ; a++)
            {
                tiles[x-a,y].AttackZoneHide();
            }
            for (int a = 1;  y-a>=0; a++)
            {
                tiles[x,y-a].AttackZoneHide();
            }
        }
        if(blockInSlot==3){
            for(int a=-1;a<2;a++){
                for(int b=-1;b<2;b++){
                    if(a!=0 || b!=0){
                        if(x+a>=0 && y+b>=0 && x+a<size.x && y+b<size.y){
                            tiles[x+a,y+b].AttackZoneHide();
                        }
                    }
                }
            }
        }
        
    }
    // #endregion Put Block On Fields

    // #region Check Brefore Put Block On Fields

    public void TileFinderCheckAttackZone(string tileActive){
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if(tiles[x,y].name==tileActive){
                    CheckAttackZone(x,y);
                }
            }
        }
    }

    public void CheckAttackZone(int x, int y){
        if(blockInSlot==0){
            for (int a = 1; a < size.x-x&&a<size.y-y; a++)
            {
                tiles[x+a,y+a].GameOverChecker();
            }
            for (int a = 1; a < size.x-x&&y-a>=0; a++)
            {
                tiles[x+a,y-a].GameOverChecker();
            }
            for (int a = 1; x-a>=0 && a<size.y-y; a++)
            {
                tiles[x-a,y+a].GameOverChecker();
            }
            for (int a = 1; x-a>=0 && y-a>=0; a++)
            {
                tiles[x-a,y-a].GameOverChecker();
            }
        }
        if(blockInSlot==1){
            for(int a=1; a<=2; a++){
                int b = 3-a;
                if(x+a<size.x&&y+b<size.y){
                    tiles[x+a,y+b].GameOverChecker();
                }
                if(x+a<size.x&&y-b>=0){
                    tiles[x+a,y-b].GameOverChecker();
                }
                if(x-a>=0&&y+b<size.y){
                    tiles[x-a,y+b].GameOverChecker();
                }
                if(x-a>=0&&y-b>=0){
                    tiles[x-a,y-b].GameOverChecker();
                }
            }
        }
        if(blockInSlot==2){
            for (int a = 1; a < size.x-x; a++)
            {
                tiles[x+a,y].GameOverChecker();
            }
            for (int a = 1; a<size.y-y; a++)
            {
                tiles[x,y+a].GameOverChecker();
            }
            for (int a = 1; x-a>=0 ; a++)
            {
                tiles[x-a,y].GameOverChecker();
            }
            for (int a = 1;  y-a>=0; a++)
            {
                tiles[x,y-a].GameOverChecker();
            }
        }
        if(blockInSlot==3){
            for(int a=-1;a<2;a++){
                for(int b=-1;b<2;b++){
                    if(a!=0 || b!=0){
                        if(x+a>=0 && y+b>=0 && x+a<size.x && y+b<size.y){
                            tiles[x+a,y+b].GameOverChecker();
                        }
                    }
                }
            }
        }
    }

    // #endregion Check Brefore Put Block On Fields

    // #region Dextroy 3 Count Block

    public void DestroyCountedBlock(int maxBlock){
        for(int a=0;a<3;a++){ 
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2 tilesLocation = tiles[x,y].GetLocation();
                    Vector2 blockLocation = block[maxBlock,a].GetLocation();
                    if(tilesLocation== blockLocation){
                        tiles[x,y].isBlock=false;
                    }
                }
            }
            block[blockInSlot,a].DestroyBlock();
            
        }
        if(knightCounter == 3){ knightCounter=0;}
        if(bishopCounter == 3){ bishopCounter=0;}
        if(rockCounter == 3){ rockCounter=0;}
        if(dragonCounter == 3){ dragonCounter=0;}
    }

    // #endregion Dextroy 3 Count Block

}
