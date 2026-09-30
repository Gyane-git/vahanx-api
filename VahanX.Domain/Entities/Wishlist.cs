using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// User's wishlist containing vehicle listings.
/// </summary>
public class Wishlist : BaseEntity
{
    public Guid UserId { get; set; }

    public ICollection<WishlistItem> Items { get; set; } = new List<WishlistItem>();
}
