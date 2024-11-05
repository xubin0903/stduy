using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName="MapConfigSO",menuName="Map/MapConfigSO")]
public class MapConfigSO: ScriptableObject
{
    public List<Roomblueprint> roomblueprints;
}
[System.Serializable]
public class Roomblueprint
{
    public int min, max;
    public RoomType[] roomTypes;

}