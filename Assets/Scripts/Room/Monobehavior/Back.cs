using UnityEngine;

public class Back : MonoBehaviour
{
    public ObjectEventSO loadMapEvent;
    private void OnMouseDown()
    {
        //���ص�ͼ,�㲥�¼�
        loadMapEvent.RaiseEvent(null,this);

    }
}
