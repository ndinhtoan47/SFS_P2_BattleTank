
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;

namespace SFS_BattleTank.GameObjects
{
    public class Bullet : GameObject
    {
        protected Texture2D _sprite;
        protected const string BULLET_PATH = "gameBullet";
        protected Rectangle _desRect;

        public Bullet(float x, float y,ulong id)
            : base(x, y, Consts.ES_BULLET)
        {
            _desRect = new Rectangle(0,0,10,10);
            _id = id;
        }

        public override bool Init()
        {
            return base.Init();
        }
        public override void LoadContents(ContentManager contents)
        {
            _sprite = contents.Load<Texture2D>(BULLET_PATH);
            base.LoadContents(contents);
        }
        public override void Draw(SpriteBatch sp)
        {
            if (_sprite != null)
            {
                sp.Draw(
                    texture: _sprite,
                    destinationRectangle: new Rectangle((int)_position.X, (int)_position.Y, (int)_desRect.Width, (int)_desRect.Height),
                    color: Color.Green);
            }
            base.Draw(sp);
        }
        public override string Respose(string cmd)
        {
            return base.Respose(cmd);
        }
        public override void Behavior(string cmd)
        {
            base.Behavior(cmd);
        }
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
    }
}
