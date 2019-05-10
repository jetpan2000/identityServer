using System;

namespace Octacom.Odiss.Core.Entities.Settings
{
    public class ApplicationSettings
    {
        /// <summary>
        /// Website name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Maximum records returned for each Application
        /// </summary>
        public int MaxRecords { get; set; }

        /// <summary>
        /// Maximum records presented for every page
        /// </summary>
        public int MaxPerPage { get; set; }

        /// <summary>
        /// Restrict application access for each user
        /// </summary>
        public bool RestrictAppsAccess { get; set; }

        /// <summary>
        /// Date format for the entire website
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// Date format definition for the entire website (using languages)
        /// </summary>
        public string DateFormatDefinition { get; set; }

        /// <summary>
        /// Company Logo
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// Shortcut Icon (Favicon)
        /// </summary>
        public string ShortcutIcon { get; set; }

        /// <summary>
        /// Color scheme
        /// </summary>
        public string ColorScheme { get; set; }

        public ViewerType ViewerType { get; set; }

        /// <summary>
        /// Enabled languages. Array containing two digit language name
        /// </summary>
        public string[] EnabledLanguages { get; set; }

        /// <summary>
        /// Path to submitted temporary files
        /// </summary>
        public string TemporarySubmitFilesPath { get; set; }

        public string DocumentsPath { get; set; }

        /// <summary>
        /// Lock the user access after X wrong attempts
        /// </summary>
        public int LockUserAfterAttempts { get; set; }

        /// <summary>
        /// Lock the user for X minutes after Y attempts
        /// </summary>
        public int LockUserForMinutes { get; set; }

        /// <summary>
        /// Maximum concurrent connected users
        /// </summary>
        public int MaxConcurrentUsers { get; set; }

        /// <summary>
        /// Maximum users (active users)
        /// </summary>
        public int MaxNamedUsers { get; set; }

        /// <summary>
        /// Named users out of max limit
        /// </summary>
        public string NamedUsersOutOfMaxLimit { get; set; }

        /// <summary>
        /// Minimum password length
        /// </summary>
        public int MinimumPasswordLength { get; set; }

        /// <summary>
        /// Maximum password length
        /// </summary>
        public int MaximumPasswordLength { get; set; }


        /// <summary>
        /// Force Password Strength
        /// </summary>
        public bool ForcePasswordStrength { get; set; }

        /// <summary>
        /// Enable Password Reset
        /// </summary>
        public bool PasswordReset { get; set; }

        /// <summary>
        /// Maximum Session Timeout
        /// </summary>
        public int MaximumSessionTimeout { get; set; }

        /// <summary>
        /// Enable Username Reminder
        /// </summary>
        public bool UsernameReminder { get; set; }

        /// <summary>
        /// Procedure to reset all data
        /// </summary>
        public string ResetProcedure { get; set; }

        /// <summary>
        /// Group Applications menu as Dropdown
        /// </summary>
        public bool GroupApplications { get; set; }

        /// <summary>
        /// Group Applications menu name
        /// </summary>
        public string GroupApplicationsName { get; set; }

        /// <summary>
        /// Default Application
        /// First application to open after user logins in
        /// </summary>
        public Guid? DefaultApplication { get; set; }

        /// <summary>
        /// Hidden Fields Option - Name
        /// Name to show on users tab security option when hidding fields from some users
        /// </summary>
        public string HiddenFieldsOptionName { get; set; }

        public bool CustomCSS { get; set; }
    }
}
