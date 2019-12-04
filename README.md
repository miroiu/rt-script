![alt text](https://github.com/miroiu/rt-script/blob/master/RTScript/icon.ico "RTScript logo")
# RTScript
An Interpreted scripting language which can interact with its host language.

## Usage from the command line
rtscript -?

Example C# code:
```csharp
Action<string, object> write = (str, arg) => Context.Print($"{str}{arg}");
Context.Declare("write", write, isConst: true);

Context.Declare("PI", 3.14, isConst: true);
Context.Declare("myList", new List<int>{ 0, 1, 2, 3, 4, 5, 6, 7, 8 }, isConst: true);
Context.Declare("myDictionary", new Dictionary<string, int>(), isConst: true);
```

Example RTScript code:
```csharp
write("The value of PI is: ", PI);

myList.Add(9);
print(myList);

var arr = [1, 2, 3, 4];
print arr[0];

myDictionary.Add('one', 0);
myDictionary["one"] = 1;
 
print myDictionary;
```

## Specifications:
  - somewhat type safe interpreted language
  - operators can be overloaded
  - execution is single threaded and sequential, all statements are executed in the order they are encountered
  
Language rules: 
  - statements are delimited by a semicolon (i.e. ";")
  - the decimal separator is dot (i.e. ".")
  - empty statements are permitted and ignored during evaluation
  - a statement can be written on a single or multiple lines
  - spaces between an operator and its operands are optional

Data types:
  - number (double)
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
  - \+ -> addition
  - \- -> subtraction
  - / -> division
  - \* -> multiplication
  - == -> equals
  - != -> not equals
  - &lt; -> less than
  - &gt; -> greater than
  
Unary operators:
  - \- -> minus
  - ! -> logical negation

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
