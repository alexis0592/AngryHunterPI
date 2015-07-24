using UnityEngine;
using System.Collections;

public class Player {

	private string id;
	private string nick;
	private int points;
	private bool connected;
	private object mira;

	public Player ()
	{
	
	}

	public Player (string id, string nick, int points, bool connected, object mira)
	{
		this.id = id;
		this.nick = nick;
		this.points = points;
		this.connected = connected;
		this.mira = mira;
	}
	
	public string Id {
		get;
		set;
	}

	public string Nick {
		get;
		set;
	}

	public int Points {
		get;
		set;
	}

	public bool Connected {
		get;
		set;
	}

	public object Mira {
		get;
		set;
	}

}
