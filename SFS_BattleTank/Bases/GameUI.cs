
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace SFS_BattleTank.Bases
{
    public class GameUI
    {
        protected Vector2 _position;
        protected Rectangle _bounding;
        protected string _type;

        public GameUI(string type, Vector2 position, Rectangle bounding)
        {
            _type = type;
            _position = position;
            _bounding = bounding;
        }

        public virtual void Update(float deltaTime) { }
        public virtual void Init() { }
        public virtual void LoadContents(ContentManager contents) { }
        public virtual void Draw(SpriteBatch sp) { }
        
        // properties
        public Vector2 GetPosition() { return _position; }
        public void SetPosition(Vector2 value) { _position = value; }
        public string GetType() { return _type; }
        public Rectangle GetBoundingBox() { return _bounding; }

    }
}
