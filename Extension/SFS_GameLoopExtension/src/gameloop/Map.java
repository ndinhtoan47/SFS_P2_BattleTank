package gameloop;

import com.smartfoxserver.v2.mmo.Vec3D;

import java.util.LinkedList;
import java.util.List;

/**
 *
 * @author Administrator
 */
public class Map {
	public int number;
	public int totaltitle = 0;
	public List<Vec3D> console = new LinkedList<Vec3D>();
	private int Wight, Height;
	private final List<CollisionTitle> conllisiontiles = new LinkedList<CollisionTitle>();

	public List<CollisionTitle> getCollisionListTiles() {
		return conllisiontiles;
	}

	public int getWight() {
		return Wight;
	}

	public int getHeight() {
		return Height;
	}

	public Map() {
	}

	public void Genanrate(int[][] map, int size, RoomExtension ext, int roll, int collum) {
		for (int x = 0; x < collum; x++)
			for (int y = 0; y < roll; y++) 
			{
				number = map[y][x];
				if (number != 0) 
				{
					conllisiontiles.add(new CollisionTitle(x * size, y * size, size, size));
					totaltitle += 1;
					ext.trace("add 1 title map  sucess");
				} else 
				{
					//console.add(new Vec3D(x * size, y * size));
				}
				Wight = (x + 1) * size;
				Height = (y + 1) * size;
			}

	}
	// public void write(RoomExtension ext)
	// {
	// for(CollisionTitle title:conllisiontiles)
	// {
	//// title.write(ext);
	// }
	// }
}