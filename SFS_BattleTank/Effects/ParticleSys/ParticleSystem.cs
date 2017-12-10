
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
namespace SFS_BattleTank.Effects.ParticleSys
{
    public class ParticleSystem
    {
        protected RandomMaxMin _rd;
        protected Rectangle _boundingBox;
        protected List<Particle> _particles;
        protected List<Texture2D> _textures;
        protected bool _active;
        protected int _minSize;
        protected float _activeTime;

        public ParticleSystem(Rectangle boundingBox, float activeTime)
        {
            _rd = new RandomMaxMin();
            _boundingBox = boundingBox;
            _activeTime = activeTime;
            _particles = new List<Particle>();
            _textures = new List<Texture2D>();
            _minSize = 1;
            _active = true;
        }

        public virtual void Update(float deltaTime)
        {
            for (int i = 0; i < _particles.Count; i++)
            {
                _particles[i].Update(deltaTime);
            }
            if (_active) AddPar();
            CheckActiveTime(deltaTime);
            RemoveOutTimeParticles();
        }
        public virtual void Draw(SpriteBatch sp)
        {
            for (int i = 0; i < _particles.Count; i++)
            {
                _particles[i].Draw(sp);
            }
        }
        public virtual void LoadContents(ContentManager contents) { }
        public virtual void AddPar() { }
        protected void RemoveOutTimeParticles()
        {
            for (int i = 0; i < _particles.Count; i++)
            {
                if (_particles[i].TOTALTIMELIFE >= _particles[i].LIFETIME)
                {
                    _particles.Remove(_particles[i]);
                    i--;
                }
            }
        }
        public void SetActive(bool value)
        {
            _active = value;
        }
        public int ParticleNum() { return _particles.Count; }
        protected void CheckActiveTime(float deltaTime)
        {
            if (_active)
            {
                _activeTime -= deltaTime;
                if(_activeTime <= 0)
                {
                    _active = false;
                }
            }
        }
    }
}
