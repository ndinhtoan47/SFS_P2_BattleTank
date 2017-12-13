

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Camera;
using SFS_BattleTank.Constants;
using SFS_BattleTank.GameObjCtrl;
using SFS_BattleTank.InputControl;
using SFS_BattleTank.Managers;
using SFS_BattleTank.Maps;
using SFS_BattleTank.UI;
using Sfs2X;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using System.Collections.Generic;

namespace SFS_BattleTank.GameScenes
{
    public class PlayScene : Scene
    {
        protected Camera2D _camera;
        protected ParticleManager _parManager;
        protected Texture2D _background;
        protected Map1 map1;
        public PlayScene(ContentManager contents)
            : base(Consts.SCENE_PLAY, contents)
        {
            _parManager = new ParticleManager();
            _camera = new Camera2D(1008, 1008);
            map1 = new Map1();
        }

        public override bool Init()
        {
            _network.AddController(Consts.CTRL_TANK, new TankController(_contents));
            _network.AddController(Consts.CTRL_BULLET, new BulletController(_contents));
            _network.AddController(Consts.CTRL_ITEM, new ItemController(_contents));
            map1.Init();
            return base.Init();
        }
        public override bool LoadContents()
        {
            _parManager.LoadContents(_contents);
            _background = _contents.Load<Texture2D>(@"map\background");
            map1.LoadContent(_contents);
            return base.LoadContents();
        }
        public override void Shutdown()
        {
            base.Shutdown();
        }
        public override void Update(float deltaTime)
        {
            _parManager.Update(deltaTime);
            TestTank();
            GameObject tank = _network.GetMainTank();
            if (tank != null)
                _camera.Update(deltaTime, tank.GetPosition());
            base.Update(deltaTime);
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, _camera.GetTransfromMatrix());
            sp.Draw(_background, Vector2.Zero, Color.White);
            DrawObj(sp);
            _parManager.Draw(sp);
            map1.Draw(sp);
            sp.End();
            base.Draw(sp);
        }

        protected void DrawObj(SpriteBatch sp)
        {
            Dictionary<string, Controller> controllers = Game1.network.GetControllers();
            foreach (Controller i in controllers.Values)
            {
                Dictionary<int, GameObject> obj = i.GetAllGameObject();
                foreach (GameObject ii in obj.Values)
                {
                    ii.Draw(sp);
                }
            }
        }

        // test method
        protected void TestItem()
        {
            // test
            if (Input.IsKeyDown(Keys.A))
            {
                SFSObject addData = new SFSObject();
                addData.PutDouble(Consts.X, 100);
                addData.PutDouble(Consts.Y, 100);
                addData.PutDouble(Consts.GO_ID, 100);
                addData.PutDouble(Consts.TYPE_KIND_OF_ITEM, 0);
                Dictionary<string, Controller> ctrl = _network.GetControllers();
                ctrl[Consts.CTRL_ITEM].Add(null, addData);
            }
            if (Input.IsKeyDown(Keys.C))
            {
                SFSObject data = new SFSObject();
                data.PutUtfString(Consts.BHVR, Consts.BHVR_ITEM_COUNT_DOWN);
                data.PutDouble(Consts.GO_ID, 100);
                Dictionary<string, Controller> ctrl = _network.GetControllers();
                ctrl[Consts.CTRL_ITEM].UpdateData(null, data);
            }
            if (Input.IsKeyDown(Keys.R))
            {
                SFSObject data = new SFSObject();
                data.PutDouble(Consts.GO_ID, 100);
                Dictionary<string, Controller> ctrl = _network.GetControllers();
                ctrl[Consts.CTRL_ITEM].Remove(null, data);
            }
        }
        // test tank
        protected void TestTank()
        {
            Dictionary<string, Controller> ctrl = _network.GetControllers();
            SmartFox sfs = _network.GetInstance();
            if (Input.IsKeyDown(Keys.A))
            {
                User myself = sfs.MySelf;
                SFSObject addData = new SFSObject();
                addData.PutDouble(Consts.X, 100);
                addData.PutDouble(Consts.Y, 100);
                ctrl[Consts.CTRL_TANK].Add(myself, addData);
            }
            if (Input.IsKeyDown(Keys.R))
            {
                GameObject tank = _network.GetMainTank();
                User myself = sfs.MySelf;
                ctrl[Consts.CTRL_TANK].Remove(myself, null);
                if (tank != null)
                    _parManager.Add(Consts.TYPE_PAR_EXPLOSION, new Rectangle((int)tank.GetPosition().X - 16, (int)tank.GetPosition().Y - 16, 32, 32));
            }
        }
    }
}
