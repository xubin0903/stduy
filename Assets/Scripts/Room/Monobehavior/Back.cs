using UnityEngine;

public class Back : MonoBehaviour
{
    public ObjectEventSO loadMapEvent;
    private void OnMouseDown()
    {
        //返回地图,广播事件
        loadMapEvent.RaiseEvent(null,this);

    }
}
