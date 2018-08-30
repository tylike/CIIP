;#DropDB IMatrix.ERPEasyTest

#Application IMatrix.ERP.Win
#Application IMatrix.ERP.Web

*FillForm
 User Name = Admin
*Action Log On

#IfDef IMatrix.ERP.Win

*Action About...
 
*Action OK

#EndIf

#IfDef IMatrix.ERP.Web
*Action Navigation(My Details)
#EndIf
