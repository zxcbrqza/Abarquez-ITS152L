using System;
using System.Data.SqlClient;
using System.Globalization;
using BlogDataLibrary.Data;
using BlogDataLibrary.Database;
using BlogDataLibrary.Models;
using Microsoft.Extensions.Configuration;

namespace BlogTestUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlData db = GetConnection();

            // these were the initial codes from the instructions
            /*Register(db);
            Authenticate(db);
            AddPost(db);
            ListPosts(db);
            ShowPostDetails(db);

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();*/

            // display the main menu
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Blogify: A Mini Blog App");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.WriteLine("----------------------------");
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("Register an account");
                    Register(db);
                }
                else if (choice == "2")
                {
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("Login using an existing account");
                    UserModel user = Authenticate(db);
                    if (user != null)
                    {
                        Console.Write("Press enter to continue..");
                        Console.ReadLine();
                        ShowUserMenu(db, user);
                    }
                }
                else if (choice == "3")
                {
                    Console.WriteLine("Exiting the program...");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Press Enter to try again...");
                    Console.ReadLine();
                }
            }
        }

        private static void ShowUserMenu(SqlData db, UserModel user)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Welcome, {user.UserName}");
                Console.WriteLine("1. Add Post");
                Console.WriteLine("2. List Posts");
                Console.WriteLine("3. Show Post Details");
                Console.WriteLine("4. Logout");
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    bool repeatAdd;
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("Add a new post");
                    do
                    {
                        AddPost(db);
                        Console.WriteLine("----------------------------");
                        Console.Write("Do you want to add another post? [y/n]: ");
                        repeatAdd = Console.ReadLine()?.ToLower() == "y";
                    } while (repeatAdd);

                    Console.Write("Press Enter to go back...");
                    Console.ReadLine();
                }
                else if (choice == "2")
                {
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("All Posts");
                    ListPosts(db);
                    Console.Write("Press enter to go back..");
                    Console.ReadLine();
                }
                else if (choice == "3")
                {
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("Post Details");
                    ShowPostDetails(db);
                    Console.Write("Press enter to go back..");
                    Console.ReadLine();
                }
                else if (choice == "4")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Press Enter to try again...");
                    Console.ReadLine();
                }
            }
        }

        private static UserModel GetCurrentUser(SqlData db)
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            UserModel user = db.Authenticate(username, password);

            return user;
        }

        public static UserModel Authenticate(SqlData db)
        {
            UserModel user = GetCurrentUser(db);

            if (user == null)
            {
                // I added the feature that the user must press enter to try again
                Console.WriteLine("Invalid credentials. Press Enter to try again...");
                Console.ReadLine();
            }
            else
            {
                // Changed it into a successful logged in message
                Console.WriteLine($"Successfully logged in using {user.UserName}.");
            }
            return user;
        }

        public static void Register(SqlData db)
        {
            string username;
            string password;
            string firstName;
            string lastName;

            while (true)
            {
                // Added error handling. Made sure the user cannot use an existing username
                Console.Write("Enter new username: ");
                username = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.WriteLine("Username cannot be empty or whitespace.");
                    Console.WriteLine("Press Enter to try again..");
                    Console.ReadLine();
                    continue;
                }

                // Check if the username already exists
                if (db.UsernameExists(username))
                {
                    Console.WriteLine("Username already exists. Please choose a different username.");
                    Console.WriteLine("Press Enter to try again...");
                    Console.ReadLine();
                    continue;
                }

                break;
            }

            while (true)
            {
                Console.Write("Enter new password: ");
                password = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("Password cannot be empty or whitespace.");
                    Console.WriteLine("Press Enter to try again..");
                    Console.ReadLine();
                    continue;
                }
                break;
            }

            while (true)
            {
                Console.Write("Enter new first name: ");
                firstName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(firstName))
                {
                    Console.WriteLine("First name cannot be empty.");
                    Console.WriteLine("Press Enter to try again..");
                    Console.ReadLine();
                    continue;
                }
                break;
            }

            while (true)
            {
                Console.Write("Enter new last name: ");
                lastName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(lastName))
                {
                    Console.WriteLine("Last name cannot be empty.");
                    Console.WriteLine("Press Enter to try again..");
                    Console.ReadLine();
                    continue;
                }
                break;
            }

            db.Register(username, firstName, lastName, password);
            Console.WriteLine("User registered successfully. Press Enter to continue...");
            Console.ReadLine();
        }

        private static void AddPost(SqlData db)
        {
            // Error handling to ensure correct data
            // if invalid input or nonexistent account, an error message will show
            UserModel user = GetCurrentUser(db);
            if (user == null)
            {
                Console.WriteLine("User authentication failed. Please try again.");
                return;
            }

            // if title is blank or only white spaces, an error message will show
            Console.Write("Title: ");
            string title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Title cannot be empty. Please try again.");
                return;
            }

            // if body is blank or only white spaces, an error message will show
            Console.WriteLine("Write body: ");
            string bodyc = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(bodyc))
            {
                Console.WriteLine("Body cannot be empty. Please try again.");
                return;
            }

            PostModel post = new PostModel
            {
                Title = title,
                Body = bodyc,
                DateCreated = DateTime.Now,
                UserId = user.Id
            };
            db.AddPost(post);
            Console.WriteLine("Post added successfully. Press enter to continue");
            Console.ReadLine();
        }

        private static void ListPosts(SqlData db)
        {
            List<ListPostModel> posts = db.ListPosts();

            foreach (ListPostModel post in posts)
            {
                Console.WriteLine($"{post.Id}. Title: {post.Title} by {post.UserName} [{post.DateCreated.ToString("yyyy-MM-dd")}]");
                // Changed the original code to first check if the body text is at least 20 characters long
                // Console.WriteLine($"{post.Body.Substring(0, 20)}...");
                string postPreview = post.Body.Length > 20 ? post.Body.Substring(0, 20) : post.Body;
                Console.WriteLine($"{postPreview}...");
                Console.WriteLine();
            }
        }
        private static void ShowPostDetails(SqlData db)
        {
            // I added an exception handling if the user inputs an invalid post ID
            Console.Write("Enter a post ID: ");
            if(!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid input. Please enter a valid psot ID.");
                return;
            }

            ListPostModel post = db.ShowPostDetails(id);
            if(post == null)
            {
                Console.WriteLine($"No post found with ID {id}");
                return;
            }

            //int id = Int32.Parse(Console.ReadLine());
            //ListPostModel post = db.ShowPostDetails(id);

            Console.WriteLine(post.Title);
            Console.WriteLine($"by {post.FirstName} {post.LastName} [{post.UserName}]");

            Console.WriteLine();

            Console.WriteLine(post.Body);

            Console.WriteLine(post.DateCreated.ToString("MMM d yyyy"));
        }
        static SqlData GetConnection()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = builder.Build();
            ISqlDataAccess dbAccess = new SqlDataAccess(config);
            SqlData db = new SqlData(dbAccess);

            return db;
        }
    }
}
