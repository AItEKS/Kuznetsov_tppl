namespace Pascal.Tests
{
    public class ProgramExecutionTests
    {
        [Fact]
        public void Main_ExecutesAndPrintsCorrectOutput()
        {
            var originalConsoleOut = Console.Out;
            var stringWriter = new StringWriter();

            try
            {
                Console.SetOut(stringWriter);

                Program.Main(new string[0]);

                string capturedOutput = stringWriter.ToString();

                Assert.StartsWith("Program(Name=Default, Compound(Assign(Var(Name=y) := Number(Token(Type=Number, Value='2')')), Compound(Assign(Var(Name=a) := Number(Token(Type=Number, Value='3')')), Assign(Var(Name=b) := BinOp+(BinOp+(Number(Token(Type=Number, Value='10')'), Var(Name=a)), BinOp/(BinOp*(Number(Token(Type=Number, Value='10')'), Var(Name=y)), Number(Token(Type=Number, Value='4')')))), Assign(Var(Name=c) := BinOp-(Var(Name=a), Var(Name=b))), NoOp), Assign(Var(Name=x) := Number(Token(Type=Number, Value='11')')), NoOp))", capturedOutput);

                Assert.Contains("Таблица символов после выполнения:", capturedOutput);

                Assert.Contains("y = 2", capturedOutput);
                Assert.Contains("x = 11", capturedOutput);

                Assert.Contains("a =", capturedOutput);
                Assert.Contains("b =", capturedOutput);
                Assert.Contains("c =", capturedOutput);
            }
            finally
            {
                Console.SetOut(originalConsoleOut);
                stringWriter.Dispose();
            }
        }
    }
}