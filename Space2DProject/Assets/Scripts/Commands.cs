using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBase
{
    private string _commandId;
    private string _commandDescription;
    private string _commandFormat;

    public string commandId => _commandId;
    public string commandDescription => _commandDescription;
    public string commandFormat => _commandFormat;


    public CommandBase(string id, string description, string format)
    {
        _commandId = id;
        _commandDescription = description;
        _commandFormat = format;
    }
}

public class Commands : CommandBase
{
    private Action command;
        
    public Commands(string id, string description, string format,Action command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}

public class Commands<T1> : CommandBase
{
    private Action<T1> command;
        
    public Commands(string id, string description, string format,Action<T1> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value)
    {
        command.Invoke(value);
    }
}

public class Commands<T1,T2> : CommandBase
{
    private Action<T1,T2> command;
        
    public Commands(string id, string description, string format,Action<T1,T2> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value1,T2 value2)
    {
        command.Invoke(value1,value2);
    }
}

public class Commands<T1, T2,T3> : CommandBase
{
    private Action<T1, T2, T3> command;

    public Commands(string id, string description, string format, Action<T1, T2, T3> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value1, T2 value2, T3 value3)
    {
        command.Invoke(value1, value2, value3);
    }
}