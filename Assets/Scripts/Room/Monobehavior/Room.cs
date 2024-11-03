using UnityEngine;

public class Room : MonoBehaviour
{
    public int colume;
    public int line;
    private SpriteRenderer spriteRenderer;
    public RoomDataSO roomDataSO;
    public RoomState roomState;
    private void Awake()
    {
        spriteRenderer=GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        SetUpRoom(0, 0, roomDataSO);
    }
    private void OnMouseDown()
    {
        Debug.Log("房间类型:" + roomDataSO.roomType);
    }
    public void SetUpRoom(int colume,int line,RoomDataSO roomDataSO)
    {
        this.colume = colume;
        this.line = line;
        this.roomDataSO = roomDataSO;
        spriteRenderer.sprite = roomDataSO.roomIcon;
    }
}
