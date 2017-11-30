

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
        protected string _essental;
        // position
        protected Vector2 _position;
        // rotation in degrees
        protected int _rotation;
        public GameObject(float x,float y,string essental)
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
        // properties
        public ulong ID
        {
            get { return _id; }
            private set { _id = value; }
        }
        public string Essental()
        {
            return _essental;
        }
        public void SetPosition(Vector2 value)
        {
            _position = value;
        }
        public Vector2 GetPosition() { return _position; }
        public void SetRotation(int degrees) { _rotation = degrees; }
    }
}
