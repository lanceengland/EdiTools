using EdiTools;
using EdiTools.Utilities;
using System;
using System.CodeDom;

internal class Program
{
    static void Main(string[] args)
    {
		try
		{
			var f = new EdiFile(@"C:\Users\LanceEngland\source\repos\EdiTools2\samples\sample837-P.txt");

			var t = f.FunctionalGroups[0].TransactionSets[0];
			//var segs = EdiTools.Utilities.FileOperations.GetEdi837SegmentsForPatientControlNumber(f, "1001A");

			foreach(var text in FileOperations.Split837ByClaims(f, "clm12345678"))

            {
				Console.WriteLine(text);
				Console.WriteLine();
			}
        }
		catch (System.Exception e)
		{
			Console.WriteLine(e.ToString());
		}
		finally 
		{
            Console.WriteLine("fin");
			Console.WriteLine();
            Console.Write("Press ENTER to close...");
			Console.ReadLine();
		}
    }
}
