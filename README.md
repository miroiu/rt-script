![alt text](https://github.com/miroiu/rt-script/blob/master/RTScript/icon.ico "RTScript logo")
# RTScript
An interpreted scripting language with syntax similar to C#.

## Usage from the command line
```
// shows all commands
rtscript -?

// shows the repl version
rtscript -v 

// starts the repl mode
rtscript -r

// evaluates files
rtscript -f myscript.rt myotherscript.rt

// starts the repl mode with plugins
rtscript -r --add rtstdlib.dll mylib.dll

// evaluates files with plugins
rtscript -f myscript.rt --add rtstdlib.dll
```

## Features:
  - type safe
  - type inference
  - operators can be overloaded
  - implicit type conversion
  - very similar to C#
  
## Examples:
C#:
```csharp
Action<string, object> write = (str, arg) => Context.Print($"{str}{arg}");
Context.Declare("write", write, isConst: true);

Context.Declare("PI", 3.14, isConst: true);
Context.Declare("myList", new List<int>{ 0, 1, 2, 3, 4, 5, 6, 7, 8 }, isConst: true);
Context.Declare("myDictionary", new Dictionary<string, int>(), isConst: true);

Context.Declare("int", typeof(int)); // Declare static type
```

RTScript:
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

## Plugins:
```csharp
using RTLang;

namespace MyPlugin
{
    public static class RTScriptPlugin
    {
        public static void Inject(IExecutionContext context)
        {
            context.Print("Hello from plugin!");
        }
    }
}
```
Plugins must have a public class named RTScriptPlugin with a public static Inject method that takes an IExecutionContext as the parameter and doesn't return anything.
 
## Variables:
```csharp
// Declaration and initialization
var myVar = 1;
const PI = 3.14;

// Initialization
myVar = 2;
PI = 1; // prints error
```

## Built-in data types:
```csharp
integer: 1
double: 1.0
string: 's' | "s" | "'q'" | '"q"'
boolean: true | false
array of any type: [value, value, value] // where all values have to be of the same type
```

## Binary operators:
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

## Unary operators:
```
minus: -
logical negation: !
```

## Commands:
```csharp
print 'hello'; // prints to the output stream
```

## Errors:
  - syntax errors and evaluation errors are printed to the console with their respective line and column numbers
  - evaluation and syntax errors are not fail-fast, meaning that the code will execute until an error is found

## Comments:
  - the entire line after '//' is ignored during evaluation 
