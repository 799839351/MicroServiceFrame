using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Test : ITest
    {
        public async Task<List<Int32>> M1()
        {
            Thread.Sleep(200);
            Console.WriteLine($"进入方法1的线程id是{Thread.CurrentThread.ManagedThreadId}");
            return await Task.Run(() =>
            {
               
                Console.WriteLine($"进入方法1的线程id是{Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"进入方法1的线程Taskid是{Task.CurrentId}");
                return new List<Int32> { 1, 2, 3 };
            });
        }

        public async Task<List<string>> M2()
        {
            Console.WriteLine($"进入方法2的线程id是{Thread.CurrentThread.ManagedThreadId}");
            return await Task.Run(() => {
                Console.WriteLine($"进入方法2的线程id是{Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"进入方法2的线程Taskid是{Task.CurrentId}");
                return new List<string> { "1", "2", "3" }; }
            );
        }
    }
}
