Console.WriteLine("Hello, I am a calculator application.");
Console.WriteLine("Please press Cntrl-C to stop this application.");
Console.WriteLine(Environment.NewLine);

var calculatorService = new CalculatorService.CalculatorService();
Console.WriteLine(calculatorService.FunctionalityDescription);

var stopApplication = false;
while (!stopApplication)
{
    try
    {
        var addends = Console.ReadLine();
        var sum = calculatorService.Add(addends);
        Console.WriteLine($"The sum is {sum}");
        Console.WriteLine(Environment.NewLine);
    }
    catch (Exception e)
    {
        Console.WriteLine($"ERROR: {e.Message}");
        Console.WriteLine(Environment.NewLine);
    }
}
