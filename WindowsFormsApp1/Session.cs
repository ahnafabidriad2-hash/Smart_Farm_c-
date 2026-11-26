namespace WindowsFormsApp1
{
    // ke login kore ase seta mone rakhe. age profile, order er page gula jant na ke login korse
    public static class Session
    {
        public static int UserId;
        public static string Role = "";
        public static string Username = "";

        public static void Logout()
        {
            UserId = 0;
            Role = "";
            Username = "";
        }
    }
}
