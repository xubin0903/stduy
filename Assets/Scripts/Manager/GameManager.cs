using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public MapLayoutSO mapLayout;
    public Vector2Int selectedRoom;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    public void UnlockConnectedRoos()
    {
        for (int i = 0; i < mapLayout.roomDataList.Count; i++)
        {
            if (new Vector2Int (mapLayout.roomDataList[i].column, mapLayout.roomDataList[i].line) == selectedRoom)
            {
                foreach(Vector2Int direction in mapLayout.roomDataList[i].connectedRooms)
                {
                    foreach(MapRoomData room in mapLayout.roomDataList)
                    {
                        if(new Vector2Int(room.column, room.line) == direction)
                        {
                            room.roomState = RoomState.Addressable;
                        }
                    }
                }
            }
        }
    }
    public void LockedCurrentColumnRooms()
    {
        for (int i = 0; i < mapLayout.roomDataList.Count; i++)
        {
            if(mapLayout.roomDataList[i].column == selectedRoom.x&& mapLayout.roomDataList[i].roomState!= RoomState.Visited)
            {
                mapLayout.roomDataList[i].roomState = RoomState.Locked;
            }
        }
    }
    public void OnApplicationQuit()
    {
        mapLayout.roomDataList.Clear();
        mapLayout.lineDataList.Clear();
        mapLayout = null;
    }
}
