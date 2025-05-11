using Microsoft.EntityFrameworkCore;

namespace Marketplace.API;

public class ShopDbCacheKey
{
    private readonly Type _dbContextType;
    private readonly bool _designTime;
    private readonly string? _schema;

    public ShopDbCacheKey(DbContext context, bool designTime)
    {
        _dbContextType = context.GetType();
        _designTime = designTime;
        _schema = (context as IShopDbContext)?.Schema;
    }

    protected bool Equals(ShopDbCacheKey other) =>
        _dbContextType == other._dbContextType
        && _designTime == other._designTime
        && _schema == other._schema;

    public override bool Equals(object? obj) =>
        (obj is ShopDbCacheKey otherAsKey) && Equals(otherAsKey);

    public override int GetHashCode()
    {
        HashCode hash = new();
        hash.Add(_dbContextType);
        hash.Add(_designTime);
        hash.Add(_schema);

        return hash.ToHashCode();
    }
}
