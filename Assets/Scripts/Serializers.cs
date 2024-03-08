using RPGM.Gameplay;
using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public struct GameState
{
    public Vector2 Player;
    public Vector2 Lantern;
    public Tombstone[] Tombstones;
}

public class LocationSerializer : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        Vector2 v2 = (Vector2)obj;
        info.AddValue("x", v2.x);
        info.AddValue("y", v2.y);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        Vector2 v2 = (Vector2)obj;
        v2.x = (float)info.GetValue("x", typeof(float));
        v2.y = (float)info.GetValue("y", typeof(float));
        obj = v2;
        return obj;
    }

}

public class TombestoneSerializer : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        Tombstone.State state = (Tombstone.State)obj;
        info.AddValue("state", state);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        Tombstone.State state = (Tombstone.State)obj;
        state = (Tombstone.State)info.GetValue("state", typeof(Tombstone.State));
        obj = state;
        return obj;
    }
}
