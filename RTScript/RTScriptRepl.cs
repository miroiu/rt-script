using RTLang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace RTScript
{
    public class RTScriptRepl
    {
        private readonly IExecutionContext _context;
        private readonly HashSet<string> _loadedAssemblies = new HashSet<string>();

        private const string _entryPointTypeName = "RTScriptPlugin";
        private const string _entryPointMethodName = "Inject";
        private const string _pluginExtension = ".dll";

        public RTScriptRepl(IOutputStream output)
            => _context = RTLang.RTScript.NewContext(output);

        public void Evaluate(string code)
            => RTLang.RTScript.Execute(code, _context);

        public bool AddReference(Assembly assembly)
        {
            if (_loadedAssemblies.Add(assembly.FullName))
            {
                var types = assembly.GetExportedTypes();
                foreach (var type in types)
                {
                    if (type.Name == _entryPointTypeName)
                    {
                        var inject = type.GetMethod(_entryPointMethodName);
                        if (inject != default && HasExpectedSignature(inject))
                        {
                            var action = (Action<IExecutionContext>)Delegate.CreateDelegate(typeof(Action<IExecutionContext>), inject);

                            try
                            {
                                _context.Print($"+{assembly.GetName().Name}");
                                action(_context);
                            }
                            catch (Exception ex)
                            {
                                _context.Print(ex.Message);
                            }

                            return true;
                        }
                    }
                }

                _context.Print($"Assembly '{assembly.FullName}' is not a plugin.");
                return false;
            }

            _context.Print($"Assembly '{assembly.FullName}' is already loaded.");
            return false;
        }

        public bool AddReference(string plugin)
        {
            if (Path.GetExtension(plugin) == _pluginExtension && File.Exists(plugin))
            {
                var assembly = Assembly.LoadFrom(plugin);
                return AddReference(assembly);
            }

            _context.Print($"File '{plugin}' not found.");
            return false;
        }

        private bool HasExpectedSignature(MethodInfo inject)
        {
            if (inject.IsStatic && inject.ReturnType == typeof(void))
            {
                var parameters = inject.GetParameters();

                if (parameters.Length == 1)
                {
                    var param = parameters[0];

                    if (param.ParameterType == typeof(IExecutionContext))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
