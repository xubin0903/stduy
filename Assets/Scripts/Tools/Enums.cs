using System;

[Flags]
public enum RoomType
{
    AverageRoom=1,EliteRoom=2,ShopRoom=4,BossRoom=8,Treasure=16,RestRoom=32
}
public enum RoomState
{
    Locked,Visited,Addressable
}