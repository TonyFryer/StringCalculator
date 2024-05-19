Console.CancelKeyPress += (sender, e) =>
{
    e.Cancel = true;
    Console.WriteLine("Control-C was pressed, terminating application.");
    Environment.Exit(0);
};

Console.WriteLine("Hello, I am a calculator application.");
Console.WriteLine("Please press Ctrl-C to stop this application.");
Console.WriteLine("--------------------------------------------------------------");

var calculatorService = new CalculatorService.CalculatorService();
Console.WriteLine(calculatorService.FunctionalityDescription);
Console.WriteLine("--------------------------------------------------------------");

while (true)
{
    try
    {
        Console.WriteLine("Enter values for addition...");
        var addends = Console.ReadLine();
        var sum = calculatorService.Add(addends);
        Console.WriteLine($"The sum is {sum}");
        Console.WriteLine();
    }
    catch (Exception e)
    {
        Console.WriteLine($"ERROR: {e.Message}");
        Console.WriteLine();
    }
}
