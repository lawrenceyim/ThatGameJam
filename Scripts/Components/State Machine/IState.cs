using InputSystem;

namespace StateMachineSystem;

public interface IState {
    public void Process(double delta);
    public void PhysicsProcess();
    public void Enter();
    public void Exit();
    public void Input(InputEventDto dto);
}
