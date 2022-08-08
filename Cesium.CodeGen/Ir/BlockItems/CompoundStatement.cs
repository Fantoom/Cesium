using Cesium.CodeGen.Contexts;
using Cesium.CodeGen.Extensions;

namespace Cesium.CodeGen.Ir.BlockItems;

internal class CompoundStatement : IBlockItem
{
    private readonly List<IBlockItem> _items;

    private CompoundStatement(List<IBlockItem> items)
    {
        _items = items;
    }

    public CompoundStatement(Ast.CompoundStatement statement)
        : this(statement.Block.Select(x => x.ToIntermediate()).ToList())
    {
    }

    bool IBlockItem.HasDefiniteReturn => _items.Any(x => x.HasDefiniteReturn);

    public IBlockItem Lower() => this; // since actual lowering of child items is done on emit, anyway

    public void EmitTo(IDeclarationScope scope)
    {
        List<Exception> exceptions = new();
        foreach (var item in _items)
        {
            try
            {
                item.Lower().EmitTo(scope);

            }
            catch (Exception e)
            {
                exceptions.Add(e);
            }
        }
        if (exceptions.Count > 0)
            throw new AggregateException(exceptions);
    }
}
