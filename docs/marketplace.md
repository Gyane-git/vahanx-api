# VahanX Marketplace Module

## Architecture

The Marketplace module follows the same Clean Architecture principles as the rest of the VahanX platform:

```
VahanX.Domain (innermost - no dependencies)
    ↓
VahanX.Application (depends on Domain)
    ↓
VahanX.Infrastructure (depends on Application + Domain)
    ↓
VahanX.Api (depends on all)
```

## Vehicle vs VehicleListing

**Vehicle** is the canonical vehicle definition (e.g., "Toyota Corolla 1.8 GLi"). It represents the master data for a specific vehicle configuration.

**VehicleListing** is a marketplace offer for a specific vehicle. It contains seller-specific information like price, mileage, condition, and location.

A single Vehicle definition may have multiple listings over time from different sellers.

## Seller Model

- **Seller** represents an individual marketplace seller profile
- Seller is separate from User (authentication account)
- A User can have one Seller profile
- Seller contains marketplace-specific information (display name, contact, verification status)

## Dealer Model

- **Dealer** represents a business/showroom
- A Dealer can have multiple **DealerBranch** locations
- A Dealer can have multiple **DealerStaff** members
- DealerStaff links to User for future authorization integration

## Listing Ownership

A listing can belong to either:
- An **Individual Seller** (SellerId is set, DealerId is null)
- A **Dealer** (DealerId is set, SellerId is null)

Both cannot be null. Both cannot be set simultaneously (enforced by business rules).

## Listing Lifecycle

```
Draft → PendingReview → Published → Sold/Expired/Archived
```

| Status | Description |
|--------|-------------|
| Draft | Initial state, editable by seller |
| PendingReview | Submitted for future admin moderation |
| Published | Live and visible in marketplace search |
| Paused | Temporarily hidden from search |
| Sold | Vehicle has been sold |
| Expired | Listing has expired |
| Rejected | Rejected by admin (future) |
| Archived | Removed from active listings |

### State Transitions

- **Submit**: Draft/Rejected → PendingReview
- **Publish**: PendingReview/Draft → Published
- **Pause**: Published → Paused
- **Mark Sold**: Published/Paused → Sold
- **Archive**: Any (except Sold/Archived) → Archived

## Media Architecture

- Uses URL/storage reference architecture (no binary storage in SQL Server)
- Ready for future S3, Azure Blob, Cloudinary integration
- One primary image per listing
- Display order supported for multiple images

## Search Architecture

- Database-backed search using EF Core
- Only Published listings appear in public search
- Supports filtering by: keyword, brand, model, variant, fuel type, transmission, body type, location, seller, dealer, price range, mileage range, manufacture year
- Supports sorting by: newest, price low/high, mileage low/high, year newest
- Uses AsNoTracking() for read-only queries
- Uses projection to DTOs to avoid loading full entity graphs

## Wishlist

- Users can save listings to their wishlist
- Prevents duplicate items
- Compatible with future authentication

## Compare

- Users can compare multiple listings
- Maximum 4 items (configurable via MaxCompareItems constant)
- Prevents duplicate items

## API Endpoints

### Listings
- `GET /api/v1/listings` - Get all listings (paginated, filterable)
- `GET /api/v1/listings/{id}` - Get listing by ID
- `POST /api/v1/listings` - Create listing (Draft status)
- `PUT /api/v1/listings/{id}` - Update listing
- `DELETE /api/v1/listings/{id}` - Delete listing
- `POST /api/v1/listings/{id}/submit` - Submit for review
- `POST /api/v1/listings/{id}/publish` - Publish listing
- `POST /api/v1/listings/{id}/pause` - Pause listing
- `POST /api/v1/listings/{id}/mark-sold` - Mark as sold
- `POST /api/v1/listings/{id}/archive` - Archive listing

### Sellers
- `GET /api/v1/sellers` - Get all sellers
- `GET /api/v1/sellers/{id}` - Get seller by ID
- `POST /api/v1/sellers` - Create seller
- `PUT /api/v1/sellers/{id}` - Update seller
- `DELETE /api/v1/sellers/{id}` - Delete seller
- `GET /api/v1/sellers/{id}/listings` - Get seller's listings

### Dealers
- `GET /api/v1/dealers` - Get all dealers
- `GET /api/v1/dealers/{id}` - Get dealer by ID
- `POST /api/v1/dealers` - Create dealer
- `PUT /api/v1/dealers/{id}` - Update dealer
- `DELETE /api/v1/dealers/{id}` - Delete dealer
- `GET /api/v1/dealers/{id}/listings` - Get dealer's listings
- `GET /api/v1/dealers/{id}/branches` - Get dealer branches
- `POST /api/v1/dealers/{id}/branches` - Create branch
- `PUT /api/v1/dealers/{id}/branches/{branchId}` - Update branch
- `DELETE /api/v1/dealers/{id}/branches/{branchId}` - Delete branch

### Listing Media
- `GET /api/v1/listings/{id}/media` - Get listing media
- `POST /api/v1/listings/{id}/media` - Add media
- `PUT /api/v1/listings/{id}/media/{mediaId}` - Update media
- `DELETE /api/v1/listings/{id}/media/{mediaId}` - Delete media
- `POST /api/v1/listings/{id}/media/{mediaId}/primary` - Set primary

### Search
- `GET /api/v1/search/vehicles` - Search published listings

### Wishlist
- `GET /api/v1/wishlist` - Get user's wishlist
- `POST /api/v1/wishlist/items/{listingId}` - Add to wishlist
- `DELETE /api/v1/wishlist/items/{listingId}` - Remove from wishlist

### Compare
- `GET /api/v1/compare` - Get compare list
- `POST /api/v1/compare/items/{listingId}` - Add to compare
- `DELETE /api/v1/compare/items/{listingId}` - Remove from compare

## Database Relationships

```
Vehicle (1) ← (many) VehicleListing
Seller (1) ← (many) VehicleListing
Dealer (1) ← (many) VehicleListing
Dealer (1) ← (many) DealerBranch
Dealer (1) ← (many) DealerStaff
VehicleListing (1) ← (many) ListingMedia
Wishlist (1) ← (many) WishlistItem → (1) VehicleListing
CompareList (1) ← (many) CompareItem → (1) VehicleListing
```

## Future Extension Points

- Phase 4: Verification + Inspection + Vehicle History
- Phase 5: Enquiry + Lead + Test Drive
- Future: Reviews, Notifications, Messaging, Finance, Services, EV Charging, Advertisement, Subscription, Analytics, Admin moderation
