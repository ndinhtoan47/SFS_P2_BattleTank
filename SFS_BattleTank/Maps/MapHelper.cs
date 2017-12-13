
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
namespace SFS_BattleTank.Maps
{
    static public class MapHelper
    {
        static public int[,] LoadFileMap(string path, ref int layerSize)
        {
            layerSize = 0;
            List<int[]> map = new List<int[]>();
            try
            {
                StreamReader reader = new StreamReader(path);
                // get size
                string line = reader.ReadLine();
                int k = 1;
                for (int i = line.Length; i > 0; i--)
                {
                    layerSize += ((int)line[i - 1] - 48) * k;
                    k *= 10;
                }

                // get map
                line = reader.ReadLine();
                char[] chararray = null;
                int count = 0;
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] != ',') count++;
                }
                while (line != null)
                {
                    chararray = line.ToCharArray();
                    int[] temp = new int[count];
                    int index = 0;
                    for (int i = 0; i < chararray.Length; i++)
                    {
                        if (chararray[i] != ',' && index < count)
                        {
                            temp[index] = (int)chararray[i] - 48;
                            index++;
                        }
                    }
                    map.Add(temp);
                    line = reader.ReadLine();
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            int x = map.Count;
            int y = map[0].Length;
            int[,] result = new int[x, y];
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    result[i, j] = map[i][j];
                }
            }
            return result;
        }
    }
}
