
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
namespace SFS_BattleTank.Maps
{
    public class Map
    {
        protected Dictionary<int, List<Tile>> _map;
        public Map()
        {
            _map = new Dictionary<int, List<Tile>>();
        }
        public virtual void Draw(SpriteBatch sp) { }
        public virtual void Init() { }
        public virtual void LoadContent(ContentManager contents) { }

        public List<Tile> GetTilesByIndex(int index)
        {
            if (_map.ContainsKey(index))
            {
                return _map[index];
            }
            return null;
        }
        public Dictionary<int, List<Tile>> GetMap()
        {
            return _map;
        }

        protected void Add(int layerIndex, int[,] layer, int size)
        {
            if (_map.ContainsKey(layerIndex)) return;
            List<Tile> temp = new List<Tile>();

            int x = layer.GetLength(0);
            int y = layer.GetLength(1);
            if(x > y)
            {
                int y2 = y;
                y = x;
                x = y2;
            }
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (layer[i, j] != 0)
                        temp.Add(new Tile(j * size, i * size, size, layer[i, j]));
                }
            }
            _map.Add(layerIndex, temp);
        }
    }
}
