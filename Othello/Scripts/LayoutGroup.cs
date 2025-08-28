using Godot;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public partial class LayoutGroup : Node
{
	[Export] private float width = 0f;
	[Export] private float height = 0f;
	private float minWidth = 0f;
	private float minHeight = 0f;
	[Export] private float spacing = 0f;
	[Export] private float topPadding = 0f;
	[Export] private float bottomPadding = 0f;
	[Export] private float leftPadding = 0f;
	[Export] private float rightPadding = 0f;
	[Export] private int rowColumns = 1;
	[Export] private Orientation orientation = Orientation.HORIZONTAL;
	private int childrenLastUpdate = 0;

	private int targetFrameRate = 60;
	private float frameTimer = 0f;
	private float frameTimerReset = 1f;
	private int frameCount = 0;
	private int framesLastSecond = 0;


	public enum Orientation
	{
		HORIZONTAL,
		VERTICAL
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (frameTimer <= 0)
		{
			frameTimer = frameTimerReset;
			framesLastSecond = frameCount;
			frameCount = 0;
		}
		frameTimer -= (float)delta;
		frameCount++;
		float frameInterval = framesLastSecond / targetFrameRate;
		float intervalCheck = frameCount / frameInterval;
		if (intervalCheck % 1 == 0)
		{
			FixedUpdate();
		}
	}
	private void FixedUpdate()
	{
		if (GetChildCount(false) != childrenLastUpdate)
		{
			UpdateChildren();
		}
		childrenLastUpdate = GetChildCount(false);
	}

	private void UpdateChildren()
	{
		
	}

	#region Getters and Setters
	public float GetWidth()
	{
		return width;
	}
	public void SetWidth(float width)
	{
		this.width = width;
		UpdateChildren();
	}
	public float GetHeight()
	{
		return height;
	}
	public void SetHeight(float height)
	{
		this.height = height;
		UpdateChildren();
	}
	public float GetMinWidth()
	{
		return minWidth;
	}
	public float GetMinHeight()
	{
		return minHeight;
	}
	public float GetSpacing()
	{
		return spacing;
	}
	public void SetSpacing(float spacing)
	{
		this.spacing = spacing;
		UpdateChildren();
	}
	public float GetTopPadding()
	{
		return topPadding;
	}
	public void SetTopPadding(float topPadding)
	{
		this.topPadding = topPadding;
		UpdateChildren();
	}
	public float GetBottomPadding()
	{
		return bottomPadding;
	}
	public void SetBottomPadding(float bottomPadding)
	{
		this.bottomPadding = bottomPadding;
		UpdateChildren();
	}
	public float GetLeftPadding()
	{
		return leftPadding;
	}
	public void SetLeftPadding(float leftPadding)
	{
		this.leftPadding = leftPadding;
		UpdateChildren();
	}
	public float GetRightPadding()
	{
		return rightPadding;
	}
	public void SetRightPadding(float rightPadding)
	{
		this.rightPadding = rightPadding;
		UpdateChildren();
	}
	public int GetRowColumns()
	{
		return rowColumns;
	}
	public void SetRowColumns(int rowColumns)
	{
		this.rowColumns = rowColumns;
		UpdateChildren();
	}
	public Orientation GetOrientation()
	{
		return orientation;
	}
	public void SetOrientation(Orientation orientation)
	{
		this.orientation = orientation;
		UpdateChildren();
	}
	#endregion
}
