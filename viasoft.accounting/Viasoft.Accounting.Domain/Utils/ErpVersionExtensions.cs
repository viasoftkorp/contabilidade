using System;
using Viasoft.Core.MultiTenancy.Abstractions.Environment.Model;

namespace Viasoft.Accounting.Domain.Utils;

public static class ErpVersionExtensions
{
    public static string VersionWithoutBuild(this OrganizationUnitEnvironment environmentDetails)
    {
        if (string.IsNullOrWhiteSpace(environmentDetails.DesktopDatabaseVersion) || environmentDetails.DesktopDatabaseVersion.ToLower() == "Development".ToLower())
        {
            return "Development";
        }

        var desktopDatabaseVersionSplited = environmentDetails.DesktopDatabaseVersion.Split(".");
        if (desktopDatabaseVersionSplited.Length < 3)
        {
            throw new Exception("Invalid environment version");
        }
        var desktopDatabaseVersionWithoutVersion = desktopDatabaseVersionSplited[0] + "." +
                                                   desktopDatabaseVersionSplited[1] + "." +
                                                   desktopDatabaseVersionSplited[2];
        return desktopDatabaseVersionWithoutVersion;
    }
}
