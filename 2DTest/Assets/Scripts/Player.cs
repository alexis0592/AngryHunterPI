using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour{

	private int id;
	private string nick;
	private int points;
	private bool connected;

	public Player ()
	{
	
	}

	public Player (int id, string nick, int points, bool connected)
	{
		this.id = id;
		this.nick = nick;
		this.points = points;
		this.connected = connected;
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
}
