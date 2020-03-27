Describe 'EdiTools' {

    $testData = @"
ISA*00*          *00*          *ZZ*TheSender      *ZZ*TheReceiver    *190101*1200*<*00501*000000001*0*P*:~
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
IEA*1*000000001~
"@
    $null = [System.IO.File]::WriteAllText("$TestDrive\wrapped.txt", $testData.Replace("`r", "").Replace("`n", ""))
    $null = [System.IO.File]::WriteAllText("$TestDrive\unwrapped_Lf.txt", $testData.Replace("`r", ""))
    $null = [System.IO.File]::WriteAllText("$TestDrive\unwrapped_CrLf.txt", $testData)

    Context 'Get-EdiFile'{
        It 'takes a single parameter' {
            $f = Get-EdiFile "$TestDrive\wrapped.txt"
            $f.Count | Should Be 1 
        }

        It 'parses files with no line-breaks' {
            $f = Get-EdiFile "$TestDrive\wrapped.txt"
            $f.Count | Should Be 1 
        }
        
        It 'parses files with LF line-breaks' {
            $f = Get-EdiFile "$TestDrive\unwrapped_Lf.txt"
            $f.Count | Should Be 1 
        }

        It 'parses files with CR/LF line-breaks' {
            $f = Get-EdiFile "$TestDrive\unwrapped_CrLf.txt"
            $f.Count | Should Be 1 
        }

        It 'takes an array parameter' {
            $f = Get-EdiFile "$TestDrive\wrapped.txt", "$TestDrive\unwrapped_Lf.txt"
            $f.Count | Should Be 2
        }

        It 'parses each file only once' {
            $f = Get-EdiFile "$TestDrive\wrapped.txt", "$TestDrive\wrapped.txt"
            $f.Count | Should Be 1
        }

        It 'accepts Get-Children as piped input' {
            Get-ChildItem -Path $TestDrive | Get-EdiFile -OutVariable f
            $f.Count | Should Be 3
        }

        It 'accepts Select-String as piped input' {
            Select-String -Path $TestDrive\*.* -Pattern "ST*835" -SimpleMatch -AllMatches | Get-EdiFile -OutVariable f
            $f.Count | Should Be 3
        }
    }

    Context 'Get-EdiTransactionSet'{
        It 'placeholder' {
            1 + 1 | Should Be 2 
        }
    }

    Context 'Get-Edi835'{
        It 'placeholder' {
            1 + 1 | Should Be 2 
        }
    }
}
