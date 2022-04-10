using Cesium.Test.Framework;
using Yoakke.SynKit.C.Syntax;

namespace Cesium.Parser.Tests.ParserTests;

public class StatementParserTests : ParserTestBase
{
    private static Task DoTest(string source)
    {
        var lexer = new CLexer(source);
        var parser = new CParser(lexer);

        var result = parser.ParseStatement();
        Assert.True(result.IsOk, result.GetErrorString());

        var serialized = JsonSerialize(result.Ok.Value);
        return Verify(serialized, GetSettings());
    }

    [Fact]
    public Task ReturnArithmetic() => DoTest("return 2 + 2 * 2;");

    [Fact]
    public Task ReturnVariableArithmetic() => DoTest("return x + 2 * 2;");

    [Fact]
    public Task CompoundStatementWithVariable() => DoTest("{ int x = 0; }");

    [Fact]
    public Task BitArithmetic() => DoTest("return ~1 << 2 >> 3 | 4 & 5 ^ 6;");

    [Fact]
    public Task IfStatement() => DoTest("if (1) { int x = 0; }");

    [Fact]
    public Task IfElseStatement() => DoTest("if (1) { int x = 0; } else { int y = 1; }");

    [Fact]
    public Task NestedIfs() => DoTest(@"
if (1)
    if (2) { 
        int x = 0;
    } else {
        int y = 1;
    } 
");

    [Fact]
    public Task RelationalOperators() => DoTest("return 1 > 2 < 4 <= 5 >= 6;");

    [Fact]
    public Task EqualityOperators() => DoTest("return 1 == 2 != 3;");

    [Fact]
    public Task LogicalAndOperator() => DoTest("return 1 && 2;");

    [Fact]
    public Task LogicalOrOperator() => DoTest("return 1 || 2;");
}
