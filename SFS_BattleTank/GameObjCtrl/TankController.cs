﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
using SFS_BattleTank.GameObjects;
using SFS_BattleTank.InputControl;
using Sfs2X;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests;
using Sfs2X.Requests.MMO;
using System.Collections.Generic;

namespace SFS_BattleTank.GameObjCtrl
{
    public class TankController : Controller
    {
        protected const int TANK_SPEED = 100;
        protected Dictionary<int, GameObject> _tanks;
        protected float _totalFireTime;
        protected float _delayFire;

        protected int _lastXDir;
        protected int _lastYDir;

        public TankController(ContentManager contents)
            : base(contents)
        {
            _tanks = new Dictionary<int, GameObject>();
            _totalFireTime = 0;
            _delayFire = 0.75f;
            _lastXDir = 1;
            _lastYDir = 0;
        }

        public override void Add(User user,SFSObject data)
        {
            if (data.ContainsKey(Consts.X) && data.ContainsKey(Consts.Y))
            {
                if (!_tanks.ContainsKey(user.Id))
                {
                    _tanks.Add(user.Id, new Tank(user.Id, (float)data.GetDouble(Consts.X),(float)data.GetDouble(Consts.Y)));
                    _tanks[user.Id].LoadContents(_contents);
                }
            }
            base.Add(user,data);
        }
        public override void Remove(User user)
        {
            if (!_tanks.ContainsKey(user.Id))
            {
                return;
            }
            _tanks.Remove(user.Id);
            base.Remove(user);
        }
        public override void GetDirection(out int x, out int y)
        {
            KeyboardState state = Keyboard.GetState();
            Keys[] keys = state.GetPressedKeys();

            Keys dir = Keys.None;
            foreach (Keys i in keys)
            {
                if (i == Keys.Left || i == Keys.Right || i == Keys.Up || i == Keys.Down)
                {
                    dir = i;
                    break;
                }
            }
            switch (dir)
            {
                case Keys.Left:
                    {
                        x = -1; y = 0;
                        break;
                    }
                case Keys.Right:
                    {
                        x = 1; y = 0;
                        break;
                    }
                case Keys.Up:
                    {
                        x = 0; y = -1;
                        break;
                    }
                case Keys.Down:
                    {
                        x = 0; y = 1;
                        break;
                    }
                default:
                    {
                        x = 0; y = 0;
                        break;
                    }
            }
            return;
            base.GetDirection(out x, out y);
        }
        public override void UpdateData(User user, SFSObject data)
        {
            int me = _network.GetInstance().MySelf.Id;
            if (user.Id != me)
            {
                if (_tanks.ContainsKey(user.Id))
                {
                    if (data.ContainsKey(Consts.X) && data.ContainsKey(Consts.Y) && data.ContainsKey(Consts.ROTATION))
                    {
                        _tanks[user.Id].SetVariable((float)data.GetDouble(Consts.X),
                                                                  (float)data.GetDouble(Consts.Y),
                                                                  (int)data.GetDouble(Consts.ROTATION));
                    }
                }
            }
            base.UpdateData(user,data);
        }
        public override void Behaviour(string cmd, int id, SFSObject data)
        {
            base.Behaviour(cmd, id, data);
        }
        public override void Update(float deltaTime)
        {
            int x, y;
            this.GetDirection(out x, out y);
            if (x != 0 || y != 0)
                Move(deltaTime, x, y);
            if(CheckFire(deltaTime))
            {
                Fire(_lastXDir, _lastYDir);
            }
            SetDir(x, y);
            base.Update(deltaTime);
        }
        public override Dictionary<int, GameObject> GetAllGameObject()
        {
            return _tanks;
        }
        public override void Init()
        {
            GetMyId();
            base.Init();
        }
        // myseft active
        public void Move(float deltaTime, int xDir, int yDir)
        {
            float vx, vy;
            vx = vy = 0;
            if (xDir != 0) vx = xDir * deltaTime * TANK_SPEED;
            if (yDir != 0) vy = yDir * deltaTime * TANK_SPEED;

            if (_tanks.Count <= 0) return;
            Vector2 curPosition = _tanks[_myId].GetPosition();
            SmartFox sfs = _network.GetInstance();
            // set mysefl rotation
            #region
            if (sfs.MySelf != null)
            {
                _tanks[_myId].SetPosition(new Vector2(curPosition.X + vx, curPosition.Y + vy));

                if (xDir != 0)
                {
                    if (xDir == 1)
                        _tanks[_myId].SetRotation(0);
                    else _tanks[_myId].SetRotation(180);
                }
                if (yDir != 0)
                {
                    if (yDir == 1)
                        _tanks[_myId].SetRotation(90);
                    else _tanks[_myId].SetRotation(-90);
                }
            }
            #endregion
            else
            {
                _tanks[_myId].SetPosition(new Vector2(curPosition.X + vx, curPosition.Y + vy));
            }

            // send new update to server
            if (_network.GetInstance().IsConnected)
            {
                curPosition = _tanks[_myId].GetPosition();
                SFSObject data = new SFSObject();
                data.PutDouble(Consts.X, curPosition.X);
                data.PutDouble(Consts.Y, curPosition.Y);
                //data.PutDouble(Consts.XDIR, xDir);
                //data.PutDouble(Consts.YDIR, yDir);
                data.PutDouble(Consts.VX, vx);
                data.PutDouble(Consts.VY, vy);
                data.PutDouble(Consts.ROTATION, _tanks[_myId].GetRotation());
                data.PutUtfString(Consts.TYPE, Consts.TYPE_TANK);

                sfs.Send(new ExtensionRequest(Consts.CRQ_MOVE, data,_network.GetInstance().LastJoinedRoom));
            }
        }
        public bool CheckFire(float deltaTime)
        {
            if (Input.IsKeyDown(Keys.Space))
            {
                if (_delayFire <= _totalFireTime)
                {
                    _totalFireTime = 0;
                    return true;
                } 
            }
            _totalFireTime += deltaTime;
            return false;
        }
        public void Fire(int xDir,int yDir)
        {
            SFSObject data = new SFSObject();
            data.PutInt(Consts.XDIR, xDir);
            data.PutInt(Consts.YDIR, yDir);
            _network.GetInstance().Send( new ExtensionRequest(Consts.CRQ_FIRE, data));
        }
        protected void SetDir(int x,int y)
        {
            if(x != 0 || y != 0)
            {
                _lastXDir = x;
                _lastYDir = y;
            }
        }
    }
}
