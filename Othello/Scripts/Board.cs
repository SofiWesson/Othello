using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class Piece
{
	public Piece(Node2D piece, State state)
	{
		this.piece = piece;
		GetOrientations();
		SetState(state);
	}

	public enum State
	{
		RESERVED = 0,
		WHITE,
		BLACK
	}

	private State state;
	private Node2D piece;
	private Node2D reserved;
	private Node2D white;
	private Node2D black;

	private void GetOrientations()
	{
		for (int i = 0; i < piece.GetChildCount(); i++)
		{
			Node2D orientation = (Node2D)piece.GetChild(i);
			if (orientation.Name == "WhiteSide")
				white = orientation;
			else if (orientation.Name == "BlackSide")
				black = orientation;
			else
				reserved = orientation;
		}
	}

	#region Getters and Setters
	public State GetState()
	{
		return state;
	}
	public void SetState(State state)
	{
		this.state = state;
		if (state == State.RESERVED)
		{
			reserved.Visible = true;
			white.Visible = false;
			black.Visible = false;
		}
		else if (state == State.WHITE)
		{
			white.Visible = true;
			black.Visible = false;
			reserved.Visible = false;
		}
		else if (state == State.BLACK)
		{
			black.Visible = true;
			white.Visible = false;
			reserved.Visible = false;
		}
	}
	public Node2D GetPiece()
	{
		return piece;
	}
	#endregion
}

public class Space
{
	// instantiater
	public Space(Piece piece, State state, Board board)
	{
		this.board = board;
		this.piece = piece;
		this.state = state;
	}

	public enum State
	{
		EMPTY = 0,
		OCCUPIED
	}

	// reference to the board
	private Board board;
	private Piece piece;
	private State state;
	private float positionOffset = 3.5f;

	#region Getters and Setters
	public Piece GetPiece()
	{
		return piece;
	}
	public Vector2 GetPosition(int row, int column)
	{
		Vector2 position = new Vector2((column - positionOffset) * board.GetSizeModifier(), (row - positionOffset) * board.GetSizeModifier());
		Debug.Print(position.X + " " + position.Y);
		return position;
	}
	public State GetState()
	{
		return state;
	}
	public void SetState(State state)
	{
		this.state = state;
	}
	#endregion
}

public partial class Board : Node
{
	private Space[,] board = new Space[8, 8];
	private PackedScene packedPiece = ResourceLoader.Load<PackedScene>("res://Prefabs/Piece.tscn");
	private int boardSize = 8;
	private float sizeModifier = 10;
	private bool boardUpdated = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitialiseBoardSpaces();
		SetBoardStart();
	}
	private void InitialiseBoardSpaces()
	{
		for (int i = 0; i < boardSize; i++)
		{
			for (int j = 0; j < boardSize; j++)
			{
				Node2D piece = (Node2D)packedPiece.Instantiate();
				AddChild(piece);
				Piece newPiece = new Piece(piece, Piece.State.RESERVED);
				Space newSpace = new Space(newPiece, Space.State.EMPTY, this);
				board[i, j] = newSpace;
			}
		}
	}
	public void SetBoardStart()
	{
		// set white centre start pieces
		PlacePiece(4, 3, Space.State.OCCUPIED, Piece.State.WHITE);
		PlacePiece(3, 4, Space.State.OCCUPIED, Piece.State.WHITE);
		// set black centre start pieces
		PlacePiece(4, 4, Space.State.OCCUPIED, Piece.State.BLACK);
		PlacePiece(3, 3, Space.State.OCCUPIED, Piece.State.BLACK);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		boardUpdated = false;
	}

	private void PlacePiece(int row, int column, Space.State occupancy, Piece.State colour)
	{
		board[row, column].SetState(occupancy);
		board[row, column].GetPiece().SetState(colour);
		board[row, column].GetPiece().GetPiece().Position = board[row, column].GetPosition(row, column);
		boardUpdated = true;
	}

	#region Getters and Setters
	public float GetSizeModifier()
	{
		return sizeModifier;
	}
	#endregion
}
