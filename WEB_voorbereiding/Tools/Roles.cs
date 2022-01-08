using System.Collections.Generic;

namespace WEB_voorbereiding.Tools
{
    public static class Roles
    {


        //dit zijn gewoon strings
        public const string Lector = "Lector";
        public const string Student = "Student";
        public const string Admin = "Admin";

        public static IEnumerable<string> IERoles => GetRoles();

        private static List<string> GetRoles()
        {
            List<string> lst = new List<string>
                {
                    Lector,
                    Student,
                    Admin
                };

            return lst;
        }
    }
}

