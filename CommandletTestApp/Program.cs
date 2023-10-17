using var reader = new StreamReader("output.txt");
var line = reader.ReadLine();
while (line is not null)
{
	Console.WriteLine(line);
	Thread.Sleep(10);
	line = reader.ReadLine();
}
