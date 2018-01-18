package gameloop;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
 public class CollisionTitle extends GameObject
{
    protected int _titleID;
    public CollisionTitle(float x,float y,int w,int h)
    {
        super(x, y, w, h);
        super.SetType(RoomExtension.ES_TILE);
    }
    public void SetTitleID(int tId)
    {
        this._titleID= tId;
    }
    
    public int GetTittleID()
    {
        return _titleID;
    }
}
