
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
using SFS_BattleTank.GameObjects;
using SFS_BattleTank.InputControl;
using SFS_BattleTank.Managers;
using SFS_BattleTank.Sounds;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using System;
using System.Collections.Generic;
namespace SFS_BattleTank.GameObjCtrl
{
    public class BulletController : Controller
    {
        protected Dictionary<int, GameObject> _bullets;

        protected const string SOUND_FIRE = @"sounds\s_fire";
        protected SEffect _s_fire;
        public BulletController(ContentManager contents)
            : base(contents)
        {
            _bullets = new Dictionary<int, GameObject>();

            _s_fire = new SEffect();
            _s_fire.LoadContents(contents, SOUND_FIRE);
        }

        public override void Add(Sfs2X.Entities.User user, SFSObject data)
        {
            if (_bullets != null)
            {
                if (data.ContainsKey(Consts.X) && (data.ContainsKey(Consts.Y) && data.ContainsKey(Consts.GO_ID)))
                {
                    int objectId = (int)data.GetDouble(Consts.GO_ID);
                    if (!_bullets.ContainsKey(objectId))
                    {
                        _bullets.Add((int)data.GetDouble(Consts.GO_ID), new Bullet((float)data.GetDouble(Consts.X),
                                                        (float)data.GetDouble(Consts.Y),
                                                        (ulong)objectId));
                        _bullets[objectId].LoadContents(_contents);
                        _s_fire.Play();
                    }
                }
            }
            base.Add(user, data);
        }
        public override void Remove(User user, SFSObject data)
        {
            if (!data.ContainsKey(Consts.GO_ID)) return;
            else
            {
                int objectId = (int)data.GetDouble(Consts.GO_ID);
                if (_bullets.ContainsKey(objectId))
                    _bullets.Remove(objectId);
            }
            base.Remove(user, data);
        }
        public override void Init()
        {
            base.Init();
        }
        public override void UpdateData(User user, SFSObject data)
        {
            int go_id = (data.ContainsKey(Consts.GO_ID)) ? (int)data.GetDouble(Consts.GO_ID) : -1;
            if (!(data.ContainsKey(Consts.X) && data.ContainsKey(Consts.Y))) return;
            if (go_id != -1)
            {
                if (_bullets.ContainsKey(go_id))
                {
                    _bullets[go_id].SetPosition(new Vector2((float)data.GetDouble(Consts.X), (float)data.GetDouble(Consts.Y)));
                }
            }
            base.UpdateData(user, data);
        }
        public override void Behaviour(string cmd, int id, SFSObject data)
        {
            base.Behaviour(cmd, id, data);
        }
        public override Dictionary<int, GameObject> GetAllGameObject()
        {
            return _bullets;
            return base.GetAllGameObject();
        }


        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
    }
}
