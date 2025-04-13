using Godot;
using System;

public partial class Buildings : TileMapLayer, IHurtbox
{
	public void disable()
	{
		throw new NotImplementedException();
	}

	public void enable()
	{
		throw new NotImplementedException();
	}

	public Node getHurtboxOwner()
	{
		return this;
	}

	public bool isEnabled()
	{
		throw new NotImplementedException();
	}

}
