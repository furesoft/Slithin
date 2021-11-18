using System;

namespace Slithin.ModuleSystem.WASInterface;

public class ProcExitException : Exception
{
    public ProcExitException(int rc)
    {
        ReturnCode = rc;
    }

    public int ReturnCode { get; }
}