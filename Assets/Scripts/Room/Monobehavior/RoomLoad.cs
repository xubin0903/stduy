using UnityEngine;

public class RoomLoad : MonoBehaviour
{
    public void LoadRoom(object roomData)
    {
        var roomDataSO = (RoomDataSO)roomData;
        if(roomDataSO is RoomDataSO)
        {
            Debug.Log(roomDataSO.roomType);
        }
    }
}
