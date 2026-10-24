using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Timers;

namespace EventManagementDemo.EventManagement
{
    internal class Event
    {
        //public delegate void delgName<T>(T m);

        //public void Display<Type>(Type msg)
        //{
        //    Console.WriteLine(msg);
        //}
        //public void Run()
        //{
        //    delgName<string> a = Display;
        //    Console.WriteLine("Custom - Action");
        //    a("hello");
        //}

        public delegate void delgName(string m);

        public void Display(string msg)
        {
            Console.WriteLine(msg);
        }
        public void Run()
        {
            delgName a = Display;
            Console.WriteLine("Custom - Action");
            a("hello");
            Action<string> x = Display;
            Console.WriteLine("predifined - Action");
            x("Hello");
        }

        

        public delegate int delgName2(int x, int y);
        public int Add(int a, int b)
        {
            return a + b;
        }
        public void Run1()
        {
            delgName2 add = Add;
            int x = add(5, 6);
            Console.WriteLine("Custom - Function");
            Console.WriteLine(x);
            Func<int, int,int> a = Add;
            Console.WriteLine("Predefined - Function");
            Console.WriteLine(a(2, 3));
        }

        public delegate Boolean delgName3(int x);

        bool IsValid(int x)
        {
            if (x > 18)
            {
                return true;
            }
            else
            { 
                return false;
            }
        }

        public void Run3()
        {
            delgName3 check = IsValid;
            Console.WriteLine("Custom - Predicate");
            Console.WriteLine(check(20));
            Predicate<int> check1 = IsValid;
            Console.WriteLine("Predefined - Predicate");
            Console.WriteLine(check1(20));
        }

        public void CheckString(string s, Predicate<string> condition)
        {
            Console.WriteLine(condition(s));
        }
        public bool IsPalindrome(string s)
        {
            return s == new string(s.Reverse().ToArray());
        }

        public void Run4()
        {
            Console.WriteLine("pass the body of the method as as an argumnet using lambda expression");
            CheckString("malayalam", IsPalindrome);
            CheckString("Hello", (s) => s.Distinct().Count() == s.Length);
            CheckString("programming", s => s.Length > 8);
            CheckString("level", s => s[0] == s[s.Length - 1]);
        }

        public void CheckValue<T>(T t,Predicate<T> condition)
        {
            Console.WriteLine(condition(t));
        }
        public void Run5()
        {
            CheckValue("mam",s => s == new string(s.Reverse().ToArray()));
            CheckValue("1231", s => s[0] == s[s.Length - 1]);
            CheckValue(new int[] { 1, 2, 3, 4, 1 }, s => s[0] == s[s.Length-1]);
        }


    }
}
