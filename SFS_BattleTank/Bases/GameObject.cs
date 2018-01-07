﻿

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace SFS_BattleTank.Bases
{
    public class GameObject
    {
        // only one id with one object
        protected ulong _id;
        // essental's obj
        protected int _essental;
        // position
        protected Vector2 _position;
        // rotation in degrees
        protected int _rotation;
        
        
        public GameObject(float x,float y,int essental)
        {
            _essental = essental;
            _position = new Vector2(x,y);
            _rotation = 0;
        }

        public virtual bool Init() { return true; }
        public virtual void LoadContents(ContentManager contents) { }
        public virtual void Draw(SpriteBatch sp) { }
        public virtual void Update(float deltaTime) { }
        public virtual void Behavior(string cmd) { }
        public virtual string Respose(string cmd) { return ""; }
        public virtual Rectangle GetBoundingBox() { return new Rectangle(); }
        // properties
        public ulong ID
        {
            get { return _id; }
            private set { _id = value; }
        }
        public int Essental()
        {
            return _essental;
        }
        public void SetPosition(Vector2 value)
        {
            _position = value;
        }
        public Vector2 GetPosition() { return _position; }
        public void SetRotation(int degrees) { _rotation = degrees; }
        public int GetRotation() { return _rotation; }
        public void SetVariable(float x,float y,int rotation)
        {
            _position.X = x;
            _position.Y = y;
            _rotation = rotation;
        }
    }
}
