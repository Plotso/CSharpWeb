﻿namespace DemoApp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using SIS.HTTP;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Models;
    using SIS.HTTP.Response;
    using SIS.MvcFramework;

    public class Startup : IMvcApplication
    {
        private const string TestUsernameCookie = "UsernameData";
        public void Configure(IList<Route> routeTable)
        {
            routeTable.Add(new Route(HttpMethodType.Get, "/", Index));
            routeTable.Add(new Route(HttpMethodType.Post, "/Tweets/Create", CreateTweet));
            //routeTable.Add(new Route(HttpMethodType.Get, "/users/login", Login));
            //routeTable.Add(new Route(HttpMethodType.Post, "/users/login", DoLogin));
            //routeTable.Add(new Route(HttpMethodType.Get, "/contact", Contact));
            routeTable.Add(new Route(HttpMethodType.Get, "/favicon.ico", FavIcon));
        }

        public void ConfigureServices()
        {
            var db = new ApplicationDbContext();
            db.Database.EnsureCreated();
        }
        
        
        private static HttpResponse Index(HttpRequest request)
        {
            var username = request.SessionData.ContainsKey(TestUsernameCookie)
                ? request.SessionData[TestUsernameCookie]
                : "Anonymous";
            
            var db = new ApplicationDbContext();
            var tweetsProjection = db.Tweets.Select(x => new
            {
                x.Creator,
                x.Content,
                x.CreationDate
            });
            var tweets = tweetsProjection.ToList();
            
            var html = new StringBuilder();

            html.AppendLine($"<h1> Home Page. Hello, {username} </h1> <br>");
            html.AppendLine("<div>");
            html.AppendLine("<form action='/Tweets/Create' method='post'>Username: <input type='text' name='creator' /><br>Tweet: <textarea name='tweetText'></textarea><input type='submit' /></form> <br<");
            html.AppendLine("</div>");
            html.AppendLine("<table><tr><th>Date</th><th>Creator</th><th>Content</th></tr>");
            foreach (var tweet in tweets)
            {
                html.AppendLine(
                    $"<tr><td>{tweet.CreationDate}</td><td>{tweet.Creator}</td><td>{tweet.Content}</td></tr>");
            }
            html.AppendLine("</table>");
            
            return new HtmlResponse(html.ToString());
        }
        
        // /Tweets => Index
        // /Tweets/Create => Create
        private static HttpResponse CreateTweet(HttpRequest request)
        {
            var db = new ApplicationDbContext();
            db.Tweets.Add(new Tweet
            {
                Creator = request.FormData["creator"],
                Content = request.FormData["tweetText"],
                CreationDate = DateTime.UtcNow
            });
            db.SaveChanges();
            return new RedirectResponse();
        }
        
        private static HttpResponse Login(HttpRequest request)
        {
            request.SessionData[TestUsernameCookie] = "Pesho";
            return new HtmlResponse("<h1> Login Page </h1>");
        }
        
        private static HttpResponse DoLogin(HttpRequest request)
        {
            return new HtmlResponse("<h1> Login Form to be added </h1>");
        }
        
        private static HttpResponse Contact(HttpRequest request)
        {
            return new HtmlResponse("<h1> Contact Page </h1>");
        }
        
        private static HttpResponse FavIcon(HttpRequest request)
        {
            var byteContent = File.ReadAllBytes("wwwroot/favicon.ico");
            return new FileResponse(byteContent, "image/x-icon");
        }
    }
}