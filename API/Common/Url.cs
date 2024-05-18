namespace API.Common
{
    public static class Url
    {
        public static class User
        {
            public static class Identity
            {
                public const string Login = "Login";
                public const string Logout = "Logout";
                public const string Register = "Register";
            }
        }
        public static class Product
        {
            public const string GetAll = "GetAll";
            public const string Add = "AddProduct";
        }

        public static class Project
        {
            public const string GetAll = "GetAll";
            public const string GetByCategory = "GetByCategory";
            public const string GetProjectDetails = "GetProjectDetailsById";
            public const string Add = "AddProject";
        }
    }
}
