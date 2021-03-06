// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Cci;

namespace VBSourceEmitter {
  public partial class SourceEmitter : CodeTraverser, IVBSourceEmitter {
    public virtual void PrintBaseTypesAndInterfacesList(ITypeDefinition typeDefinition) {
      IEnumerable<ITypeReference> basesList = typeDefinition.BaseClasses;
      IEnumerable<ITypeReference> interfacesList = typeDefinition.Interfaces;

      bool fFirstBase = true;
      if (typeDefinition.IsEnum && typeDefinition.UnderlyingType.TypeCode != PrimitiveTypeCode.Int32) {
        PrintBaseTypesAndInterfacesColon();
        PrintBaseTypeOrInterface(typeDefinition.UnderlyingType);
        fFirstBase = false;
      }

      foreach (ITypeReference baseTypeReference in basesList) {
        if (fFirstBase && TypeHelper.TypesAreEquivalent(baseTypeReference, typeDefinition.PlatformType.SystemObject))
          continue;

        if (fFirstBase && TypeHelper.TypesAreEquivalent(baseTypeReference, typeDefinition.PlatformType.SystemValueType))
          continue;

        if (TypeHelper.TypesAreEquivalent(baseTypeReference, typeDefinition.PlatformType.SystemEnum))
          continue;

        if (fFirstBase)
          PrintBaseTypesAndInterfacesColon();
        else
          PrintBaseTypesAndInterfacesListDelimiter();

        PrintBaseTypeOrInterface(baseTypeReference);
        fFirstBase = false;
      }

      foreach (ITypeReference interfaceTypeReference in interfacesList) {
        if (fFirstBase)
          PrintBaseTypesAndInterfacesColon();
        else
          PrintBaseTypesAndInterfacesListDelimiter();

        PrintBaseTypeOrInterface(interfaceTypeReference);
        fFirstBase = false;
      }
    }

    public virtual void PrintBaseTypesAndInterfacesColon() {
      PrintToken(VBToken.Space);
      PrintToken(VBToken.Colon);
      PrintToken(VBToken.Space);
    }

    public virtual void PrintBaseTypesAndInterfacesListDelimiter() {
      PrintToken(VBToken.Comma);
      PrintToken(VBToken.Space);
    }

    public virtual void PrintBaseTypeOrInterface(ITypeReference baseTypeReference) {
      PrintBaseTypeOrInterfaceName(baseTypeReference);
    }

  }
}
