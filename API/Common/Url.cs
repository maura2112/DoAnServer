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
                public const string ResetPasswordInputCode = "ResetPasswordInputCode";
                public const string ResetNewPassword = "ResetNewPassword";
            }
            public const string Profile = "Profile";
            public const string Update = "Update";
            public const string ChangePassword = "ChangePassword";
            public const string UpdateExperience = "UpdateExperience";
            public const string UpdateEducation = "UpdateEducation";
            public const string UpdateQualification = "UpdateQualification";

            public const string AddPortfolio = "AddPortfolio";
            public const string UpdatePortfolio = "UpdatePortfolio";
            public const string DeletePortfolio = "DeletePortfolio";
            public const string ConvertIntoRecruiter = "ConvertIntoRecruiter"; //it2
            public const string GetUsers = "GetUsers";
            public const string Lock = "Lock";
            public const string Unlock = "Unlock";

            public const string Roles = "Roles";
            public const string GetUser = "GetUser";
        }
        public static class Product
        {
            public const string GetAll = "GetAll";
            public const string Add = "AddProduct";
        }
        public static class Category
        {
            public const string GetAll = "GetAll";
            public const string GetAllPaginaton = "GetAllPaginaton";
            public const string Add = "AddCategory";
            public const string Update = "UpdateCategory";
            public const string Delete = "DeleteCategory";
            public const string GetByStatus = "GetByStatus";
            public const string RestoreDeleted = "RestoreDeleted";

        }

        public static class Blog
        {
            public const string GetAll = "GetAll";
            public const string Create = "Create";
            public const string Update = "Update";
            public const string Delete = "Delete";
            public const string Detail = "Detail";
            public const string Publish = "Publish";
            public const string Gets = "Gets";

        }
        public static class Rating
        {
            public const string Rate = "Rate";
        }
        public static class Project
        {
            public const string GetAll = "GetAll";
            public const string Gets = "Gets";
            //public const string GetByCategory = "GetByCategory";
            public const string GetProjectDetails = "GetProjectDetailsById";
            public const string GetProjectsByUserId = "GetProjectsByUserId";
            public const string Add = "AddProject";
            public const string Update = "UpdateProject";
            public const string Delete = "DeleteProject";
            public const string Filter = "Filter";
            public const string Search = "SearchAdmin";
            public const string SearchHomePage = "SearchHomePage";
            public const string SearchRecruiter = "SearchRecruiter";
            public const string AllStatus = "AllStatus";
            public const string UpdateStatus = "UpdateStatus";
            public const string AcceptBid = "AcceptBid";
            public const string GetByStatus = "GetByStatus";
            public const string Favorite = "Favorite";
            public const string AddFavorite = "AddFavorite";

        }

        public static class Bid
        {
            public const string GetBiddingListByUserId = "GetBiddingListByUserId";
            public const string GetBiddingListByProjectId = "GetBiddingListByProjectId";

            public const string Bidding = "Bidding";
            public const string GetBidByProjectLoggedUser = "GetBidByProjectLoggedUser";
            public const string Update = "UpdateBidding";
            public const string Delete = "DeleteBidding";
            public const string AcceptBidding = "AcceptBidding";
        }

        public static class ProjectSkill
        {
            public const string GetAll = "GetAll";
            public const string GetById = "GetById";
            public const string Add = "Add";
        }

        public static class Skill
        {
            public const string AddSkill = "AddSkill";
            public const string Add = "Add";
            public const string Update = "Update";
            public const string Delete = "Delete";
            public const string Gets = "Gets";
            public const string All = "GetAll";
            public const string AllPublish = "AllPublish";
            public const string GetByCategoryId = "GetByCategoryId";

        }

        public static class Report
        {
            public const string Categories = "Categories";
            public const string Create = "Create";
            public const string Reports = "Reports";
            public const string Approve = "Approve";
        }
        public static class Statistic
        {
            public const string CategoriesPieChart = "CategoriesPieChartData";
            public const string ProjectsPieChart = "ProjectsPieChartData";
            public const string UsersPieChart = "UsersPieChartData";
            public const string NewUserData = "NewUserData";
            public const string StatisticProjects = "StatisticProjects";
            public const string StatisticUsers = "StatisticUsers";
            public const string StatisticSkills = "StatisticSkills";
            
        }
    }
}
