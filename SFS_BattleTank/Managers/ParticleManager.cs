using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SFS_BattleTank.Constants;
using SFS_BattleTank.Effects.Particles;
using SFS_BattleTank.Effects.ParticleSys;
using System.Collections.Generic;


namespace SFS_BattleTank.Managers
{
    public class ParticleManager
    {
        protected List<ParticleSystem> _particles;
        protected ContentManager _contents;

        public ParticleManager()
        {
            Init();
        }
        protected void Init()
        {
            _particles = new List<ParticleSystem>();
        }
        public void Remove()
        {
            for(int i = 0; i < _particles.Count ; i++)
            {
                if(_particles[i].ParticleNum() <= 0)
                {
                    _particles.Remove(_particles[i]);
                    i--;
                }
            }
        }
        public void LoadContents(ContentManager contents)
        {
            _contents = contents;
        }
        public void Update(float deltaTime)
        {
            for (int i = 0; i < _particles.Count; i++)
            {
                _particles[i].Update(deltaTime);
            }
            Remove();
        }
        public void Add(string type, Rectangle boundingBox)
        {
            if(type == Consts.TYPE_PAR_EXPLOSION)
            {
                _particles.Add(new ExplosionPar(boundingBox,1.0f));
            }
            if(type == Consts.TYPE_PAR_FIRE)
            {
                _particles.Add(new FirePar(boundingBox,5.0f));
            }
            if(_particles.Count > 0)
            {
                _particles[_particles.Count - 1].LoadContents(_contents);
            }
        }
        public void Draw(SpriteBatch sp)
        {
            for (int i = 0; i < _particles.Count; i++)
            {
                _particles[i].Draw(sp);
            }
        }
        
    }
}
