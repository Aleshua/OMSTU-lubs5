namespace SpaceBattle.Lib;
using Hwdtech;
public class EndMoveCommand : ICommand
{
    public IMoveCommandEndable obj;
    public EndMoveCommand(IMoveCommandEndable obj)
    {
        this.obj = obj;
    }
    public void Execute()
    {
        ICommand EndCommand = IoC.Resolve<ICommand>("Command.EmptyCommand");
        IoC.Resolve<ICommand>("Game.DeleteProperty", obj.UObject, "velocity").Execute();
        IoC.Resolve<ICommand>("Game.InjectCommand", obj.MoveCommand, EndCommand).Execute();
    }
}
