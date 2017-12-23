

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
namespace SFS_BattleTank.Sounds
{
    public class SBackground
    {
        protected Song _song;
        protected bool _isMute;
        public SBackground()
        {
            _song = null;
            _isMute = false;
        }

        public void Play(TimeSpan startPos, float volume = 1.0f)
        {
            MediaPlayer.Volume = volume;
            MediaPlayer.Play(_song, startPos);
        }
        public void LoadContents(ContentManager content, string path)
        {
            _song = content.Load<Song>(path);
            Init();
        }

        public void Dispose()
        {
            if (_song != null)
            {
                _song.Dispose();
                _song = null;
            }
        }
        public void Stop()
        {
            MediaPlayer.Stop();
        }
        public void Pause()
        {
            MediaPlayer.Pause();
        }
        public void Resume()
        {
            MediaPlayer.Resume();
        }
        public void Mute(bool value = true)
        {
            MediaPlayer.IsMuted = value;
            _isMute = value;
        }
        public void Repeat(bool value = false)
        {
            MediaPlayer.IsRepeating = value;
        }
        public bool IsMute() { return _isMute; }

        protected void Init()
        {
            this.Repeat(true);
        }
    }
}
