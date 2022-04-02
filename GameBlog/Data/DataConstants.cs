namespace GameBlog.Data
{
    public class DataConstants
    {
        public class Article
        {
            public const int TitleMinLength = 8;
            public const int TitleMaxLength = 100;

            public const int ContentMinLength = 10;
            public const int ContentMaxLength = 5000;
        }

        public class Comment
        {
            public const int CommentMaxLength = 800;
            public const int CommentMinLength = 1;
        }

        public class User
        {
            public const int UserNameMaxLength = 60;
            public const int UserNameMinLength = 4;

            public const int PasswordMaxLength = 32;
            public const int PasswordMinLength = 6;

            public const int FullNameMaxLength = 64;
            public const int FullNameMinLength = 8;

            public const int MinReputation = 0;
        }

        public class Game
        {
            public const int NameMaxLength = 64;
            public const int NameMinLength = 2;

            public const int DescriptionMaxLength = 300;
            public const int DescriptionMinLength = 30;

            public const int GenreMaxLength = 30;
            public const int GenreMinLength = 3;

        }

        public class Rating
        {
            public const int MinRating = 0;
            public const int MaxRating = 10;
        }

        public class Role
        {
            public const string Administrator = "Administrator";
        }
    }
}
