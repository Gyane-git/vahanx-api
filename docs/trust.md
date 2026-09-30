# VahanX Trust Module

## Architecture

The Trust module follows the same Clean Architecture principles as the rest of the VahanX platform.

## Trust Principle

VahanX must NEVER falsely represent seller-provided information as independently verified.

**Seller Provided Information** is clearly distinguished from **VahanX Verified Information**.

## Verification Model

### VehicleVerification

Represents VahanX's verification process for a vehicle/listing.

**Status Lifecycle:**
```
NotVerified → Pending → InReview → Verified → Expired/Revoked
                                    → Rejected
```

| Status | Description |
|--------|-------------|
| NotVerified | No verification attempted |
| Pending | Verification created, awaiting submission |
| InReview | Under review by VahanX staff |
| Verified | Successfully verified |
| Rejected | Verification failed |
| Expired | Previously verified, now expired |
| Revoked | Previously verified, now revoked |

### Verification Types

- BasicDocumentVerification
- VehicleIdentityVerification
- OwnershipVerification
- RegistrationVerification
- InsuranceVerification
- InspectionBasedVerification

### VerificationDocument

Supporting documents for verification. Uses media reference, not binary storage.

### VehicleVerificationStatusHistory

Append-only audit trail for verification status changes.

## Inspection Model

### Inspection

Physical/technical inspection of a vehicle.

**Status Lifecycle:**
```
Scheduled → InProgress → Completed
                    → Cancelled
```

### Inspection Types

- PreSale
- DealerInspection
- VahanXInspection
- ServiceInspection

### InspectionItem

Individual inspection items with condition ratings:
- NotInspected, Excellent, Good, Fair, Poor, Critical

### InspectionReport

Generated inspection report with overall score and recommendations.

## Vehicle History

All history records are **append-only** and **immutable** once finalized.

### OwnershipHistory
Tracks vehicle ownership changes over time.

### MileageHistory
Tracks mileage recordings over time. Helps detect suspicious mileage changes.

### ServiceHistory
Tracks vehicle service records.

### AccidentHistory
Tracks accident records with severity and repair status.

### InsuranceHistory
Tracks insurance policy history.

### RegistrationHistory
Tracks vehicle registration history.

### PriceHistory
Tracks marketplace price changes for listings. Append-only.

## Reviews & Ratings

### Review Types
- VehicleReview
- SellerReview
- DealerReview

### Review Status
```
Pending → Published → Hidden/Reported
       → Rejected
```

### Rating Summary
- AverageRating
- TotalReviews
- Rating1Count through Rating5Count

## Trust Badges

Trust badges are derived from actual valid verification/inspection states:
- **Verified Vehicle**: Active verification exists
- **Inspected Vehicle**: Completed inspection exists
- **Verified Seller**: Seller verification is active
- **Verified Dealer**: Dealer verification is active

No badge is shown when the underlying verification is expired/revoked.

## Public vs Private Data

### Public Data
- Verification status
- Verification type
- Verified date
- Expiry date
- Public verification reference

### Private Data (Never exposed publicly)
- Internal reviewer notes
- Private documents
- Sensitive document numbers
- Internal moderation information
- Private user information
- Insurance details
- Registration references

## API Endpoints

### Verifications
- `GET /api/v1/verifications` - Get all verifications
- `GET /api/v1/verifications/{id}` - Get verification by ID
- `GET /api/v1/verifications/vehicle/{vehicleId}` - Get public verification for vehicle
- `POST /api/v1/verifications` - Create verification
- `PUT /api/v1/verifications/{id}` - Update verification
- `DELETE /api/v1/verifications/{id}` - Delete verification
- `POST /api/v1/verifications/{id}/submit` - Submit for review
- `POST /api/v1/verifications/{id}/approve` - Approve verification
- `POST /api/v1/verifications/{id}/reject` - Reject verification
- `POST /api/v1/verifications/{id}/revoke` - Revoke verification

### Inspections
- `GET /api/v1/inspections` - Get all inspections
- `GET /api/v1/inspections/{id}` - Get inspection by ID
- `POST /api/v1/inspections` - Create inspection
- `PUT /api/v1/inspections/{id}` - Update inspection
- `DELETE /api/v1/inspections/{id}` - Delete inspection
- `POST /api/v1/inspections/{id}/start` - Start inspection
- `POST /api/v1/inspections/{id}/complete` - Complete inspection
- `POST /api/v1/inspections/{id}/cancel` - Cancel inspection
- `GET /api/v1/inspections/{id}/items` - Get inspection items
- `POST /api/v1/inspections/{id}/items` - Add inspection item
- `PUT /api/v1/inspections/{id}/items/{itemId}` - Update inspection item

### Vehicle History
- `GET /api/v1/vehicles/{vehicleId}/history` - Get aggregated history
- `GET /api/v1/vehicles/{vehicleId}/history/ownership` - Get ownership history
- `GET /api/v1/vehicles/{vehicleId}/history/mileage` - Get mileage history
- `GET /api/v1/vehicles/{vehicleId}/history/service` - Get service history
- `GET /api/v1/vehicles/{vehicleId}/history/accident` - Get accident history
- `GET /api/v1/vehicles/{vehicleId}/history/insurance` - Get insurance history
- `GET /api/v1/vehicles/{vehicleId}/history/registration` - Get registration history
- `GET /api/v1/vehicles/{vehicleId}/history/price/{listingId}` - Get price history

### Reviews
- `GET /api/v1/vehicles/{vehicleId}/reviews` - Get vehicle reviews
- `POST /api/v1/vehicles/{vehicleId}/reviews` - Create vehicle review
- `GET /api/v1/sellers/{sellerId}/reviews` - Get seller reviews
- `POST /api/v1/sellers/{sellerId}/reviews` - Create seller review
- `GET /api/v1/dealers/{dealerId}/reviews` - Get dealer reviews
- `POST /api/v1/dealers/{dealerId}/reviews` - Create dealer review
- `GET /api/v1/reviews/{id}` - Get review by ID
- `PUT /api/v1/reviews/{id}` - Update review
- `DELETE /api/v1/reviews/{id}` - Delete review
- `POST /api/v1/reviews/{id}/report` - Report review
- `GET /api/v1/vehicles/{vehicleId}/rating-summary` - Get vehicle rating summary
- `GET /api/v1/sellers/{sellerId}/rating-summary` - Get seller rating summary
- `GET /api/v1/dealers/{dealerId}/rating-summary` - Get dealer rating summary

## Database Relationships

```
Vehicle (1) ← (many) VehicleVerification
Vehicle (1) ← (many) Inspection
Vehicle (1) ← (many) OwnershipHistory
Vehicle (1) ← (many) MileageHistory
Vehicle (1) ← (many) ServiceHistory
Vehicle (1) ← (many) AccidentHistory
Vehicle (1) ← (many) InsuranceHistory
Vehicle (1) ← (many) RegistrationHistory
VehicleListing (1) ← (many) PriceHistory
VehicleVerification (1) ← (many) VerificationDocument
VehicleVerification (1) ← (many) VehicleVerificationStatusHistory
Inspection (1) ← (many) InspectionItem
Inspection (1) ← (1) InspectionReport
Inspection (1) ← (many) InspectionMedia
```

## Future Extension Points

- Phase 5: Enquiry + Lead + Test Drive
- Future: Notifications, Messaging, Finance, Services, EV Charging, Advertisement, Subscription, Analytics, Admin moderation
