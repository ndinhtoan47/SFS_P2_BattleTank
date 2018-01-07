﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
using SFS_BattleTank.GameObjects;
using SFS_BattleTank.GameScenes;
using SFS_BattleTank.InputControl;
using Sfs2X;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests;
using Sfs2X.Requests.MMO;
using System.Collections.Generic;
using System.Diagnostics;

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
        protected int _lastRotation;

        public TankController(ContentManager contents)
            : base(contents)
        {
            _tanks = new Dictionary<int, GameObject>();
            _delayFire = 0.888f;
            _totalFireTime = _delayFire;
            _lastXDir = 1;
            _lastYDir = 0;
            _lastRotation = 0;
        }

        public override void Add(User user, IMMOItem item)
        {
            if (user != null)
            {
                if (_tanks.ContainsKey(user.Id)) return;
                if (user.ContainsVariable(Consts.X) &&
                    user.ContainsVariable(Consts.X) &&
                    user.ContainsVariable(Consts.ROTATION) &&
                    user.ContainsVariable(Consts.ALIVE))
                {
                    _tanks.Add(user.Id, new Tank(user.Id,
                        (float)user.GetVariable(Consts.X).GetIntValue(),
                        (float)user.GetVariable(Consts.Y).GetIntValue()));
                    _tanks[user.Id].SetRotation((int)user.GetVariable(Consts.ROTATION).GetIntValue());
                    _tanks[user.Id].LoadContents(_contents);
                }
            }
            base.Add(user, item);
        }
        public override void Remove(User user, IMMOItem item)
        {
            if (user != null)
            {
                if (!_tanks.ContainsKey(user.Id))
                    return;
                _tanks.Remove(user.Id);
            }
            base.Remove(user, item);
        }
        public override void UpdateData(User user, List<string> changedVars, IMMOItem item)
        {
            if (user != null)
            {
                if (_tanks.ContainsKey(user.Id))
                {
                    List<UserVariable> vars = user.GetVariables();
                    if (changedVars.Contains(Consts.X) ||
                        changedVars.Contains(Consts.Y) ||
                        changedVars.Contains(Consts.ROTATION) ||
                        changedVars.Contains(Consts.ALIVE) ||
                        changedVars.Contains(Consts.KILL) ||
                        changedVars.Contains(Consts.DEATH))
                    {
                        Tank tank = (Tank)_tanks[user.Id];
                        if (changedVars.Contains(Consts.X) ||
                        changedVars.Contains(Consts.Y) ||
                        changedVars.Contains(Consts.ROTATION)) // position changed
                        {
                            float x = (float)user.GetVariable(Consts.X).GetIntValue();
                            float y = (float)user.GetVariable(Consts.Y).GetIntValue();
                            int r = (int)user.GetVariable(Consts.ROTATION).GetIntValue();
                            tank.SetVariable(x, y, r);
                        }
                        if (changedVars.Contains(Consts.ALIVE)) // state changed
                        {
                            bool a = (bool)user.GetVariable(Consts.ALIVE).GetBoolValue();
                            if (!a)
                            {
                                tank.Death();
                            }
                            else tank.ReGeneration();
                        }
                        // score changed
                        if ((user.Id == _mySelf) && (changedVars.Contains(Consts.KILL) || changedVars.Contains(Consts.DEATH)))
                        {
                            if (user.ContainsVariable(Consts.KILL))
                            {
                                int kill = (int)user.GetVariable(Consts.KILL).GetIntValue();
                                tank.SetKill(kill);
                                PlayScene._killCount.SetInfo(kill.ToString());
                            }
                            if (user.ContainsVariable(Consts.DEATH))
                            {
                                int death = (int)user.GetVariable(Consts.DEATH).GetIntValue();
                                tank.SetDeath(death);
                                PlayScene._deathCount.SetInfo(death.ToString());
                            }
                        }
                    }
                }
            }
            base.UpdateData(user, changedVars, item);
        }

        public override void Behaviour(string cmd, int id, SFSObject data)
        {
            base.Behaviour(cmd, id, data);
        }
        public override void Update(float deltaTime)
        {
            if (_tanks.ContainsKey(_mySelf))
            {
                Tank me = (Tank)_tanks[_mySelf];
                if (me.IsAlive())
                {
                    int rotation;
                    Keys dir = this.GetDirection(out rotation);
                    if (dir != Keys.None)
                        this.Move(deltaTime, rotation);
                    if (CheckFire(deltaTime))
                    {
                        Fire();
                    }
                }
            }
            base.Update(deltaTime);
        }
        public override Dictionary<int, GameObject> GetAllGameObject()
        {
            return _tanks;
        }
        public override void Init()
        {
            _mySelf = GetMySefl();
            base.Init();
        }

        public Keys GetDirection(out int rotation)
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
                        rotation = 180;
                        break;
                    }
                case Keys.Right:
                    {
                        rotation = 0;
                        break;
                    }
                case Keys.Up:
                    {
                        rotation = -90;
                        break;
                    }
                case Keys.Down:
                    {
                        rotation = 90;
                        break;
                    }
                default:
                    {
                        rotation = _lastRotation;
                        break;
                    }
            }
            return dir;
        }
        // myseft active
        protected void Move(float deltaTime, int rotation)
        {
            if (_tanks.Count <= 0 || !_tanks.ContainsKey(_mySelf)) return;
            SmartFox sfs = _network.GetInstance();

            // get velocity
            //float vx, vy;
            //vx = vy = 0;
            //if (xDir != 0) vx = xDir * deltaTime * TANK_SPEED;
            //if (yDir != 0) vy = yDir * deltaTime * TANK_SPEED;
            // calculate current position
            //Vector2 curPosition = _tanks[_mySelf].GetPosition();
            //curPosition.X += vx;
            //curPosition.Y += vy;
            //int rotation = 0;
            // set mysefl rotation
            //if (xDir != 0)
            //{
            //    if (xDir == 1)
            //        rotation = 0;
            //    else rotation = 180;
            //}
            //if (yDir != 0)
            //{
            //    if (yDir == 1)
            //        rotation = 90;
            //    else rotation = -90;
            //}
            // send new update to server    
            if (_network.GetInstance().IsConnected)
            {
                List<UserVariable> vars = new List<UserVariable>();
                //vars.Add(new SFSUserVariable(Consts.X, (double)curPosition.X));
                //vars.Add(new SFSUserVariable(Consts.Y, (double)curPosition.Y));
                vars.Add(new SFSUserVariable(Consts.ROTATION, (int)rotation));
                sfs.Send(new SetUserVariablesRequest(vars));
            }
            _lastRotation = rotation;
        }
        protected bool CheckFire(float deltaTime)
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
        protected void Fire()
        {
            SmartFox sfs = _network.GetInstance();
            if (sfs != null)
            {
                SFSObject data = new SFSObject();
                sfs.Send(new ExtensionRequest(Consts.CRQ_FIRE, data, _network.GetCurretRoom()));
            }
        }

    }
}
