

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
namespace SFS_BattleTank.UI
{
    public class NamePlate :GameUI
    {
        protected string _userName;
        protected SpriteFont _font;
        protected Texture2D _plate;
        protected bool _isOwnerRoom;

        public NamePlate(Vector2 position,Rectangle bounding,string userName,bool isOwnerRoom):
            base(Consts.UI_NAME_PLATE,position,bounding)
        {
            _isEnable = false;
            _userName = userName;
            _isOwnerRoom = isOwnerRoom;
        }

        public override void Update(float deltaTime) { base.Update(deltaTime); }
        public override void Init() { base.Init(); }
        public override void LoadContents(ContentManager contents) 
        {
            if (_isOwnerRoom) _plate = contents.Load<Texture2D>(Consts.UIS_OWNER_DISPLAY);
            else _plate = contents.Load<Texture2D>(Consts.UIS_PLAYER_DISPLAY);
            _font = contents.Load<SpriteFont>("font");
            base.LoadContents(contents); 
        }
        public override void Draw(SpriteBatch sp)
        {
            if(_plate != null)
            {
                sp.Draw(_plate, new Rectangle((int)_position.X, (int)_position.Y, _bounding.Width, _bounding.Height), Color.White);
            }
            base.Draw(sp); 
        }
        public override void CMD(string cmd) 
        {
            base.CMD(cmd);
        }
        public override void ChangeBackground(string name) { base.ChangeBackground(name); }
        protected override void InitBoundingBox(float textScale) { base.InitBoundingBox(textScale); }
    }
}
