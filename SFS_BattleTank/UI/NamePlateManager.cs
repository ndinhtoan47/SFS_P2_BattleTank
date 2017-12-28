

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SFS_BattleTank.Constants;
using Sfs2X.Entities;
using System.Collections.Generic;
namespace SFS_BattleTank.UI
{
    public class NamePlateManager
    {
        private Dictionary<int, NamePlate> _namePlates;
        private ContentManager _contents;

        private int _col;
        private int _row;
        public NamePlateManager()
        {
            _namePlates = new Dictionary<int, NamePlate>();            
        }

        public void Init()
        {
            _col = 1;
            _row = 0;
        }
        public void LoadContents(ContentManager contents)
        {
            _contents = contents;
        }
        public void Add(User user, bool isOnwer)
        {
            if(user != null && !_namePlates.ContainsKey(user.Id))
            {
                _namePlates.Add(user.Id, new NamePlate(Vector2.Zero, Rectangle.Empty, user.Name, isOnwer));
                InitUser(user);
            }
        }
        public void Remove(User user,bool isOnwer)
        {
            if (user != null && _namePlates.ContainsKey(user.Id))
            {
                _namePlates.Remove(user.Id);
            }
        }
        public void InitUser(User user)
        {
            if(user != null && _namePlates.ContainsKey(user.Id))
            {
                _namePlates[user.Id].LoadContents(_contents);
                _namePlates[user.Id].SetBoundingBox(new Rectangle(0, 0, (int)(_namePlates[user.Id].GetSprite().Width * 0.25f), (int)(_namePlates[user.Id].GetSprite().Height * 0.25f)));
                _namePlates[user.Id].SetPosition(new Vector2(Consts.VIEWPORT_WIDTH * 0.25f * _col, _namePlates[user.Id].GetBoundingBox().Height * _row * 0.75f));
                _namePlates[user.Id].Init();

                if (_col % 2 == 0) _row++;
                if (_col == 1) _col = 2;
                else _col = 1;
                
            }
        }
        public void Draw(SpriteBatch sp)
        {
            foreach (NamePlate np in _namePlates.Values)
                np.Draw(sp);
        }
    }
}
