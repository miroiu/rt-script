using System;
using System.Collections.Generic;

namespace RTLang
{
    public interface IExecutionContext
    {
        // Assign a value to a variable
        void Assign(string name, object value);

        // Creates a variable with an initial value
        void Declare(string name, object value, bool isConst = false);

        // Declare a type
        void Declare(Type type);

        // Returns the value of a variable
        object GetValue(string name);

        // Returns the type of a variable
        Type GetType(string name);

        // Tells if 'name' is a type
        bool IsType(string name);

        // Tells if variable can be written to
        bool IsReadOnly(string name);

        // Writes to the output stream
        void Print(object value);

        // Transforms a string literal into an object
        object Evaluate(LiteralType type, string value);

        // Applies an unary operator to a value
        object Evaluate(UnaryOperatorType operatorType, object value);

        // Applies a binary operator 
        object Evaluate(BinaryOperatorType operatorType, object left, object right);

        // Returns all the variables
        IEnumerable<string> GetVariables();

        // Returns all the types
        IEnumerable<string> GetTypes();
    }
}