using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("预制体")]
    public Room roomPrefab;
    public LineRenderer linePrefab;
    [Header("地图配置")]
    public MapConfigSO mapConfigSO;
    private float screenHeight;
    private float screenWidth;
    private float columnWidth;
    private Vector3 generatePoint;
    public float boder;//防止左边界溢出
    private List<Room> rooms = new List<Room>();
    private List<LineRenderer> lines = new List<LineRenderer>();
 
    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth =  screenHeight * Camera.main.aspect;
        columnWidth = screenWidth / mapConfigSO.roomblueprints.Count;
    }
    private void Start()
    {
        CreatMap();
    }
    public void CreatMap()
    {
        //生成第一列房间
        List<Room> PreviuosRooms = new List<Room>();
        for (int column = 0; column < mapConfigSO.roomblueprints.Count;column++)
        {
           

            var roomblueprint = mapConfigSO.roomblueprints[column];
            var amount = Random.Range(roomblueprint.min, roomblueprint.max);
            
            var generatePoint = new Vector3(-screenWidth/2 + column  * columnWidth + boder, screenHeight/2 -screenHeight/(amount+1), 0);
            //生成第二列
            List<Room> currentRooms = new List<Room>();
            //Debug.Log(generatePoint);
            //固定最后一列位置
            if(column == mapConfigSO.roomblueprints.Count-1)
            {
                generatePoint.x = screenWidth/2-boder*1.5f;
            }
            for(int i = 0; i < amount; i++)
            {
                //Debug.Log("生成"+generatePoint);
                var room = Instantiate(roomPrefab,generatePoint,Quaternion.identity,transform);
                generatePoint.y = generatePoint.y - screenHeight / (amount+1);
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
        //随机连接房间
        for (int i = 0; i < previuosRooms.Count; i++)
        {
            var randomRoom = currentRooms[Random.Range(0, currentRooms.Count)];
            CreatLine(previuosRooms[i].transform.position, randomRoom.transform.position);
            ConnectedRooms.Add(randomRoom);
        }
        //如果有没有被连接的房间，则随机连接
        for (int i = 0; i < currentRooms.Count; i++)
        {
            if (!ConnectedRooms.Contains(currentRooms[i]))
            {
                var randomRoom = previuosRooms[Random.Range(0, previuosRooms.Count)];
               CreatLine(randomRoom.transform.position, currentRooms[i].transform.position);
                ConnectedRooms.Add(currentRooms[i]);
            }
        }
    }
    private void CreatLine(Vector3 start, Vector3 end)
    {
        var line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, transform);
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        lines.Add(line);
    }
    //重新生成地图
    [ContextMenu("重新生成地图")]
    public void ReGenerateMap()
    {
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
}
