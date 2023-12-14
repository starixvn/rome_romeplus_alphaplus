namespace AlphaPlus.SecurityCode.Configurations;

public class SecurityCodeOptions
{
    public bool IsActive { get; set; }

    public IEnumerable<string> Paths { get; set; }

    public IEnumerable<string> BetListsPaths { get; set; } = Enumerable.Empty<string>();

    public IEnumerable<string> StatementPaths { get; set; } = Enumerable.Empty<string>();

    public IEnumerable<string> Sites { get; set; }

    public string SiteMainApp { get; set; }

    public string VerifyingSecurityCodePath { get; set; }

    public string ResetSecurityCodePath { get; set; }

    public string VerifyingOtpPath { get; set; }

    public bool IsSiteMain { get; set; }
}
