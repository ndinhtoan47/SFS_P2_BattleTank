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
	
	public Random rd;
	
	private int gameState;
	private SmartFoxServer sfs;
	private Game game;
	private ScheduledFuture<?> gameTask;
	private MMORoom room;
	@Override
	public void init() 
	{
		game = new Game(this);
		this.addEventHandler(SFSEventType.USER_JOIN_ROOM,EventUserJoinRoom.class);
		this.addEventHandler(SFSEventType.USER_DISCONNECT, EventUserDisconnect.class);
		this.addEventHandler(SFSEventType.USER_LEAVE_ROOM, EventUserLeaveRoom.class);
		this.addEventHandler(SFSEventType.USER_VARIABLES_UPDATE, EventUserVariableUpdate.class);
		
		this.addRequestHandler("ready", RequestReady.class);
		this.addRequestHandler("play", RequestPlay.class);
		// Get a reference to the SmartFoxServer instance
		rd = new Random();
		sfs = SmartFoxServer.getInstance();
		gameState = STATE_WAIT;
		
		room = (MMORoom)this.getParentRoom();
		RoomVariable state = new SFSRoomVariable("state",(int)STATE_WAIT);
		room.setVariables(Arrays.asList(state));
		
		gameTask = sfs.getTaskScheduler().scheduleAtFixedRate(game, 0, 1000, TimeUnit.MILLISECONDS);
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
}
