namespace SpaceBattle.Lib;

public interface IMoveCommandEndable
{
    IUObject UObject { get; }
    ICommand MoveCommand { get; }
    Queue<ICommand> Queue { get; }
}
