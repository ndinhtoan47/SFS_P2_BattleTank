package gameloop;

import java.util.Arrays;
import java.util.Random;
import java.util.concurrent.ScheduledFuture;
import java.util.concurrent.TimeUnit;

import com.smartfoxserver.v2.SmartFoxServer;
import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.entities.variables.RoomVariable;
import com.smartfoxserver.v2.entities.variables.SFSRoomVariable;
import com.smartfoxserver.v2.extensions.SFSExtension;
import com.smartfoxserver.v2.mmo.MMORoom;

public class RoomExtension extends SFSExtension
{
	public static final int STATE_WAIT = 1;
	public static final int STATE_PLAYING = 2;
	public static final double PI = Math.PI;
	public static final int SPEED_BULLET = 150;
	public static final int SPEED_TANK = 140;
	public static final float DEATH_DURATION = 5.0f;
	
	public static final int ITEM_TYPE_BULLET = 12;
	public static final int ITEM_TYPE_ARMOR = 13;
	public static final int ITEM_TYPE_ISVISIABLE = 14;
	public static final int ITEM_TYPE_FREZZE = 15;	
	public static final int ES_TANK = 11;
	public static final int ES_TILE = 16;
	
	public static Random rd;
	
	private int gameState;
	private SmartFoxServer sfs;
	private Game game;
	private ScheduledFuture<?> gameTask;
	private MMORoom room;
	@Override
	public void init() 
	{
		rd = new Random();
		
		this.addEventHandler(SFSEventType.USER_JOIN_ROOM,EventUserJoinRoom.class);
		this.addEventHandler(SFSEventType.USER_DISCONNECT, EventUserDisconnect.class);
		this.addEventHandler(SFSEventType.USER_LEAVE_ROOM, EventUserLeaveRoom.class);
		this.addEventHandler(SFSEventType.USER_VARIABLES_UPDATE, EventUserVariableUpdate.class);
		
		this.addRequestHandler("fire", RequestFire.class);
		this.addRequestHandler("ready", RequestReady.class);
		this.addRequestHandler("play", RequestPlay.class);
		this.addRequestHandler("item", RequestTest.class);
		
		// Get a reference to the SmartFoxServer instance
		game = new Game(this);
		sfs = SmartFoxServer.getInstance();
		gameState = STATE_WAIT;
		
		room = (MMORoom)this.getParentRoom();
		RoomVariable state = new SFSRoomVariable("state",(int)STATE_WAIT);
		room.setVariables(Arrays.asList(state));
		
		gameTask = sfs.getTaskScheduler().scheduleAtFixedRate(game, 0, 10, TimeUnit.MILLISECONDS);
	}
	@Override
	public void destroy()
	{
		gameTask.cancel(true);
	}
	public Game GetGameInstance() {
		return game;
	}
	public int GetGameState()
	{
		return gameState;
	}

	public void SetGameState(int state)
	{
		gameState = state;
	}
	
	public static double ConvertToRadian(double value)
	{
		double result = 0;
		// pi = 180
		result = (value * RoomExtension.PI)/180.0;
		return result;
	}
}
