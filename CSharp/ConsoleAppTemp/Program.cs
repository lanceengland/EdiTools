using EdiTools;
using EdiTools.Edi837;
using System;

internal class Program
{
    static void Main(string[] args)
    {
		try
		{
			var f = new EdiFile(@"C:\Users\LanceEngland\source\repos\EdiTools2\samples\sample837-P.txt");

			var t = f.FunctionalGroups[0].TransactionSets[0];

			//var hierarchy = new EdiTools.Edi837.DocumentHierarchy(t.Segments);

			var foo = new EdiTools.Edi837.TransactionSet(t);

			Console.WriteLine("fin");
		}
		catch (System.Exception e)
		{
			Console.WriteLine(e.ToString());
		}
		finally 
		{
			Console.Write("Press ENTER to close...");
			Console.ReadLine();
		}
    }
}
