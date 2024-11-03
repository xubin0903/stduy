using System;

[Flags]
public enum RoomType
{
    averageRoom=1,eliteRoom=2,shopRoom=4,BossRoom=8,Treasure=16,restRoom=32
}
public enum RoomState
{
    Locked,Visited,Addressable
}