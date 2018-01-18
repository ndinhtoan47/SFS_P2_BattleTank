
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
namespace SFS_BattleTank.Maps
{
    public class Map1 : Map
    {
        public Map1()
        :base()
        { }

        public override void Init()
        {
            int size = 0;
            int[,] treeLayer = MapHelper.LoadFileMap(@"..\..\..\..\Data\layer1.data",ref size);
            this.Add(1, treeLayer, size);
            //int[,] emptyLayer = MapHelper.LoadFileMap(@"..\..\..\..\Data\layer0.data", ref size);
            //this.Add(2, emptyLayer, size);
            base.Init();
        }
        public override void Draw(SpriteBatch sp)
        {
            foreach (List<Tile> list in _map.Values)
            {
                foreach (Tile tile in list)
                {
                    tile.Draw(sp);
                }
            }
            base.Draw(sp);
        }
        public override void LoadContent(ContentManager contents)
        {
            foreach(List<Tile> list in _map.Values)
            {
                foreach(Tile tile in list)
                {
                    tile.LoadContents(contents);
                }
            }
            base.LoadContent(contents);
        }
    }
}
