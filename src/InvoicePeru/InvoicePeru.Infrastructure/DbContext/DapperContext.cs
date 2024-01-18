namespace InvoicePeru.Infrastructure.DbContext;
public class DapperContext
{
    private readonly DapperSettings _dapperSettings;

    public DapperContext(IOptions<DapperSettings> DapperSettings)
    {
        _dapperSettings = DapperSettings.Value;
    }

    public IDbConnection CreateConnection()
        => new SqlConnection(_dapperSettings.SqlServer);
}