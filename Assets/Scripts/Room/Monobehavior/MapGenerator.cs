using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("预制体")]
    //由于字典没办法在编辑器中预览，所以这里用数组代替
    public Room[] roomPrefabsArray;
    public RoomType[] roomTypes;
    public LineRenderer linePrefab;
    public Dictionary<RoomType, Room> roomPrefabs;
    [Header("地图配置")]
    public MapConfigSO mapConfigSO;
    public MapLayoutSO mapLayoutSO;
    private float screenHeight;
    private float screenWidth;
    private float columnWidth;
    private Vector3 generatePoint;
    public float boder;//防止左边界溢出
    public List<Room> rooms = new List<Room>();
    private List<LineRenderer> lines = new List<LineRenderer>();
    
    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth =  screenHeight * Camera.main.aspect;
        columnWidth = screenWidth / mapConfigSO.roomblueprints.Count;
        //初始化房间预制体
        roomPrefabs = new Dictionary<RoomType, Room>();
        for (int i = 0; i < roomTypes.Length; i++)
        {
            if(roomPrefabsArray[i]!= null)
            {
                roomPrefabs.Add(roomTypes[i], roomPrefabsArray[i]);
            }
        }
    }
    private void Start()
    {
        if(mapLayoutSO.roomDataList.Count==0)
        ReGenerateMap();
        else
        {
            GameManager.instance.UnlockConnectedRoos();
            LoadMap();
        }
    }
    public void CreatMap()
    {
        //生成第一列房间
        List<Room> PreviuosRooms = new List<Room>();
        for (int column = 0; column < mapConfigSO.roomblueprints.Count;column++)
        {
           

            var roomblueprint = mapConfigSO.roomblueprints[column];
            var amount = Random.Range(roomblueprint.min, roomblueprint.max);
            var generatePoint = new Vector3(-screenWidth / 2 + column * columnWidth + boder, screenHeight / 2 - screenHeight / (amount + 1), 0);
            
            //生成第二列
            List<Room> currentRooms = new List<Room>();

            
            //Debug.Log(generatePoint);
            //固定最后一列位置
            if(column == mapConfigSO.roomblueprints.Count-1)
            {
                generatePoint.x = screenWidth/2-boder*1.5f;
                generatePoint.y = 0;
            }
            for(int i = 0; i < amount; i++)
            {


                //Debug.Log("生成"+generatePoint);
                int line = i;
                var j = Random.Range(0, roomblueprint.roomTypes.Length);
                var room = Instantiate(roomPrefabs[roomblueprint.roomTypes[j]],generatePoint,Quaternion.identity,transform);
                generatePoint.y = generatePoint.y - screenHeight / (amount+1);
                room.column = column;
                room.line = line;
                if(column == 0)
                {
                    room.GetComponent<Room>().roomState = RoomState.Addressable;
                }
                else
                {
                    room.GetComponent<Room>().roomState = RoomState.Locked;
                }
                rooms.Add(room);
                currentRooms.Add(room);
            }
            
                
            //如果前一列不为空连接下一列
            if (PreviuosRooms.Count > 0)
            {
                //连接房间
                ConnetRooms(PreviuosRooms, currentRooms);

            }
            PreviuosRooms = currentRooms;
        }

    }

    private void ConnetRooms(List<Room> previuosRooms, List<Room> currentRooms)
    {
        HashSet<Room> ConnectedRooms = new HashSet<Room>();
        //随机连接房间（正向连接）
        for (int i = 0; i < previuosRooms.Count; i++)
        {
            var randomRoom = currentRooms[Random.Range(0, currentRooms.Count)];
            CreatLine(previuosRooms[i].transform, randomRoom.transform);
            ConnectedRooms.Add(randomRoom);
        }
        //如果有没有被连接的房间，则随机连接(反向链接)
        for (int i = 0; i < currentRooms.Count; i++)
        {
            if (!ConnectedRooms.Contains(currentRooms[i]))
            {
                var randomRoom = previuosRooms[Random.Range(0, previuosRooms.Count)];
               CreatLine(randomRoom.transform, currentRooms[i].transform);
                ConnectedRooms.Add(currentRooms[i]);
            }
        }
    }
    private void CreatLine(Transform start, Transform end)
    {
        
        var connectedRoom=end.GetComponent<Room>();
        start.GetComponent<Room>().connectedRooms.Add(new Vector2Int(connectedRoom.column,connectedRoom.line));
        
        
        var line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, transform);
        line.SetPosition(0, start.position);
        line.SetPosition(1, end.position);
        lines.Add(line);
    }
    //重新生成地图
    [ContextMenu("重新生成地图")]
    public void ReGenerateMap()
    {
        Debug.Log("重新生成地图");
        foreach(var room in rooms)
        {
            Destroy(room.gameObject);
        }
        foreach(var line in lines)
        {
            Destroy(line.gameObject);
        }
        rooms.Clear();
        lines.Clear();
        CreatMap();
    }
    public void SaveMap(object nothing)
    {
        Debug.Log("保存地图");
        // 保存地图
        mapLayoutSO.roomDataList.Clear();
        mapLayoutSO.lineDataList.Clear();
        for(int i = 0; i < rooms.Count; i++)
        {
            MapRoomData roomData = new MapRoomData();
            roomData.x = rooms[i].transform.position.x;
            roomData.y = rooms[i].transform.position.y;
            roomData.column=rooms[i].column;
            roomData.line=rooms[i].line;
            roomData.roomDataSO= rooms[i].roomDataSO;
            roomData.roomState= rooms[i].roomState;
            roomData.connectedRooms=rooms[i].connectedRooms;
            if (roomData.roomState == RoomState.Visited)
            {
                Debug.Log("房间"+roomData.column+","+roomData.line+"已访问");
            }
            mapLayoutSO.roomDataList.Add(roomData);
        }
        for(int i = 0; i < lines.Count; i++)
        {
           LineData lineData = new LineData();
           lineData.startPos=new SerializeVector3(lines[i].GetPosition(0));
           lineData.endPos=new SerializeVector3(lines[i].GetPosition(1));
           mapLayoutSO.lineDataList.Add(lineData);
        }
    }
    public void LoadMap()
    {
        Debug.Log("加载地图");
        // 加载地图
        rooms.Clear();
        lines.Clear();
        for(int i = 0; i < mapLayoutSO.roomDataList.Count; i++)
        {
            var roomData = mapLayoutSO.roomDataList[i];
            var room = Instantiate(roomPrefabs[roomData.roomDataSO.roomType], new Vector3(roomData.x, roomData.y, 0), Quaternion.identity, transform);
            room.roomDataSO = roomData.roomDataSO;
            room.roomState = roomData.roomState;
            room.connectedRooms = roomData.connectedRooms;
            room.column = roomData.column;
            room.line = roomData.line;
            rooms.Add(room);
        }
        for(int i = 0; i < mapLayoutSO.lineDataList.Count; i++)
        {
            var lineData = mapLayoutSO.lineDataList[i];
            var line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, transform);
            line.SetPosition(0, lineData.startPos.ToVector3());
            line.SetPosition(1, lineData.endPos.ToVector3());
            lines.Add(line);
        }
    }
}
