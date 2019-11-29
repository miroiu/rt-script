![alt text](https://github.com/miroiu/rt-script/blob/master/RTScript/icon.ico "RTScript logo")
# RTScript
An Interpreted scripting language which can interact with its host language.

## Usage from the command line
rtscript -?

## Specifications:
  - somewhat type safe interpreted language
  - operators can be overloaded
  - execution is single threaded and sequential, all statements are executed in the order they are encountered
  
Language rules: 
  - statements are delimited by a semicolon (i.e. ";")
  - the decimal separator is comma (i.e. ".")
  - empty statements are permitted and ignored during evaluation
  - a statement can be written on a single or multiple lines
  - spaces between an operator and its operands are optional

Data types:
  - a numeric type (double)
  - string
  - boolean
  - array of string, boolean and number

Variables:
  - variable declarations consists of the 'var' keyword followed by the variable's name or the 'const' keyword for variables that cannot be reasigned
  - variables name consist exclusively of letters and/or underscore and numbers
  - variables declaration and initialization must be a single statement so the data type can be infered (e.g. var a = 5;)
  - reserved words of the language can't be used as variable names
  - reserved words are: var, const, print, true, false, null

Binary operators:
  - \+ -> addition (e.g 2 + 3 = 5)
  - \- -> subtraction (e.g 2 - 1 = 1)
  - / -> division (e.g 6 : 2 = 3)
  - * -> multiplication (e.g 3 x 2 = 6)
  - == -> equals
  - != -> not equals
  - < -> less
  - > -> greater
  
Unary operators:
  - \- -> (for negative numbers, e.g. -1)
  - ! -> logical negation (e.g !true)

Commands:
  - print (prints to the output stream, e.g. print 2 + 3)
  - user defined commands

Operations grouping:
  - parentheses with unlimited nesting depth

Errors:
  - syntax errors and evaluation errors are printed to the console with their respective line and column numbers
  - evaluation and syntax errors are not fail-fast, meaning that the code will execute until an error is found
  - attempting to use a variable that was not assigned any value should throw an evaluation error

Comments:
  - the entire line after '//' is ignored during evaluation 
