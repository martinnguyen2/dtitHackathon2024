using System;

namespace Backend.API.Services.Contracts;

public interface IPythonExecuteService
{
    string Execute(string script="main.py", string arguments="");
}
