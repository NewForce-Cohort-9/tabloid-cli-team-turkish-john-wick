using System;
using System.Collections.Generic;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    public class BlogManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private BlogRepository _blogRepository;
        private string _connectionString;

        public BlogManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _blogRepository = new BlogRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Blog Menu");
            Console.WriteLine(" 1) List Blogs");
            Console.WriteLine(" 2) Blog Details");
            Console.WriteLine(" 3) Add Blog");
            Console.WriteLine(" 4) Edit Blog");
            Console.WriteLine(" 5) Remove Blog");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;

                //after a blog is selected with choose, and it is confirmed NOT null, the BlogDetailManager method is called
                case "2":
                    Blog blog = Choose();
                    if (blog == null)
                    {
                        return this;
                    }
                    else
                    {
                        return new BlogDetailManager(this, _connectionString, blog.Id);
                    }
                case "3":
                    Add();
                    return this;
                case "4":
                    Edit();
                    return this;
                case "5":
                    Remove();
                    return this;
                case "0":
                    //returns user to main menu
                    return _parentUI;
                default:
                    //return "error" string if selection invalid
                    Console.WriteLine("!--- INVALID SELECTION ---!");
                    return this;
            }
        }

        // This method lists all of the Blogs
        private void List()
        {
            Console.WriteLine();
            List<Blog> blogs = _blogRepository.GetAll();
            foreach (Blog blog in blogs)
            {
                Console.WriteLine(blog);
            }
            Console.WriteLine();
        }

        // This method lets the user select a Blog from the list 
        // note: we are adding 1 to index of items, then subtracting 1 to get the correct index back before utilizing user input
        private Blog Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Blog:";
            }

            Console.WriteLine(prompt);

            List<Blog> blogs = _blogRepository.GetAll();

            for (int i = 0; i < blogs.Count; i++)
            {
                Blog blog = blogs[i];
                Console.WriteLine($" {i + 1}) {blog.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return blogs[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("!--- INVALID SELECTION ---!");
                Console.WriteLine();
                return null;
            }
        }

        // This method lets the user create a blog object to be inserted into the blog repo
        private void Add()
        {
            Console.WriteLine();
            Console.WriteLine("New Blog");
            Blog blog = new Blog();

            Console.WriteLine();
            Console.Write("Title: ");
            blog.Title = Console.ReadLine();

            Console.WriteLine();
            Console.Write("Url: ");
            blog.Url = Console.ReadLine();

            _blogRepository.Insert(blog);
        }

        // This method allows the user to edit (update) a blog entry
        private void Edit()
        {
            Console.WriteLine();
            Blog blogToEdit = Choose("Which blog would you like to edit?");
            if (blogToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New Title (blank to leave unchanged: ");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                blogToEdit.Title = title;
            }

            Console.WriteLine();
            Console.Write("New URL (blank to leave unchanged: ");
            string url = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(url))
            {
                blogToEdit.Url = url;
            }


            _blogRepository.Update(blogToEdit);
        }

        // This method allows the user to remove (delete) a blog entry
        private void Remove()
        {
            Console.WriteLine();
            Blog blogToDelete = Choose("Which blog would you like to remove?");
            if (blogToDelete != null)
            {
                _blogRepository.Delete(blogToDelete.Id);
                Console.WriteLine();
                Console.WriteLine($"!--- {blogToDelete.Title} has been DELETED ---!");
                Console.WriteLine();
            }
        }
    }
}