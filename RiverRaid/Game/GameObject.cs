using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiverRaid.RaidGame
{
    public enum GameObjectType
    {
        BACKGROUND,
        BRIDGE,
        SHIP,
        HELICOPTER,
        ENEMY_PLANE,
        SHOT,
        FUEL,

    }

    class GameObject
    {
        public GameObjectType type;
        public float x, y;
        public float width, height;
        public Rectangle hitbox
        {
            get
            {
                return new Rectangle((int)x, (int)y, (int)width, (int)height);
            }
        }

        public GameObject(GameObjectType type, float x, float y, float width, float height)
        {
            this.type = type;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
    }
}
