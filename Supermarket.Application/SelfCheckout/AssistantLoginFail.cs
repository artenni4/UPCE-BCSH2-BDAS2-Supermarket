namespace Supermarket.Core.SelfCheckout
{
    public enum AssistantLoginFail
    {
        /// <summary>
        /// In case of bad login data
        /// </summary>
        InvalidCredentials,

        /// <summary>
        /// In case when employee does not have required role
        /// </summary>
        PermissionDenied
    }
}
