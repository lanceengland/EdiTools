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
			var f = new EdiFile(args[0]);

			Console.WriteLine("Original");
			Console.WriteLine(f.Interchange.Text);
            Console.WriteLine();

            Console.WriteLine("Unwrap");
            Console.WriteLine(f.Interchange.Unwrap());
            Console.WriteLine();

            var t = f.FunctionalGroups[0].TransactionSets[0];
			//var segs = EdiTools.Utilities.FileOperations.GetEdi837SegmentsForPatientControlNumber(f, "1001A");

			foreach(var text in FileOperations.Split837ByClaims(f, "clm12345678"))
            {
				var deidentified = DataDeidentification.Deidentify837(text);


				// just for display purposes
				Console.WriteLine(deidentified);
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
