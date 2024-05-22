using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lukomor.AlgebraJump.Runner
{
    public class Zone : MonoBehaviour
    {
        public ZoneType ZoneType;
    }
    
    public enum ZoneType
    {
        Default = 0,
        Damage = 1,
        DoubleJump = 2,
        GravityFlip = 3
    }
}