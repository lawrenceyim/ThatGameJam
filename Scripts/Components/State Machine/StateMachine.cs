#nullable enable
using System;
using System.Collections.Generic;
using InputSystem;

namespace StateMachineSystem;

public class StateMachine<T> where T : Enum {
    internal readonly Dictionary<T, IState> states = new();
    internal IState? currentState;
    internal T currentKey;

    public void AddState(T key, IState state) {
        states[key] = state;
    }

    public void RemoveState(T key) {
        states.Remove(key);
    }

    public void Process(double delta) {
        currentState?.Process(delta);
    }

    public void PhysicsProcess() {
        currentState?.PhysicsProcess();
    }

    public void Input(InputEventDto dto) {
        currentState?.Input(dto);
    }

    public void SwitchState(T key) {
        currentKey = key;
        IState? state = states.GetValueOrDefault(key);
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }

    public T GetCurrentKey() {
        return currentKey;
    }
}