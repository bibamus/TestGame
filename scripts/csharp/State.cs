﻿using Godot;

namespace TestGame.global;

public abstract partial class State : Node
{
    public virtual void  StateEnter(){}
    public virtual void StateProcess(double delta){}
    public virtual void StateExit(){}
}