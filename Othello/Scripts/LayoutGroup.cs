using Godot;
using System;
using System.Collections.Generic;
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
	private Node2D[,] children;
	private int childrenLastUpdate = 0;
	private Vector2 boundingWidthPositions = Vector2.Zero;
	private Vector2 boundingHeightPositions = Vector2.Zero;
	private float childrenPerRowColumn = 0;

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
	}

	private void UpdateChildren()
	{
		GetChildrenPerRowColumn();

		if (GetChildCount(false) != childrenLastUpdate)
			UpdateChildArray();

		GetMinWidthHeight();
		UpdateBoundingPositions();
		SetChildrenPositions();

		childrenLastUpdate = GetChildCount(false);
	}
	// how many children in each row or column
	private void GetChildrenPerRowColumn()
	{
		float childCount = GetChildCount(false);
		// how many children can go into a row/column
		childrenPerRowColumn = childCount / rowColumns;
		// rounding up non whole numbers, left over goes in last row/column
		float remainder = childrenPerRowColumn % 1;
		if (remainder != 0)
			childrenPerRowColumn += 1 - remainder;
	}
	// update array to include new children
	private void UpdateChildArray()
	{
		// clear array for repopulation if not null
		if (children != null)
			Array.Clear(children);

		int rowColumn = 0;
		int childrenInRowColumn = 0;
		for (int i = 0; i < GetChildCount(false); i++)
		{
			children[rowColumn, childrenInRowColumn] = GetChild<Node2D>(i);
			childrenInRowColumn++;
			if (childrenInRowColumn == childrenPerRowColumn)
			{
				rowColumn++;
				childrenInRowColumn = 0;
			}
		}
	}
	// get the minimum width and height of the layout group --------------------- OPTIMISE ---------------------
	private void GetMinWidthHeight()
	{
		if (orientation == Orientation.HORIZONTAL)
		{
			float widestRow = 0; // trackes the widest row, determines the minimum width
			float currentRowWidth = 0; // tracks width of the current row being checked, replaces widestRow if it's bigger
			float highestInRow = 0; // tracks the highest item of the current row being checked
			float totalHeight = 0; // combination of the highest items in each row, determines minimum height
			for (int i = 0; i < rowColumns; i++)
			{
				for (int rowColumn = 0; rowColumn < childrenPerRowColumn; rowColumn++)
				{
					// gets the Sprite2D of the selected child then gets the width of it's bounding box
					currentRowWidth += children[i, rowColumn].GetNode<Sprite2D>(children[i, rowColumn].GetPath()).GetRect().Size.X;
					// gets selected childs height
					float itemHeight = children[i, rowColumn].GetNode<Sprite2D>(children[i, rowColumn].GetPath()).GetRect().Size.Y;
					// track the highest item of the current row being checked
					if (itemHeight > highestInRow)
						highestInRow = itemHeight;
				}
				// tracks the widest row
				if (currentRowWidth > widestRow)
					widestRow = currentRowWidth;
				currentRowWidth = 0; // reset for next loop
				totalHeight += highestInRow; // combination of the highest items in each row
				highestInRow = 0; // reset for next loop
			}
			minWidth = widestRow;
			minHeight = totalHeight;
		}
		else if (orientation == Orientation.VERTICAL)
		{
			float highestColumn = 0; // trackes the highest column, determines the minimum height
			float currentColumnHeight = 0; // tracks height of the current column being checked, replaces highestColumn if it's bigger
			float widestInColumn = 0; // tracks the widest item of the current column being checked
			float totalWidth = 0; // combination of the widest items in each column, determines minimum width
			for (int i = 0; i < rowColumns; i++)
			{
				for (int rowColumn = 0; i < childrenPerRowColumn; rowColumn++)
				{
					// gets the Sprite2D of the selected child then gets the height of it's bounding box
					currentColumnHeight += children[i, rowColumn].GetNode<Sprite2D>(children[i, rowColumn].GetPath()).GetRect().Size.Y;
					// gets selected childs width
					float itemWidth = children[i, rowColumn].GetNode<Sprite2D>(children[i, rowColumn].GetPath()).GetRect().Size.X;
					// track the widest item of the current column being checked
					if (itemWidth > widestInColumn)
						widestInColumn = itemWidth;
				}
				// tracks the highest column
				if (currentColumnHeight > highestColumn)
					highestColumn = currentColumnHeight;
				currentColumnHeight = 0; // reset for next loop
				totalWidth += widestInColumn; // combination of the widest items in each column
				widestInColumn = 0; // reset for next loop
			}
			minWidth = totalWidth;
			minHeight = highestColumn;
		}
		width = minWidth;
		height = minHeight;
	}
	// updates the dimentions of the bounding box
	private void UpdateBoundingPositions()
	{
		// offsets for horizontal
		if (orientation == Orientation.HORIZONTAL)
		{
			height += topPadding + bottomPadding + (spacing * rowColumns);
			width += leftPadding + rightPadding + (spacing * childrenPerRowColumn);
		}
		else // offsets for vertical
		{
			height += topPadding + bottomPadding + (spacing * childrenPerRowColumn);
			width += leftPadding + rightPadding + (spacing * rowColumns);
		}
		Vector2 position = GetNode<Node2D>(GetPath()).Position;
		// bounding width on the x axis, bounding width x is left, bounding width y is right
		boundingWidthPositions.X = position.X;
		boundingWidthPositions.Y = position.X + width;
		// bounding height on the y axis, bounding height x is up, bounding height y is down
		boundingHeightPositions.X = position.Y;
		boundingHeightPositions.Y = position.Y + height;
	}
	private void SetChildrenPositions()
	{
		float rightOffset = 0;
		float downOffset = 0;
		Vector2 position = GetNode<Node2D>(GetPath()).Position;
		// set positions using spacing, padding, rowColumns, childrenPerRowColumn
		// need to know widest and highest items before hand
		
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
	public Vector2 GetBoundingHeightPositions()
	{
		return boundingHeightPositions;
	}
	public Vector2 GetBoundingWidthPositions()
	{
		return boundingWidthPositions;
	}
	#endregion
}
