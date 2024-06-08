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
                public const string External = "External";
                public const string ResetPassword = "ResetPassword";
                public const string ResetPasswordInputCode= "ResetPasswordInputCode";
                public const string ResetNewPassword = "ResetNewPassword";
            }
        }
        public static class Product
        {
            public const string GetAll = "GetAll";
            public const string Add = "AddProduct";
        }
        public static class Category
        {
            public const string GetAll = "GetAll";
            public const string Add = "AddCategory";
        }
        public static class Project
        {
            public const string GetAll = "GetAll";
            //public const string GetByCategory = "GetByCategory";
            public const string GetProjectDetails = "GetProjectDetailsById";
            public const string GetProjectsByUserId = "GetProjectsByUserId";
            public const string Add = "AddProject";
            public const string Update = "UpdateProject";
            public const string Delete = "DeleteProject";
            public const string Filter = "Filter";
            public const string Search = "Search";



        }

        public static class Bid
        {
            public const string GetBiddingListByUserId = "GetBiddingListByUserId";
            public const string GetBiddingListByProjectId = "GetBiddingListByProjectId";

            public const string Bidding = "Bidding";
            //public const string GetDetailBidById = "GetDetailBidById";
            public const string Update = "UpdateBidding";
            public const string Delete = "DeleteBidding";
        }

        public static class ProjectSkill
        {
            public const string GetAll = "GetAll";
            public const string GetById = "GetById";
            public const string Add = "Add";
        }

        public static class Skill
        {
            public const string Add = "Add";
            public const string GetByCategoryId = "GetByCategoryId";

        }
    }
}
