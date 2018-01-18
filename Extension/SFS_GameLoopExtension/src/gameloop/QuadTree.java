package gameloop;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
import java.awt.Rectangle;
import java.util.ArrayList;
import java.util.List;

/**
 *
 * @author Administrator
 */
public class QuadTree 
{
    
 private int MAX_OBJECTS = 6;
        private int MAX_LEVELS = 10;

        private int level;
        private List<GameObject> objects;
        private Rectangle bounds;
        private QuadTree[] nodes;

        /*
         * Constructor
         */
        public QuadTree(int pLevel, Rectangle pBounds)
        {
            level = pLevel;
            objects = new ArrayList<GameObject>();
            bounds = pBounds;
            nodes = new QuadTree[4];
        }
        /*
 * Clears the quadtree
 */
        public void clear()
        {
            objects.clear();
            for (int i = 0; i < nodes.length; i++)
            {
                if (nodes[i] != null)
                {
                    nodes[i].clear();
                    nodes[i] = null;
                }
            }
        }
        /*
 * Splits the node into 4 subnodes
 */
        private void split()
        {
            int subWidth = (int)(bounds.width / 2);
            int subHeight = (int)(bounds.height / 2);
            int x = (int)bounds.x;
            int y = (int)bounds.y;

            nodes[0] = new QuadTree(level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
            nodes[1] = new QuadTree(level + 1, new Rectangle(x, y, subWidth, subHeight));
            nodes[2] = new QuadTree(level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
            nodes[3] = new QuadTree(level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
        }
        /*
 * Determine which node the object belongs to. -1 means
 * object cannot completely fit within a child node and is part
 * of the parent node
 */
        private int getIndex(GameObject entity)
        {
            int index = -1;
            double verticalMidpoint = bounds.x + (bounds.width / 2);
            double horizontalMidpoint = bounds.x + (bounds.height / 2);

            // Object can completely fit within the top quadrants
            boolean topQuadrant = (entity.GetY() < horizontalMidpoint && entity.GetY()+ entity.GetHeight() < horizontalMidpoint);
            // Object can completely fit within the bottom quadrants
            boolean bottomQuadrant = (entity.GetY() > horizontalMidpoint);

            // Object can completely fit within the left quadrants
            if (entity.GetX() < verticalMidpoint && entity.GetX() + entity.GetWidth() < verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 1;
                }
                else if (bottomQuadrant)
                {
                    index = 2;
                }
            }
            // Object can completely fit within the right quadrants
            else if (entity.GetX() > verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 0;
                }
                else if (bottomQuadrant)
                {
                    index = 3;
                }
            }

            return index;
        }
        /*
 * Insert the object into the quadtree. If the node
 * exceeds the capacity, it will split and add all
 * objects to their corresponding nodes.
 */
        public void insert(GameObject entity)
        {
            if (nodes[0] != null)
            {
                int index = getIndex(entity);

                if (index != -1)
                {
                    nodes[index].insert(entity);

                    return;
                }
            }

            objects.add(entity);

            if (objects.size() > MAX_OBJECTS && level < MAX_LEVELS)
            {
                if (nodes[0] == null)
                {
                    split();
                }

                int i = 0;
                while (i < objects.size())
                {
                    int index = getIndex(objects.get(i));
                    if (index != -1)
                    {
                        nodes[index].insert(objects.get(i));
                        objects.remove(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }
        /*
 * Return all objects that could collide with the given object
 */
        public List<GameObject> retrieve(List<GameObject> returnObjects, GameObject pRect)
        {
            int index = getIndex(pRect);
            if (index != -1 && nodes[0] != null)
            {
                nodes[index].retrieve(returnObjects, pRect);
            }

            returnObjects.addAll(objects);

            return returnObjects;
        }
    }
