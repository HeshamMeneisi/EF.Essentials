namespace EF.Essentials.Authorization
{
    public static partial class Permissions
    {
        public static partial  class Claims
        {
            public const string Grant = "claims.grant";
        }

        public static partial  class Interviews
        {
            public const string ManageInterviews = "interviews";
        }

        public static partial  class Organization
        {
            public const string Create = "organization.create";
            public const string ChangeOwner = "organization.change_owner";
            public const string ViewSubscription = "organization.view_subscription";
            public const string ViewStats = "organization.view_stats";
        }

        public static partial  class Curation
        {
            public const string Start = "curation.start";
        }
    }
}
