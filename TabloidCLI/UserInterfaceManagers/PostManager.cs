using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class PostManager : IUserInterfaceManager
    {
            private IUserInterfaceManager _parentUI;
            private PostRepository _postRepository;
            private TagRepository _tagRepository;
            private AuthorRepository _authorRepository;
            //private BlogRepository _blogRepository;
            private int _postId;
            private string _connectionString;

        public PostManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _postRepository = new PostRepository(connectionString);
            _authorRepository = new AuthorRepository(connectionString);
            //_blogRepository = new BlogRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
            {
                
                Console.WriteLine($"Post Menu");
                Console.WriteLine(" 1) List All Posts");
                Console.WriteLine(" 2) Post Details");
                Console.WriteLine(" 3) Add Post");
                Console.WriteLine(" 4) Remove Post");
                Console.WriteLine(" 0) Go Back");

                Console.Write("> ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        //View();
                        return this;
                    case "2":
                        //ViewBlogPosts();
                        return this;
                    case "3":
                        Add();
                        return this;
                    case "4":
                        Remove();
                        return this;
                    case "0":
                        return _parentUI;
                    default:
                        Console.WriteLine("Invalid Selection");
                        return this;
                }
            }
        private Post Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Post:";
            }

            Console.WriteLine(prompt);

            List<Post> posts = _postRepository.GetAll();

            for (int i = 0; i < posts.Count; i++)
            {
                Post post = posts[i];
                Console.WriteLine($" {i + 1}) {post.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return posts[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("!--- INVALID SELECTION ---!");
                Console.WriteLine();
                return null;
            }
        }
        private void Add() // This method will allow the user to add a new favorite post to the database
        {
            Console.WriteLine();
            Console.WriteLine("New Post");
            Post post = new Post();

            Console.WriteLine();
            Console.Write("Title: ");
            post.Title = Console.ReadLine(); // This will grab the title from the user and assign it to the post

            Console.WriteLine();
            Console.Write("Url: ");
            post.Url = Console.ReadLine(); // This will grab the url from the user and assign it to the post

            Console.WriteLine();
            Console.WriteLine("Select Author:");
            // In order to select an author, we need to get all the authors from the database
            List<Author> authors = _authorRepository.GetAll(); // Get all the authors from the database
            for (int i = 0; i < authors.Count; i++) // Loop through the list of authors
            {
                Console.WriteLine($"{i + 1} - {authors[i].FirstName} {authors[i].LastName}"); // Display the author's full name
            }
            int selectedAuthorIndex = Convert.ToInt32(Console.ReadLine()) - 1; // Get the index of the selected author
            post.Author = authors[selectedAuthorIndex]; // Assign the selected author to the post

            Console.WriteLine();
            // In order to select a blog, we need to get all the blogs from the database
            //Console.WriteLine("Select Blog:");
            //List<Blog> blogs = _blogRepository.GetAll(); // Get all the blogs from the database
            //for (int i = 0; i < blogs.Count; i++) // Loop through the list of blogs
            //{
            //    Console.WriteLine($"{i + 1} - {blogs[i].Title}"); // Display the blog title
            //}
            //int selectedBlogIndex = Convert.ToInt32(Console.ReadLine()) - 1; // Get the index of the selected blog
            //post.Blog = blogs[selectedBlogIndex]; // Assign the selected blog to the post

            Console.WriteLine();
            // Grab the publish date and assign it to the post
            Console.Write("Publish date (MM-DD-YYYY): ");
            post.PublishDateTime = DateTime.Parse(Console.ReadLine());

            _postRepository.Insert(post);

            Console.WriteLine();
            Console.WriteLine("Post added successfully!");
        }

        private void Edit()
        {
            Console.WriteLine();
            Post postToEdit = Choose("Which post would you like to edit?");
            if (postToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New Title (blank to leave unchanged: ");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                postToEdit.Title = title;
            }

            Console.WriteLine();
            Console.Write("New URL (blank to leave unchanged: ");
            string url = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(url))
            {
                postToEdit.Url = url;
            }


            _postRepository.Update(postToEdit);
        }


        private void Remove()
            {
                Console.WriteLine();
                Post postToDelete = Choose("Which post would you like to remove?");
                if (postToDelete != null)
                {
                    _postRepository.Delete(postToDelete.Id);
                    Console.WriteLine();
                    Console.WriteLine($"!--- {postToDelete.Title} has been DELETED ---!");
                    Console.WriteLine();
                }
            }
        }
    }


