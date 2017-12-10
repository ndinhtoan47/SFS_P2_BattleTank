

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
namespace SFS_BattleTank.Sounds
{

    public class SEffect
    {
        protected SoundEffect _sound;
        protected SoundEffectInstance _instance;
        protected bool _isMute;
        public SEffect()
        {
            _sound = null;
            _instance = null;
            _isMute = false;
        }
        public void Play(float volume = 1.0f, float pitch = 0.0f, float pan = 0.0f)
        {
            _instance.Volume = volume;
            _instance.Pitch = pitch;
            _instance.Pan = pan;

            _instance.Play();
        }
        public void LoadContents(ContentManager content, string path)
        {
            _sound = content.Load<SoundEffect>(path);
            _instance = _sound.CreateInstance();
            Init();
        }

        public void Re_CreateInstance()
        {
            _instance = _sound.CreateInstance();
        }
        public void Dispose()
        {
            Stop(true);
            _instance.Dispose();
            _sound.Dispose();
        }
        public void Stop(bool value = true)
        {
            _instance.Stop(value);
        }
        public void Pause()
        {
            _instance.Pause();
        }
        public void Resume()
        {
            _instance.Resume();
        }
        public void Mute(bool value = true)
        {
            _instance.Volume = 0.0f;
            _isMute = value;
        }
        public void Repeat(bool value = false)
        {
            _instance.IsLooped = value;
        }
        public bool IsMute() { return _isMute; }

        protected void Init()
        {
            this.Repeat(false);
        }
    }
}
