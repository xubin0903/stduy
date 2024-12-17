using UnityEngine;
[System.Serializable]

public class SerializeVector3 
{
    float x, y, z;

    public SerializeVector3(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }
    public Vector3 ToVector3()
    {
        return  new Vector3(x, y, z);
    }
    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }
}
