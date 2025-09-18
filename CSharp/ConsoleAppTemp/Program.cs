﻿using EdiTools;
using EdiTools.Utilities;
using System;

internal class Program
{
    static void Main(string[] args)
    {
		try
		{
			var f = new EdiFile(@"C:\Users\LanceEngland\source\repos\EdiTools2\samples\sample837-P.txt");

			var t = f.FunctionalGroups[0].TransactionSets[0];
			var segs = EdiTools.Utilities.FileOperations.GetEdi837SegmentsForPatientControlNumber(f, "1001A");

            Console.WriteLine(segs.CombineSegmentText());

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
