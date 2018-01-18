
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
using SFS_BattleTank.Sounds;

namespace SFS_BattleTank.GameObjects
{
    public class Bullet : GameObject
    {
        protected Texture2D _sprite;
        protected const string BULLET_PATH = "gameBullet";
        protected Rectangle _desRect;
        protected Vector2 _origin;
        protected SEffect _s_fire;
        protected const string SOUND_FIRE = @"sounds\s_fire";

        public Bullet(float x, float y,ulong id)
            : base(x, y, Consts.ES_BULLET)
        {
            _desRect = new Rectangle(0,0,10,10);
            _origin = new Vector2(5, 5);
            _id = id;
            _s_fire = new SEffect();
        }

        public override bool Init()
        {
            return base.Init();
        }
        public override void LoadContents(ContentManager contents)
        {
            _sprite = contents.Load<Texture2D>(BULLET_PATH);
            _s_fire.LoadContents(contents, SOUND_FIRE);
            _s_fire.Play();
            base.LoadContents(contents);
        }
        public override void Draw(SpriteBatch sp)
        {
            if (_sprite != null)
            {
                sp.Draw(
                    texture: _sprite,
                    origin: _origin,
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
        public override Rectangle GetBoundingBox()
        {
            return _desRect;
            return base.GetBoundingBox();
        }
    }
}
