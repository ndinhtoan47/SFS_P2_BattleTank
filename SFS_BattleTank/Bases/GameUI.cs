
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SFS_BattleTank.Constants;
using SFS_BattleTank.InputControl;
namespace SFS_BattleTank.Bases
{
    public class GameUI
    {
        protected Vector2 _position;
        protected Rectangle _bounding;
        protected string _type;
        protected bool _isEnable;

        public GameUI(string type, Vector2 position,Rectangle bounding)
        {
            _type = type;
            _position = position;
            _bounding = bounding;
            _isEnable = true;
        }

        public virtual void Update(float deltaTime) { }
        public virtual void Init() { }
        public virtual void LoadContents(ContentManager contents) { }
        public virtual void Draw(SpriteBatch sp) { }
        public virtual void CMD(string cmd) 
        {
            if(cmd == Consts.UI_CMD_DISABLE)
            {
                _isEnable = false;
                return;
            }
            if(cmd == Consts.UI_CMD_ENABLE)
            {
                _isEnable = true;
                return;
            }
        }
        public virtual void ChangeBackground(string name) { }
        protected virtual void InitBoundingBox(float textScale) { }
        
        // helper
        protected bool CheckInsideUI(Vector2 position, Rectangle boundingBox)
        {
            Vector2 mousePosition = Input.GetMousePosition();
            if (mousePosition.X >= position.X &&
                mousePosition.X <= position.X + boundingBox.Width &&
                mousePosition.Y >= position.Y &&
                mousePosition.Y <= position.Y + boundingBox.Height)
            {
                return true;
            }
            return false;
        }
        public void CenterAlignment(Rectangle box)
        {
            int x = box.X + (box.Width - _bounding.Width) / 2;
            int y = box.Y + (box.Height - _bounding.Height) / 2;
            _position.X = x;
            _position.Y = y;
        }
        public bool ClickedInsideUI()
        {
            if (CheckInsideUI(_position, _bounding))
            {
                if (Input.Clicked(Consts.MOUSEBUTTON_LEFT))
                {
                    return true;
                }
            }
            return false;
        }
        // properties
        public Vector2 GetPosition() { return _position; }
        public void SetPosition(Vector2 value) { _position = value; }
        public string GetUIType() { return _type; }
        public Rectangle GetBoundingBox() { return _bounding; }
        public bool IsEnable() { return _isEnable; }

    }
}
