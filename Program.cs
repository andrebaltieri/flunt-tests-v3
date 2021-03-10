using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MeuTeste
{
    class Program
    {
        static void Main(string[] args)
        {
            var hand = new Handler();
            hand.Handle();

            Console.WriteLine("Hello World!");
        }
    }

    public class Handler : Notifiable
    {
        public void Handle()
        {
            var person = new Person
            {
                Name = "ANdré",
                LastName = "Baltieri"
            };

            person.Validate(this);
            foreach (var item in Notifications)
            {
                Console.WriteLine(item.Message);
            }
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public string LastName { get; set; }

        public void Validate(Notifiable context)
        {
            context
                .Requires(
                    Name.IsGreaterThan(99),
                    LastName.IsGreaterThan(3));
        }
    }

    public class Notification
    {
        public Notification(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public Notification()
        {
        }

        public string Key { get; set; }
        public string Message { get; set; }
    }

    public class Notifiable
    {
        public Notifiable()
        {
            Notifications = new List<Notification>();
        }

        public List<Notification> Notifications { get; set; }

        public Notifiable Requires(params (bool, Notification)[] notifications)
        {
            foreach (var item in notifications)
            {
                if (item.Item1)
                    Notifications.Add(item.Item2);
            }

            return this;
        }
    }

    public static class IsGreaterThanExtensions
    {
        public static (bool, Notification) IsGreaterThan(this string item, int comparer, string key = "KEY",
            string error = "ERR")
        {
            return item.Length > comparer ? (true, null) : (false, new Notification(key, error));
        }
    }
}