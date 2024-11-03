using UnityEngine;
using UnityEngine.AddressableAssets;
[CreateAssetMenu(fileName ="RoomDataSO",menuName="Map/RoomDataSO")]
public class RoomDataSO : ScriptableObject
{
    public RoomType roomType;
    public Sprite roomIcon;
    public AssetReference sceneToLoad;

}