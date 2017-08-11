/* Generated By:JJTree: Do not edit this line. IDLParserVisitor.cs */

using System;

namespace parser {

public interface IDLParserVisitor
{
  Object visit(SimpleNode node, Object data);
  Object visit(ASTspecification node, Object data);
  Object visit(ASTdefinition node, Object data);
  Object visit(ASTmodule node, Object data);
  Object visit(ASTinterfacex node, Object data);
  Object visit(ASTinterface_dcl node, Object data);
  Object visit(ASTforward_dcl node, Object data);
  Object visit(ASTinterface_header node, Object data);
  Object visit(ASTinterface_body node, Object data);
  Object visit(ASTexport node, Object data);
  Object visit(ASTinterface_inheritance_spec node, Object data);
  Object visit(ASTinterface_name node, Object data);
  Object visit(ASTscoped_name node, Object data);
  Object visit(ASTvalue node, Object data);
  Object visit(ASTvalue_forward_decl node, Object data);
  Object visit(ASTvalue_box_decl node, Object data);
  Object visit(ASTvalue_abs_decl node, Object data);
  Object visit(ASTvalue_decl node, Object data);
  Object visit(ASTvalue_header node, Object data);
  Object visit(ASTvalue_base_inheritance_spec node, Object data);
  Object visit(ASTvalue_support_inheritance_spec node, Object data);
  Object visit(ASTvalue_name node, Object data);
  Object visit(ASTvalue_element node, Object data);
  Object visit(ASTstate_member node, Object data);
  Object visit(ASTinit_decl node, Object data);
  Object visit(ASTinit_param_delcs node, Object data);
  Object visit(ASTinit_param_decl node, Object data);
  Object visit(ASTinit_param_attribute node, Object data);
  Object visit(ASTconst_dcl node, Object data);
  Object visit(ASTconst_type node, Object data);
  Object visit(ASTconst_exp node, Object data);
  Object visit(ASTor_expr node, Object data);
  Object visit(ASTxor_expr node, Object data);
  Object visit(ASTand_expr node, Object data);
  Object visit(ASTshift_expr node, Object data);
  Object visit(ASTadd_expr node, Object data);
  Object visit(ASTmult_expr node, Object data);
  Object visit(ASTunary_expr node, Object data);
  Object visit(ASTprimary_expr node, Object data);
  Object visit(ASTliteral node, Object data);
  Object visit(ASTpositive_int_const node, Object data);
  Object visit(ASTtype_dcl node, Object data);
  Object visit(ASTtype_declarator node, Object data);
  Object visit(ASTtype_spec node, Object data);
  Object visit(ASTsimple_type_spec node, Object data);
  Object visit(ASTbase_type_spec node, Object data);
  Object visit(ASTtemplate_type_spec node, Object data);
  Object visit(ASTconstr_type_spec node, Object data);
  Object visit(ASTdeclarators node, Object data);
  Object visit(ASTdeclarator node, Object data);
  Object visit(ASTsimple_declarator node, Object data);
  Object visit(ASTcomplex_declarator node, Object data);
  Object visit(ASTfloating_pt_type node, Object data);
  Object visit(ASTfloating_pt_type_float node, Object data);
  Object visit(ASTfloating_pt_type_double node, Object data);
  Object visit(ASTfloating_pt_type_longdouble node, Object data);
  Object visit(ASTinteger_type node, Object data);
  Object visit(ASTsigned_int node, Object data);
  Object visit(ASTsigned_short_int node, Object data);
  Object visit(ASTsigned_long_int node, Object data);
  Object visit(ASTsigned_longlong_int node, Object data);
  Object visit(ASTunsigned_int node, Object data);
  Object visit(ASTunsigned_short_int node, Object data);
  Object visit(ASTunsigned_long_int node, Object data);
  Object visit(ASTunsigned_longlong_int node, Object data);
  Object visit(ASTchar_type node, Object data);
  Object visit(ASTwide_char_type node, Object data);
  Object visit(ASTboolean_type node, Object data);
  Object visit(ASToctet_type node, Object data);
  Object visit(ASTany_type node, Object data);
  Object visit(ASTobject_type node, Object data);
  Object visit(ASTstruct_type node, Object data);
  Object visit(ASTmember_list node, Object data);
  Object visit(ASTmember node, Object data);
  Object visit(ASTunion_type node, Object data);
  Object visit(ASTswitch_type_spec node, Object data);
  Object visit(ASTswitch_body node, Object data);
  Object visit(ASTcasex node, Object data);
  Object visit(ASTcase_label node, Object data);
  Object visit(ASTelement_spec node, Object data);
  Object visit(ASTenum_type node, Object data);
  Object visit(ASTenumerator node, Object data);
  Object visit(ASTsequence_type node, Object data);
  Object visit(ASTstring_type node, Object data);
  Object visit(ASTwide_string_type node, Object data);
  Object visit(ASTarray_declarator node, Object data);
  Object visit(ASTfixed_array_size node, Object data);
  Object visit(ASTattr_dcl node, Object data);
  Object visit(ASTexcept_dcl node, Object data);
  Object visit(ASTop_dcl node, Object data);
  Object visit(ASTop_type_spec node, Object data);
  Object visit(ASTparameter_dcls node, Object data);
  Object visit(ASTparam_dcl node, Object data);
  Object visit(ASTparam_attribute node, Object data);
  Object visit(ASTraises_expr node, Object data);
  Object visit(ASTcontext_expr node, Object data);
  Object visit(ASTparam_type_spec node, Object data);
  Object visit(ASTfixed_pt_type node, Object data);
  Object visit(ASTfixed_pt_const_type node, Object data);
  Object visit(ASTvalue_base_type node, Object data);
}


}

