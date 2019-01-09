using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
  public  interface ITest
    {
        Task<List<Int32>> M1();
        Task<List<String>> M2();
    }
}
