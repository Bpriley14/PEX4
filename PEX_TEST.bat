:: Creates a Variable for the Output File
@SET file="pex_test_results.txt"

:: Erases Everything Currently In the Output File
type NUL>%file%

:: ----------------------------------------
:: TITLE
:: ----------------------------------------
echo PEX 3 TEST CASES (C1C Hoang and C1C Riley) >> %file%


:: ----------------------------------------
:: Num Comps
:: ----------------------------------------
echo. >> %file%
echo **Num Compares** >> %file%
echo. >> %file%

echo Testing correct num comp >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\num_comp_correct.txt" >> %file%
echo. >> %file%

echo Testing incorrect num comp (less than type not match) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\num_comp_incorrect1.txt" >> %file%
echo. >> %file%

echo Testing incorrect num comp (less than equal type not match) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\num_comp_incorrect2.txt" >> %file%
echo. >> %file%

echo Testing incorrect num comp (greater than type not match) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\num_comp_incorrect3.txt" >> %file%
echo. >> %file%

echo Testing incorrect num comp (greater than equal type not match) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\num_comp_incorrect4.txt" >> %file%
echo. >> %file%

echo Testing incorrect num comp (equal type not match) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\num_comp_incorrect5.txt" >> %file%
echo. >> %file%

echo Testing incorrect num comp (not equals type not match) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\num_comp_incorrect6.txt" >> %file%
echo. >> %file%

echo Testing incorrect num comp (less than type not int or float) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\num_comp_incorrect7.txt" >> %file%
echo. >> %file%

echo Testing incorrect num comp (less than equal type not int or float) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\num_comp_incorrect8.txt" >> %file%
echo. >> %file%

echo Testing incorrect num comp (greater than type not int or float) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\num_comp_incorrect9.txt" >> %file%
echo. >> %file%

echo Testing incorrect num comp (greater than equal type not int or float) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\num_comp_incorrect10.txt" >> %file%
echo. >> %file%

echo Testing incorrect num comp (equal type not int or float) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\num_comp_incorrect11.txt" >> %file%
echo. >> %file%

echo Testing incorrect num comp (not equals type not int or float) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\num_comp_incorrect12.txt" >> %file%
echo. >> %file%

:: ----------------------------------------
:: Boolean exp
:: ----------------------------------------
echo. >> %file%
echo **Boolean exp** >> %file%
echo. >> %file%

echo Testing correct Boolean exp >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\bool_exp_correct.txt" >> %file%
echo. >> %file%

echo Testing incorrect Boolean exp (and type not match) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\bool_exp_incorrect1.txt" >> %file%
echo. >> %file%

echo Testing incorrect Boolean exp (or type not match) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\bool_exp_incorrect2.txt" >> %file%
echo. >> %file%

echo Testing incorrect Boolean exp (equals type not match) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\bool_exp_incorrect3.txt" >> %file%
echo. >> %file%

echo Testing incorrect Boolean exp (not equals type not match) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\bool_exp_incorrect4.txt" >> %file%
echo. >> %file%

echo Testing incorrect Boolean exp (and type not int or float) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\bool_exp_incorrect5.txt" >> %file%
echo. >> %file%

echo Testing incorrect Boolean exp (or type not int or float) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\bool_exp_incorrect6.txt" >> %file%
echo. >> %file%

echo Testing incorrect Boolean exp (equals type not int or float) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\bool_exp_incorrect7.txt" >> %file%
echo. >> %file%

echo Testing incorrect Boolean exp (not equals type not int or float) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\bool_exp_incorrect8.txt" >> %file%
echo. >> %file%

:: ----------------------------------------
:: Expressions
:: ----------------------------------------
echo. >> %file%
echo **Expressions** >> %file%
echo. >> %file%

echo Testing correct Expressions >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\expression_correct.txt" >> %file%
echo. >> %file%

echo Testing incorrect Expressions (+ type not match) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\expression_incorrect1.txt" >> %file%
echo. >> %file%

echo Testing incorrect Expressions (- type not match) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\expression_incorrect2.txt" >> %file%
echo. >> %file%

