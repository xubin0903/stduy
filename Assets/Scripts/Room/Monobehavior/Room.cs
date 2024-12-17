using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("�㲥")]
    public ObjectEventSO loadRoomEvent;
    public ObjectEventSO saveRoomEvent;
    public int column;
    public int line;
    private SpriteRenderer spriteRenderer;
    public RoomDataSO roomDataSO;
    public RoomState roomState;
    public List<Vector2Int> connectedRooms=new List<Vector2Int>();

    private void OnValidate()
    {
        GetComponentInChildren<SpriteRenderer>().sprite=roomDataSO.roomIcon;
    }
    private void Awake()
    {
        spriteRenderer=GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        SetUpRoom(column, line,  roomDataSO);
    }
    private void OnMouseDown()
    {
        //Debug.Log("��������:" + roomDataSO.roomType);
        if (roomState != RoomState.Addressable)
        {
            Debug.Log("���䲻����");
            return;
        }
        GameManager.instance.selectedRoom = new Vector2Int(column,line);
        roomState = RoomState.Visited;
        Debug.Log(gameObject.name+"RoomState:"+roomState);
        saveRoomEvent.RaiseEvent(null, this);
        GameManager.instance.LockedCurrentColumnRooms();
        loadRoomEvent.RaiseEvent(roomDataSO,this);
    }
    public void Update()
    {
        if (roomState == RoomState.Locked)
        {
            
        }
    }
    public void SetUpRoom(int colume,int line,RoomDataSO roomDataSO)
    {
        this.column = colume;
        this.line = line;
        this.roomDataSO = roomDataSO;
        spriteRenderer.sprite = roomDataSO.roomIcon;
        spriteRenderer.color = roomState switch
        {
            RoomState.Locked => new Color(0.5f, 0.5f, 0.5f, 1),
            RoomState.Visited => new Color(0.5f, 0.8f, 0.5f, 0.5f),
            RoomState.Addressable => Color.white,
            _ => throw new System.ArgumentOutOfRangeException() // ����δ�г������
        };
    }
}
