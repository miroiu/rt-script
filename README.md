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
Context.Declare("int", typeof(int)); // Declare static type
```

Example RTScript code:
```csharp
write("The value of PI is: ", PI);

myList.Add(9);
print myList;

var arr = [1, 2, 3, 4];
print arr[0];

myDictionary.Add('one', 0);
myDictionary["one"] = 1;
 
print myDictionary;

print int.Parse("123");
```

## Specifications:
  - somewhat type safe interpreted language
  - automatic type inference
  - operators can be overloaded
  - statements are delimited by a semicolon (i.e. ";")
  - the decimal separator is dot (i.e. ".")
  - parentheses can be nested
  
Variables:
```csharp
// Declaration and initialization
var myVar = 1;
const PI = 3.14;

// Initialization
myVar = 2;
PI = 1; // prints error
```

Built-in data types:
```csharp
integer: 1
double: 1.0
string: 's' | "s" | "'q'" | '"q"'
boolean: true | false
array of any type: [value, value, value] // where all values have to be the same type
```

Binary operators:
```
add: +
subtract: -
divide: /
multiply: *
equals: ==
not equals: !=
less than: <
greater than: > 
```

Unary operators:
```
minus: -
logical negation: !
```

Commands:
```csharp
print // prints to the output stream
user defined commands:
// C#: Action myCmd = () => Console.WriteLine("C#");
// RTScript: myCmd();
```

Errors:
  - syntax errors and evaluation errors are printed to the console with their respective line and column numbers
  - evaluation and syntax errors are not fail-fast, meaning that the code will execute until an error is found
  - attempting to use a variable that was not assigned any value should throw an evaluation error

Comments:
  - the entire line after '//' is ignored during evaluation 
