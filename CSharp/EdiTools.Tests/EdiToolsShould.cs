using NUnit.Framework;

namespace EdiTools.Tests
{
    // todo: test for invalid file format

    [TestFixture]
    public class EdiToolsShould
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var testData = GetTestData();
            var tmpDir = System.IO.Path.GetTempPath();
            var testFileList = new System.Collections.Generic.List<EdiTools.EdiFile>();
            // file with no breaks (no cr/lf)
            testFileList.Add(CreateTestFile(
                System.IO.Path.Combine(tmpDir, "unwrapped.txt"),
                testData.Replace("\r", "").Replace("\n", "")
                ));

            // file with lf only (no cr)
            testFileList.Add(CreateTestFile(
                System.IO.Path.Combine(tmpDir, "wrapped_Lf.txt"),
                testData.Replace("\r", "")
                ));

            // file with cr/lf
            testFileList.Add(CreateTestFile(
                System.IO.Path.Combine(tmpDir, "wrapped_CrLf.txt"),
                testData
                ));

            _edi835testFiles = testFileList.ToArray();
        }
        [OneTimeTearDown]
        public void Teardown()
        {
            foreach(var f in _edi835testFiles)
            {
                System.IO.File.Delete(f.FullName);
            }
        }
        private string GetTestData()
        {
            return @"ISA*00*          *00*          *ZZ*TheSender      *ZZ*TheReceiver    *190101*1200*<*00501*000000001*0*P*:~
GS*HP*TheSender*TheReceiver*20190101*12000000*1*X*005010X221A1~
ST*835*112233*005010X221A1~
BPR*I*391.05*C*ACH*CCP*01*322271724*DA*203158175*8076853391**01*122000496*DA*7341099666*20120131~
TRN*1*051036622050010*1262721578~
N1*PR*BCBS DISNEY~
N3*POBLADO RD~
N4*LOS ANGELES*CA*9006~
PER*BL*MICHAEL EISNER*TE*7145205060*EX*123*EM*edi@bcbsdisney.com~
PER*IC**UR*www.bcbsdisney.com/policies.html~
N1*PE*UCLA MEDICAL CENTER*XX*1215193883~
LX*1001~
CLP*ABC9001*1*225*200*5*12*1142381711242*22*1~
CAS*CO*45*20~
NM1*QC*1*MOUSE*MICKEY****MI*60345914A~
SVC*HC:98765*150*145~
DTM*472*20120124~
CAS*PR*3*5~
REF*0K*8910~
SVC*HC:26591*75*75~
DTM*472*20120124~
LX*1002~
CLP*ABC9002*1*225*195*10*12*1142381711242*22*1~
CAS*CO*45*20~
NM1*QC*1*DUCK*DONALD****MI*60345914B~
SVC*HC:98765*150*140~
DTM*472*20120124~
CAS*PR*3*10~
REF*0K*8910~
SVC*HC:26591*75*75~
DTM*472*20120124~
PLB*1215193883*20121231*90*3.95~
SE*31*112233~
ST*835*112299*005010X221A1~
BPR*I*391.05*C*ACH*CCP*01*322271724*DA*203158175*8076853391**01*122000496*DA*7341099666*20120131~
TRN*1*051036622050011*1262721578~
N1*PR*BCBS DISNEY~
N3*POBLADO RD~
N4*LOS ANGELES*CA*9006~
PER*BL*MICHAEL EISNER*TE*7145205060*EX*123*EM*edi@bcbsdisney.com~
PER*IC**UR*www.bcbsdisney.com/policies.html~
N1*PE*UCLA MEDICAL CENTER*XX*1215193883~
LX*1001~
CLP*ABC9001*1*225*200*5*12*1142381711242*22*1~
CAS*CO*45*20~
NM1*QC*1*MOUSE*MINNIE****MI*60345914A~
SVC*HC:98765*150*145~
DTM*472*20120124~
CAS*PR*3*5~
REF*0K*8910~
SVC*HC:26591*75*75~
DTM*472*20120124~
LX*1002~
CLP*ABC9002*1*225*195*10*12*1142381711242*22*1~
CAS*CO*45*20~
NM1*QC*1*DOG*GOOFY****MI*60345914B~
SVC*HC:98765*150*140~
DTM*472*20120124~
CAS*PR*3*10~
REF*0K*8910~
SVC*HC:26591*75*75~
DTM*472*20120124~
PLB*1215193883*20121231*90*3.95~
SE*31*112299~
GE*2*1~
GS*HP*TheSender*TheReceiver*20190101*12000000*2*X*005010X221A1~
ST*835*112233*005010X221A1~
BPR*I*391.05*C*ACH*CCP*01*322271724*DA*203158175*8076853391**01*122000496*DA*7341099666*20120131~
TRN*1*051036622050010*1262721578~
N1*PR*BCBS DISNEY~
N3*POBLADO RD~
N4*LOS ANGELES*CA*9006~
PER*BL*MICHAEL EISNER*TE*7145205060*EX*123*EM*edi@bcbsdisney.com~
PER*IC**UR*www.bcbsdisney.com/policies.html~
N1*PE*UCLA MEDICAL CENTER*XX*1215193883~
LX*1001~
CLP*ABC9001*1*225*200*5*12*1142381711242*22*1~
CAS*CO*45*20~
NM1*QC*1*MOUSE*MICKEY****MI*60345914A~
SVC*HC:98765*150*145~
DTM*472*20120124~
CAS*PR*3*5~
REF*0K*8910~
SVC*HC:26591*75*75~
DTM*472*20120124~
LX*1002~
CLP*ABC9002*1*225*195*10*12*1142381711242*22*1~
CAS*CO*45*20~
NM1*QC*1*DUCK*DONALD****MI*60345914B~
SVC*HC:98765*150*140~
DTM*472*20120124~
CAS*PR*3*10~
REF*0K*8910~
SVC*HC:26591*75*75~
DTM*472*20120124~
PLB*1215193883*20121231*90*3.95~
SE*31*112233~
ST*835*112299*005010X221A1~
BPR*I*391.05*C*ACH*CCP*01*322271724*DA*203158175*8076853391**01*122000496*DA*7341099666*20120131~
TRN*1*051036622050011*1262721578~
N1*PR*BCBS DISNEY~
N3*POBLADO RD~
N4*LOS ANGELES*CA*9006~
PER*BL*MICHAEL EISNER*TE*7145205060*EX*123*EM*edi@bcbsdisney.com~
PER*IC**UR*www.bcbsdisney.com/policies.html~
N1*PE*UCLA MEDICAL CENTER*XX*1215193883~
LX*1001~
CLP*ABC9001*1*225*200*5*12*1142381711242*22*1~
CAS*CO*45*20~
NM1*QC*1*MOUSE*MINNIE****MI*60345914A~
SVC*HC:98765*150*145~
DTM*472*20120124~
CAS*PR*3*5~
REF*0K*8910~
SVC*HC:26591*75*75~
DTM*472*20120124~
LX*1002~
CLP*ABC9002*1*225*195*10*12*1142381711242*22*1~
CAS*CO*45*20~
NM1*QC*1*DOG*GOOFY****MI*60345914B~
SVC*HC:98765*150*140~
DTM*472*20120124~
CAS*PR*3*10~
REF*0K*8910~
SVC*HC:26591*75*75~
DTM*472*20120124~
PLB*1215193883*20121231*90*3.95~
SE*31*112299~
GE*2*2~
IEA*1*000000001~";
        }
        private EdiTools.EdiFile CreateTestFile(string path, string content)
        {
            using (var sw = new System.IO.StreamWriter(path))
            {
                sw.Write(content);
            }

            return new EdiTools.EdiFile(path);
        }
        [Test]
        public void TestInterchange()
        {
            foreach (var f in _edi835testFiles)
            {
                Assert.IsNotNull(f);
                Assert.AreEqual(f.Interchange.SenderId, "TheSender");
                Assert.AreEqual(f.Interchange.ReceiverId, "TheReceiver");
                Assert.AreEqual(f.Interchange.ControlNumber, "000000001");

                Assert.AreEqual(f.Interchange.ISA.ISA01, "00");
                Assert.AreEqual(f.Interchange.ISA.ISA02, "          ");
                Assert.AreEqual(f.Interchange.ISA.ISA03, "00");
                Assert.AreEqual(f.Interchange.ISA.ISA04, "          ");
                Assert.AreEqual(f.Interchange.ISA.ISA05, "ZZ");
                Assert.AreEqual(f.Interchange.ISA.ISA06, "TheSender      ");
                Assert.AreEqual(f.Interchange.ISA.ISA07, "ZZ");
                Assert.AreEqual(f.Interchange.ISA.ISA08, "TheReceiver    ");
                Assert.AreEqual(f.Interchange.ISA.ISA09, "190101");
                Assert.AreEqual(f.Interchange.ISA.ISA10, "1200");
                Assert.AreEqual(f.Interchange.ISA.ISA11, "<");
                Assert.AreEqual(f.Interchange.ISA.ISA12, "00501");
                Assert.AreEqual(f.Interchange.ISA.ISA13, "000000001");
                Assert.AreEqual(f.Interchange.ISA.ISA14, "0");
                Assert.AreEqual(f.Interchange.ISA.ISA15, "P");
                Assert.AreEqual(f.Interchange.ISA.ISA16, ":");

                Assert.AreEqual(f.Delimiter.Segment, '~');
                Assert.AreEqual(f.Delimiter.Element, '*');
                Assert.AreEqual(f.Delimiter.Component, ':');
                // TODO: How to test line delimiters i.e. empty string, cr/lf, or lf

                Assert.AreEqual(f.Interchange.IEA.IEA01, "1");
                Assert.AreEqual(f.Interchange.IEA.IEA02, "000000001");
            }
        }

        [Test]
        public void TestFunctionalGroup()
        {
            foreach (var f in _edi835testFiles)
            {
                foreach(var g in f.FunctionalGroups)
                {
                    // these elements are the same in each group
                    Assert.AreEqual(g.GS.GS01, "HP");
                    Assert.AreEqual(g.GS.GS02, "TheSender");
                    Assert.AreEqual(g.GS.GS03, "TheReceiver");
                    Assert.AreEqual(g.GS.GS04, "20190101");
                    Assert.AreEqual(g.GS.GS05, "12000000");                    
                    Assert.AreEqual(g.GS.GS07, "X");
                    Assert.AreEqual(g.GS.GS08, "005010X221A1");
                    Assert.AreEqual(g.GE.GE01, "2");                    
                }

                // GS06 and GE02 should be unique per group in the interchange
                Assert.AreEqual(f.FunctionalGroups[0].GS.GS06, "1");
                Assert.AreEqual(f.FunctionalGroups[0].GE.GE02, "1");
                Assert.AreEqual(f.FunctionalGroups[1].GS.GS06, "2");
                Assert.AreEqual(f.FunctionalGroups[1].GE.GE02, "2");
            }
        }

        [Test]
        public void TestTransactionSet()
        {
            foreach (var f in _edi835testFiles)
            {
                foreach (var g in f.FunctionalGroups)
                {
                    var sets = g.GetTransactionSets();
                    Assert.AreEqual(sets.Length, 2);

                    foreach(var set in sets)
                    {
                        // these elements are the same in each transaction set in the test file
                        Assert.AreEqual(set.ST.ST01, "835");

                        Assert.AreEqual(set.SE.SE01, "31");

                        Assert.AreEqual(set.ID, "835");
                    }

                    // these ID elements are different per transaction sets
                    Assert.AreEqual(sets[0].ST.ST02, "112233");
                    Assert.AreEqual(sets[0].SE.SE02, "112233");
                    Assert.AreEqual(sets[0].ControlNumber, "112233");

                    Assert.AreEqual(sets[1].ST.ST02, "112299");
                    Assert.AreEqual(sets[1].SE.SE02, "112299");
                    Assert.AreEqual(sets[1].ControlNumber, "112299");
                }
            }
        }

        [Test]
        public void TestEdi835()
        {
            foreach (var f in _edi835testFiles)
            {
                foreach (var g in f.FunctionalGroups)
                {
                    var sets = g.GetTransactionSets();

                    // set 1
                    var edi835 = new EdiTools.Edi835(sets[0]);
                    Assert.AreEqual(edi835.SenderBankAccountNumber, "203158175");
                    Assert.AreEqual(edi835.CheckorEFTTraceNumber, "051036622050010");
                    Assert.AreEqual(edi835.Payer, "BCBS DISNEY");
                    Assert.AreEqual(edi835.Payee, "UCLA MEDICAL CENTER");
                    Assert.AreEqual(edi835.TotalActualProviderPaymentAmount, 391.05m);
                    Assert.AreEqual(edi835.TransactionSet.SE.TransactionSegmentCount, edi835.GetSegments().Length);

                    // set 2
                    edi835 = new EdiTools.Edi835(sets[1]);
                    Assert.AreEqual(edi835.SenderBankAccountNumber, "203158175");
                    Assert.AreEqual(edi835.CheckorEFTTraceNumber, "051036622050011");
                    Assert.AreEqual(edi835.Payer, "BCBS DISNEY");
                    Assert.AreEqual(edi835.Payee, "UCLA MEDICAL CENTER");
                    Assert.AreEqual(edi835.TotalActualProviderPaymentAmount, 391.05m);
                    Assert.AreEqual(edi835.TransactionSet.SE.TransactionSegmentCount, edi835.GetSegments().Length);
                }
            }
        }
        #region private
        private EdiTools.EdiFile[] _edi835testFiles;
        #endregion
    }
}
