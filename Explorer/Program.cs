using System;
using System.IO;

namespace Explorer
{
    class Program
    {

        static void Main()
        {
            var explorerTree =  new ExplorerTree();
            explorerTree.Start();
            Console.ReadKey();
        }
    }
}
