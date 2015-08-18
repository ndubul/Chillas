using System.Windows.Input;
using Dev2.Runtime.ServiceModel.Data;

namespace Dev2.Common.Interfaces
{
    public interface IManageSharePointSourceViewModel
    {
        /// <summary>
        ///  User or Anonymous
        /// </summary>
        AuthenticationType AuthenticationType { get; set; }

        /// <summary>
        /// The Host Name
        /// </summary>
        string ServerName { get; set; }


        /// <summary>
        /// User Name
        /// </summary>
        string UserName { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Test if connection is successful
        /// </summary>
        ICommand TestCommand { get; set; }

        ICommand CancelTestCommand { get; set; }

        string CancelTestLabel { get; }
        /// <summary>
        /// The message that will be set if the test is either successful or not
        /// </summary>
        string TestMessage { get; }

        /// <summary>
        /// Localized text for the UserName label
        /// </summary>
        string UserNameLabel { get; }

        /// <summary>
        /// Localized text for the Authentication Type label
        /// </summary>
        string AuthenticationLabel { get; }

        /// <summary>
        /// Localized text for the Password label
        /// </summary>
        string PasswordLabel { get; }

        /// <summary>
        /// Localized text for the Test label
        /// </summary>
        string TestLabel { get; }

        /// <summary>
        /// The localized text for the Host name label
        /// </summary>
        string ServerNameLabel { get; }
        /// <summary>
        /// Command for save/ok
        /// </summary>
        ICommand SaveCommand { get; set; }

        /// <summary>
        /// Header text that is used on the view
        /// </summary>
        string HeaderText { get; set; }

        /// <summary>
        /// Tooltip for the Windows Authentication option
        /// </summary>
        string AnonymousAuthenticationToolTip { get; }

        /// <summary>
        /// Tooltip for the User Authentication option
        /// </summary>
        string UserAuthenticationToolTip { get; }
        /// <summary>
        /// Has test passed
        /// </summary>
        bool TestPassed { get; set; }

        /// <summary>
        /// has test failed
        /// </summary>
        bool TestFailed { get; set; }
        /// <summary>
        /// IsTesting
        /// </summary>
        bool Testing { get; }

        /// <summary>
        /// The name of the resource
        /// </summary>
        // ReSharper disable UnusedMemberInSuper.Global
        string ResourceName { get; set; }
        // ReSharper restore UnusedMemberInSuper.Global

        /// <summary>
        /// The authentications Type
        /// </summary>
        bool UserAuthenticationSelected { get; }
    }
    public interface ISharePointSourceModel
    {
        void TestConnection(ISharepointServerSource resource);

        void Save(ISharepointServerSource toSpSource);

        string ServerName { get; set; }
    }
}
