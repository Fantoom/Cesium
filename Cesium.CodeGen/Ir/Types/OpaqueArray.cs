using Cesium.CodeGen.Contexts;
using Cesium.Core;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.CodeGen.Ir.Types;  

internal record OpaqueArray(IType Base) : IType
{
    public int? GetSizeInBytes(TargetArchitectureSet arch)  =>
        throw new CompilationException("Can't determine OpaqueArray size");

    public TypeReference Resolve(TranslationUnitContext context) =>
        throw new CompilationException("Can't resolve OpaqueArray");
}
