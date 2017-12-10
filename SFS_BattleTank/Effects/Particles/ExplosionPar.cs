using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SFS_BattleTank.Effects.ParticleSys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFS_BattleTank.Effects.Particles
{
    public class ExplosionPar : ParticleSystem
    {
        public ExplosionPar(Rectangle boundingBox,float activeTime)
            : base(boundingBox,activeTime)
        {
            _minSize = 30;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
        public override void Draw(SpriteBatch sp)
        {
            base.Draw(sp);
        }
        public override void LoadContents(ContentManager contents)
        {
            for (int i = 0; i + 0 < 5; i++)
                _textures.Add(contents.Load<Texture2D>(@"particles\explosion\" + i.ToString()));

            base.LoadContents(contents);
        }
        public override void AddPar()
        {
            if (_particles.Count < 50)
            {
                int count = 0;
                int minX = _boundingBox.X;
                int maxX = _boundingBox.X + _boundingBox.Width;
                int minY = _boundingBox.Y;
                int maxY = _boundingBox.Y + _boundingBox.Height;
                while (count < 15)
                {
                    _particles.Add(new Particle(_textures[_rd.RandomInt(0, _textures.Count - 1)],
                                                 new Microsoft.Xna.Framework.Vector2(_rd.RandomInt(minX, maxX), _rd.RandomInt(minY, maxY)),
                                                 _rd.RandomInt(5, 10),
                                                 (float)(_rd.RandomDouble() + 1.50d),
                                                 _rd.RandomInt(-30, 330),
                                                 (float)_rd.RandomDouble(),
                                                 1.0f,
                                                 _minSize,
                                                 1.0f,
                                                 _rd.RandomInt(50,150)));
                    count++;
                }
            }
            base.AddPar();
        }
    }
}
