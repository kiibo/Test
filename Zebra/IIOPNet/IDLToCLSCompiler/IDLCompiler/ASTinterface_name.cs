/* Generated By:JJTree: Do not edit this line. ASTinterface_name.cs */

using System;

namespace parser {

public class ASTinterface_name : SimpleNode {
  public ASTinterface_name(int id) : base(id) {
  }

  public ASTinterface_name(IDLParser p, int id) : base(p, id) {
  }


  /** Accept the visitor. **/
  public override Object jjtAccept(IDLParserVisitor visitor, Object data) {
    return visitor.visit(this, data);
  }
}


}

