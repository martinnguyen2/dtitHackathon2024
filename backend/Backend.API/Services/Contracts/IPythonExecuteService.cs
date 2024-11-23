using System;

namespace Backend.API.Services.Contracts;

public interface IPythonExecuteService
{
    Task<string> Execute(string script="main.py", string arguments="");
}
