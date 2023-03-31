namespace OdmGear.GrappleHooks
{
    public interface IGrappleHookInput
    {
        public delegate void OnLaunchHookInput();

        public event OnLaunchHookInput OnLaunchHookEventInput;
    }
}