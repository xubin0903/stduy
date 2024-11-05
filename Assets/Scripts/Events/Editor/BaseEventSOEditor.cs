using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
[CustomEditor(typeof(BaseEventSO<>))]
public class BaseEventSOEditor<T> : Editor
{
    private BaseEventSO<T> baseEventSO;
    private void OnEnable()
    {
        if(baseEventSO==null)
            baseEventSO = target as BaseEventSO<T>;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //��ʾ���������ƺ�����
        EditorGUILayout.LabelField("��������:"+GetListeners().Count);
        foreach (var listener in GetListeners())
        {
            if (listener != null)
            {
                EditorGUILayout.LabelField(listener.name);
            }
        }
    }
  private List<MonoBehaviour> GetListeners()
    {
        //Ϊ�˷�ֹ������
        if (baseEventSO == null || baseEventSO.onEventRaised == null)
        {
            return new List<MonoBehaviour>();
        }
        List<MonoBehaviour> listeners = new List<MonoBehaviour>();
        var subscribes = baseEventSO.onEventRaised.GetInvocationList();
        foreach (var subscribe in subscribes)
        {
            var obj = subscribe.Target as MonoBehaviour;
            if (!listeners.Contains(obj))
            listeners.Add(obj);
        }
        return listeners;
    }
}
