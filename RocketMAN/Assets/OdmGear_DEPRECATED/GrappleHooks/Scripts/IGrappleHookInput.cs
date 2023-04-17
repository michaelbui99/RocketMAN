namespace OdmGear.GrappleHooks.Scripts
{
    public interface IGrappleHookInput
    {
        public delegate void OnLaunchHookInput();

        public event OnLaunchHookInput OnLaunchHookEventInput;

        public delegate void OnReleaseHookInput();

        public event OnReleaseHookInput OnReleaseHookEventInput;

        public delegate void OnWheelInInput();

        public event OnWheelInInput OnWheelInEvent;

        public delegate void OnWheelOutInput();

        public event OnWheelOutInput OnWheelOutEvent;
    }
}