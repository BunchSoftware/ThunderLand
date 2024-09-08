using GrapeNetwork.UnitWorld.Type;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeNetwork.UnitWorld.Object
{
    public class GameObject
    {
        public string nameObject = "GameObject";
        public string tag = "None";
        public bool isActive = true;
        public TypeActiveGameObject typeActiveGameObject = TypeActiveGameObject.Active;
        public Transform transform = new Transform();
    }
}
