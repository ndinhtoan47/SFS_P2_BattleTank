package demoUserVariables;

import java.util.Random;

import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.entities.variables.SFSRoomVariable;
import com.smartfoxserver.v2.exceptions.SFSVariableException;
import com.smartfoxserver.v2.extensions.SFSExtension;

public class RoomExtension extends SFSExtension
{

	public Game game;
	public Random rd;
	
	@Override
	public void init() 
	{
		// TODO Auto-generated method stub
		this.addEventHandler(SFSEventType.USER_VARIABLES_UPDATE, UserVariablesUpdateHanlder.class);
		this.addEventHandler(SFSEventType.USER_JOIN_ROOM,EventUserJoinRoomHandler.class);
		this.addEventHandler(SFSEventType.USER_DISCONNECT, EventUserDisconnect.class);
		// client request
		this.addRequestHandler("ready", ReadyRequestHandler.class);
		this.addRequestHandler("getallid", GetAllIdHandler.class);
		this.addRequestHandler("play", PlayRequestHandler.class);
		
		game = new Game(this);
		rd = new Random();
		try {
			this.getParentRoom().setVariable(new SFSRoomVariable("isPlayed",false));
		} catch (SFSVariableException e) {
			// TODO Auto-generated catch block
			trace(e.getMessage());
		}
	}
	@Override
	 public void destroy()
	 {
		 super.destroy();
	 }
	public Game GetGameInstance()
	{
		return game;
	}

}
