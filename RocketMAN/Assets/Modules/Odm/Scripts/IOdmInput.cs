namespace Modules.Odm.Scripts
{
    public interface IOdmInput
    {
       public delegate void OdmInputTrigger();

       public event OdmInputTrigger OnGasActivate;
       public event OdmInputTrigger OnGasRelease;
       public event OdmInputTrigger OnLeftHookFire;
       public event OdmInputTrigger OnLeftHookRelease;
       public event OdmInputTrigger OnRightHookFire;
       public event OdmInputTrigger OnRightHookRelease;
    }
}