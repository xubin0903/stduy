using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MapLayoutSO", menuName = "Map/MapLayout")]
public class MapLayoutSO : ScriptableObject
{
    public List<MapRoomData> roomDataList=new List<MapRoomData>();
    public List<LineData> lineDataList=new List<LineData>();

}
[System.Serializable]
public class MapRoomData
{
    public float x, y;
    public int column, line;
    public RoomDataSO roomDataSO;
    public RoomState roomState;
    public List<Vector2Int> connectedRooms;
}
[System.Serializable]
public class LineData
{
    public SerializeVector3 startPos;
    public SerializeVector3 endPos;
}
