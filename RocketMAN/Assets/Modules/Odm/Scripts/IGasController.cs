namespace Modules.Odm.Scripts
{
    public interface IGasController
    {
        delegate void SimpleGasEvent();

        event SimpleGasEvent OnGasActivated;
        event SimpleGasEvent OnGasReleased;
    }
}