namespace Ocean_of_souls.MVVM.ViewModels
{
    class StaticTransmitter
    {
        #region DATA - USER
        public static string Login { get; set; }

        public static bool IsDark { get; set; }
        #endregion

        #region DATA - MESSAGE
        public static string MessageText { get; set; }

        public static int IdItem { get; set; }

        public static bool IsAnswer { get; set; } = false;
        #endregion

        #region DATA - ADDCHAT
        public static string NameGroup { get; set; }

        public static bool isCreationGroup { get; set; }
        #endregion
    }
}