echo Testing incorrect Expressions (* type not match) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\expression_incorrect3.txt" >> %file%
echo. >> %file%

echo Testing incorrect Expressions (/ type not match) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\expression_incorrect4.txt" >> %file%
echo. >> %file%

echo Testing incorrect Expressions (+ type not int or float) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\expression_incorrect5.txt" >> %file%
echo. >> %file%

echo Testing incorrect Expressions (- type not int or float) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\expression_incorrect6.txt" >> %file%
echo. >> %file%

echo Testing incorrect Expressions (* type not int or float) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\expression_incorrect7.txt" >> %file%
echo. >> %file%

echo Testing incorrect Expressions (/ type not int or float) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\expression_incorrect8.txt" >> %file%
echo. >> %file%

echo Testing incorrect Expressions (Can't negate string) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\expression_incorrect9.txt" >> %file%
echo. >> %file%

:: ----------------------------------------
:: Assignment
:: ----------------------------------------
echo. >> %file%
echo **Assignment** >> %file%
echo. >> %file%

echo Testing correct Assignment >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\assignment_correct.txt" >> %file%
echo. >> %file%

echo Testing incorrect Assignment (var not declared) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\assignment_incorrect1.txt" >> %file%
echo. >> %file%

echo Testing incorrect Assignment (var is constant)>> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\assignment_incorrect2.txt" >> %file%
echo. >> %file%

echo Testing incorrect Assignment (types don't match)>> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\assignment_incorrect3.txt" >> %file%
echo. >> %file%

:: ----------------------------------------
:: Constants
:: ----------------------------------------
echo. >> %file%
echo **Constants** >> %file%
echo. >> %file%

echo Testing correct Constants >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\const_declarations_correct.txt" >> %file%
echo. >> %file%

echo Testing incorrect constants (var already declared) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\const_declarations_incorrect1.txt" >> %file%
echo. >> %file%

echo Testing incorrect constants (var not declared) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\const_declarations_incorrect2.txt" >> %file%
echo. >> %file%

echo Testing incorrect constants (var not a type) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\const_declarations_incorrect3.txt" >> %file%
echo. >> %file%

echo Testing incorrect constants (types dont match) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\const_declarations_incorrect4.txt" >> %file%
echo. >> %file%

:: ----------------------------------------
:: Declarations
:: ----------------------------------------
echo. >> %file%
echo **Declarations** >> %file%
echo. >> %file%

echo Testing correct Declarations >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\var_dec_correct.txt" >> %file%
echo. >> %file%

echo Testing incorrect Declarations (var already declared) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\var_dec_incorrect1.txt" >> %file%
echo. >> %file%

echo Testing incorrect Declarations (var not declared) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\var_dec_incorrect2.txt" >> %file%
echo. >> %file%

echo Testing incorrect Declarations (var not a type) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\var_dec_incorrect3.txt" >> %file%
echo. >> %file%

:: ----------------------------------------
:: if stmt
:: ----------------------------------------
echo. >> %file%
echo **If Statment** >> %file%
echo. >> %file%

echo Testing correct If Statements >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\if_stmt_correct.txt" >> %file%
echo. >> %file%

echo Testing incorrect If Statements (Not a boolean statement) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\if_stmt_incorrect1.txt" >> %file%
echo. >> %file%

echo Testing incorrect If Statements (Not a boolean statement) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\if_stmt_incorrect2.txt" >> %file%
echo. >> %file%

:: ----------------------------------------
:: while stmt
:: ----------------------------------------
echo. >> %file%
echo **While Statment** >> %file%
echo. >> %file%

echo Testing correct While Statements >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\loop_statement_correct.txt" >> %file%
echo. >> %file%

echo Testing incorrect While Statements (Not a boolean statement) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\loop_statement_incorrect1.txt" >> %file%
echo. >> %file%

echo Testing incorrect While Statements (Not a boolean statement) >> %file%
bin\Debug\ConsoleApplication.exe "testcases\pex3\loop_statement_incorrect2.txt" >> %file%
echo. >> %file%

pause