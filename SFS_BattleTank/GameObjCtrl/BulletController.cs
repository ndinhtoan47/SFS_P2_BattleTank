﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
using SFS_BattleTank.GameObjects;
using SFS_BattleTank.GameScenes;
using SFS_BattleTank.InputControl;
using SFS_BattleTank.Managers;
using SFS_BattleTank.Sounds;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace SFS_BattleTank.GameObjCtrl
{
    public class BulletController : Controller
    {
        protected Dictionary<int, GameObject> _bullets;

        
        
        protected MMORoom _room;
        public BulletController(ContentManager contents)
            : base(contents)
        {
            _bullets = new Dictionary<int, GameObject>();
            _room = (MMORoom)_network.GetCurretRoom();
        }

        public override void Add(User user, IMMOItem item)
        {
            if (item != null && !_bullets.ContainsKey(item.Id))
            {
                // check type var contain inside item or not
                if (item.ContainsVariable(Consts.TYPE))
                {   // check type is bullet
                    if (item.GetVariable(Consts.TYPE).GetIntValue() == Consts.ES_BULLET)
                        if (item.ContainsVariable(Consts.X) && item.ContainsVariable(Consts.Y))
                        {
                            _bullets.Add(item.Id, new Bullet((float)item.GetVariable(Consts.X).GetIntValue(),
                                                                (float)item.GetVariable(Consts.Y).GetIntValue(),
                                                                (ulong)item.Id));
                            _bullets[item.Id].LoadContents(_contents);                           
                            Debug.WriteLine("Added bullet " + item.Id);
                        }
                }
            }
            base.Add(user, item);
        }
        public override void Remove(User user, IMMOItem item)
        {
            if (item != null && _bullets.ContainsKey(item.Id))
            {
                GameObject bullet = (GameObject)_bullets[item.Id];
                Rectangle bounding = new Rectangle((int)bullet.GetPosition().X, (int)bullet.GetPosition().Y,
                    bullet.GetBoundingBox().Width, bullet.GetBoundingBox().Height);
                PlayScene._parManager.Add(Consts.TYPE_PAR_EXPLOSION, bounding);
                _bullets.Remove(item.Id);
                Debug.WriteLine("Removed bullets " + item.Id + "remain " + _bullets.Count + " bullet");
            }
            base.Remove(user, item);
        }
        public override void Init()
        {
            base.Init();
        }
        public override void UpdateData(User user, List<string> changedVars, IMMOItem item)
        {
            if (item != null && _bullets.ContainsKey(item.Id))
            {
                float x = 0; float y = 0;
                if (item.ContainsVariable(Consts.X)) x = (float)item.GetVariable(Consts.X).GetIntValue();
                if (item.ContainsVariable(Consts.Y)) y = (float)item.GetVariable(Consts.Y).GetIntValue();
                _bullets[item.Id].SetPosition(new Vector2(x, y));
            }
            base.UpdateData(user, changedVars, item);
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
            List<IMMOItem> bullets = _room.GetMMOItems();
            if (bullets.Count > 0)
                foreach (IMMOItem bullet in bullets)
                {
                    this.UpdateData(null, null, bullet);
                }
            base.Update(deltaTime);
        }
    }
}
