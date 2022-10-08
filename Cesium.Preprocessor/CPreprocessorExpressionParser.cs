using Cesium.Core;
using Yoakke.SynKit.Lexer;
using Yoakke.SynKit.Parser.Attributes;

namespace Cesium.Preprocessor;

using ICPreprocessorToken = IToken<CPreprocessorTokenType>;

[Parser(typeof(CPreprocessorTokenType))]
internal partial class CPreprocessorExpressionParser
{
    [Rule("identifier: PreprocessingToken")]
    private static IPreprocessorExpression MakeIdentifier(ICPreprocessorToken token) => new IdentifierExpression(token.Text);

    [Rule("identifier_defined: 'defined' PreprocessingToken")]
    private static IPreprocessorExpression MakeIdentifier(ICPreprocessorToken definedToken, ICPreprocessorToken token) => new IdentifierExpression(token.Text);

    [Rule("identifier_defined: 'defined' '(' PreprocessingToken ')' ")]
    private static IPreprocessorExpression MakeIdentifier(ICPreprocessorToken definedToken, ICPreprocessorToken openToken, ICPreprocessorToken token, ICPreprocessorToken closedToken) => new IdentifierExpression(token.Text);

    [Rule("simple_expression: identifier")]
    private static IPreprocessorExpression MakeSimpleExpression(IPreprocessorExpression expression) => expression;

    [Rule("binary_expression: simple_expression '==' simple_expression")]
    [Rule("binary_expression: simple_expression '!=' simple_expression")]
    private static BinaryExpression MakeBinaryExpression(IPreprocessorExpression left, ICPreprocessorToken token, IPreprocessorExpression right)
        => new BinaryExpression(left, GetOperator(token), right);

    [Rule("prefix_expression: '!' expression")]
    private static UnaryExpression MakePrefixExpression(ICPreprocessorToken token, IPreprocessorExpression expression)
        => new UnaryExpression(GetOperator(token), expression);

    [Rule("expression: identifier")]
    [Rule("expression: binary_expression")]
    [Rule("expression: prefix_expression")]
    [Rule("expression: identifier_defined")]
    private static IPreprocessorExpression MakeExpression(IPreprocessorExpression expression) => expression;

    private static CPreprocessorOperator GetOperator(ICPreprocessorToken token) => token.Text switch
    {
        "==" => CPreprocessorOperator.Equals,
        "!=" => CPreprocessorOperator.NotEquals,
        "!" => CPreprocessorOperator.Negation,
        _ => throw new CompilationException($"Operator {token.Text} cannot be used in the preprocessor directives"),
    };
}