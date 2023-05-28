using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{

    private Vector2 location;
    public FieldManager fields;

    private void Awake() {
        fields = FieldManager.Instance;    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        location = new Vector2(this.transform.position.x, this.transform.position.y);
    }

    public void DestroyBlock() {
        Destroy(this.gameObject);
    }

    public Vector2 GetLocation(){
        location = new Vector2(this.transform.position.x, this.transform.position.y);
        return location;
    }

    public void ChangeId(int id, int count)
    {
        name = "block" + id + " (" + count + ")";
    }
}
