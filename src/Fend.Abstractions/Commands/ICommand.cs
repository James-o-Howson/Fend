namespace Fend.Abstractions.Commands;

public interface ICommand<TResult> : IBaseRequest;
public interface ICommand : IBaseRequest;
public interface IBaseRequest;