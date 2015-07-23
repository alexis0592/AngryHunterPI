using UnityEngine;
using System.Collections;

public class Player {

	private int id;
	private string nick;
	private int points;
	private bool connected;
	private object mira;

	public Player ()
	{
	
	}

	public Player (int id, string nick, int points, bool connected, object mira)
	{
		this.id = id;
		this.nick = nick;
		this.points = points;
		this.connected = connected;
		this.mira = mira;
	}
	
	public int Id {
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
