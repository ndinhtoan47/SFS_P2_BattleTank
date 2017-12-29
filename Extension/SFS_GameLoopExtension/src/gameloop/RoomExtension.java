package gameloop;

import java.util.concurrent.ScheduledFuture;
import java.util.concurrent.TimeUnit;

import com.smartfoxserver.v2.SmartFoxServer;
import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.extensions.SFSExtension;

public class RoomExtension extends SFSExtension
{
	private SmartFoxServer sfs;
	private Game game;
	private ScheduledFuture<?> gameTask;
	@Override
	public void init() 
	{
		this.addEventHandler(SFSEventType.USER_JOIN_ROOM,EventUserJoinRoom.class);
		// Get a reference to the SmartFoxServer instance
		sfs = SmartFoxServer.getInstance();
		game = new Game(this);
		gameTask = sfs.getTaskScheduler().scheduleAtFixedRate(game, 0, 1000, TimeUnit.MILLISECONDS);
	}
	@Override
	public void destroy()
	{
		gameTask.cancel(true);
	}
	public Game GetGameInstance() {
		// TODO Auto-generated method stub
		return game;
	}

}
