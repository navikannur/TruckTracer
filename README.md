Program Name : NavigateSimulator

Author : Navaneeth puthiyandi

Date : 28/05/2019

How to run the program.
1.	Extract TruckTrace.zip in your computer.
2.	Note that you do need to have the dotnet core runtime installed: https://www.microsoft.com/net/download/dotnet-core/2.1
3.	Go to the following folder
            \TruckTrace\ConsoleApp4\bin\Debug\netcoreapp2.1\
4.	Open command prompt execute the program as follows.

dotnet NavigateSimulator.dll <Filename with extension and full path> <custom delay> <debugmode Y or N>    

Example : dotnet NavigateSimulator.dll C:\TEMP\route_example.csv 2 Y

Parameters
Parameter 1 : csv input filename with extension and full path (mandatory parameter)
Parameter 2 : custom delay in seconds (default = 1)
Parameter 3 : if you need to see printed outputs on screen (default = Y)


Program application extension
1. NavigateSimulator.dll

ProgramSolution file
1.NavigateSimulator.sln

Program class files
1.	Navigation.cs
2.	PostRequest.cs
3.	Program.cs
4.	ReadInputCsv.cs
5.	RouteInfo.cs

Dependencies
1.	NuGet – Newtonsoft.Json

Assumptions
1.	Csv file format is fixed.
2.	Csv file is correct and is no need to be validated or reformatted.
3.	Records starting with ‘T’ is only considered for output.

Equations used

1. Velocity equation is chosen : V^2 = U^2 + 2as
            ‘v’ is velocity to be calculated
            ‘u’ is current velocity before accelerating
            ‘a' is the acceleration
            ‘s’ is the distance interval.

2. Calculations are done in meteres and seconds , but to match the speed limit and to output I have converted it to km/hr
3. Speed limit in m/s^2 = SpeedLimit * 0.277777 
4. The cordinates which are in the file but on short intervals than the delay time, is skipped. if the intervals is multiple times              higher than the delay then the cordinates are repeated on the regular delay.

Known Errors
1.	Http post request is not working on the webmap. Hence the outputs are written to the standard output screen.
