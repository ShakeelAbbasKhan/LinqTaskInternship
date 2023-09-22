﻿using LinqTask.DataModel;
using LinqTask.Services;
using System;
using System.Collections.Generic;
using System.Linq;
namespace LinqTask
{
    class Program
    {
        private readonly IService iservice;

        public Program(IService _iservice)
        {
            iservice = _iservice;
        }

        static void Main(string[] args)
        {
            var service = new Service();
            var internshipManager = new InternshipService(service);

            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Add Internship");
                Console.WriteLine("2. Search Internships");
                Console.WriteLine("3. Exit");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        internshipManager.AddingData();
                        break;
                    case "2":
                        internshipManager.Searching();
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}