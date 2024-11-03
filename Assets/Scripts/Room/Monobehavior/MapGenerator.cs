using System.Runtime.CompilerServices;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Room roomPrefab;
    public MapConfigSO mapConfigSO;
    private float screenHeight;
    private float screemWidth;
    private float columnWidth;
    private Vector3 generatePoint;
    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screemWidth = Screen.width * Camera.main.aspect;
        columnWidth = Screen.width / (mapConfigSO.roomblueprints.Count + 1);//加一给最后一行留出空间防止跑出屏幕
    }

    public void CreatMap()
    {
        for (int column = 0; column < mapConfigSO.roomblueprints.Count;column++)
        {
            var roomblueprint = mapConfigSO.roomblueprints[column];
            var amount = Random.Range(roomblueprint.min, roomblueprint.max);


            for(int i = 0; i < amount; i++)
            {
                var room = Instantiate(roomPrefab, transform);
            }
        }
    }
}
