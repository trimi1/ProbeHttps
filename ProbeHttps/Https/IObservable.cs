namespace ProbeHttps.Interface;

public interface IObservable
{
    event Action? StateChanged; 
}