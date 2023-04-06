namespace OdmGear.GrappleHooks.Scripts
{
    public interface IGrappleHookInput
    {
        public delegate void OnLaunchHookInput();

        public event OnLaunchHookInput OnLaunchHookEventInput;

        public delegate void OnReleaseHookInput();

        public event OnReleaseHookInput OnReleaseHookEventInput;
    }
}