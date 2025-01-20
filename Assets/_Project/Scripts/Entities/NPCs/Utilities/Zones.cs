using System;

namespace ATBMI.Entities.NPCs
{
    public enum ZoneType
    {
        Intimate,
        Personal,
        Public
    }
    
    [Serializable]
    public struct ZoneDetail
    {
        public ZoneType Type;
        public float Radius;
    }
}