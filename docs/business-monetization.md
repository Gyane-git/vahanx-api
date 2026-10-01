# VahanX Business & Monetization Module

## Architecture

The Business & Monetization module follows the same Clean Architecture principles as the rest of the VahanX platform.

## Subscription

### Overview
Flexible subscription system with database-driven features and plans.

### Feature System
- Database-driven feature definitions
- Feature types: Boolean, Limit, Quota, Text, Numeric
- Central entitlement service for access control

### Subscription Plans
- Target types: User, Seller, Dealer, ServiceProvider, Business
- Billing cycles: Monthly, Quarterly, Yearly, OneTime
- Trial period support
- Plan features with configurable limits

### Subscription Lifecycle
```
Pending → Trialing → Active → Expired
                    → PastDue
                    → Paused
                    → Cancelled
                    → Suspended
```

### Subscription Usage
- Tracks feature consumption per period
- Enforces limits server-side
- Prevents quota overuse

## Advertisement

### Overview
Professional advertising model with campaigns, creatives, and budget tracking.

### Campaign Lifecycle
```
Draft → PendingReview → Approved → Running → Completed
                        → Rejected    → Paused
                                      → Cancelled
```

### Budget Management
- Daily and total budget support
- Server-side budget tracking
- Prevents overspending

### Placements
- HomepageHero, HomepageBanner, SearchTop, SearchInline
- ListingDetails, CategoryPage, DealerPage
- ServicePage, ChargingPage, FuelStationPage

### Targeting
- VehicleType, VehicleCategory, Brand, Model
- Location, PriceRange, FuelType, AudienceType

## Sell Vehicle

### Overview
Vehicle selling workflow with inspection and offer management.

### Sell Request Lifecycle
```
Draft → Submitted → UnderReview → InspectionScheduled → Inspected → OfferReceived → Negotiation → Accepted
                                                                                    → Rejected
                                                                                    → Cancelled
                                                                                    → Completed
                                                                                    → Expired
```

### Sell Vehicle
- Uses existing Vehicle Core master data
- Links to canonical Vehicle when available
- Supports non-registered vehicles

### Offers
- Buyers can make offers on submitted requests
- Sellers can accept/reject offers
- Offer acceptance is transactional

## Payment

### Overview
Provider-independent payment abstraction.

### Payment Lifecycle
```
Pending → Processing → Succeeded → Refunded
                    → Failed     → PartiallyRefunded
                    → Cancelled
```

### Key Features
- Idempotency key support
- Multiple payment purposes
- Invoice generation
- Refund tracking
- Transaction history

### Security
- No card data storage
- Provider references only
- Server-side amount calculation

## API Endpoints

### Subscriptions
- `GET /api/v1/subscriptions/plans` - List plans
- `GET /api/v1/subscriptions/plans/{id}` - Plan details
- `POST /api/v1/subscriptions/plans` - Create plan
- `PUT /api/v1/subscriptions/plans/{id}` - Update plan
- `GET /api/v1/subscriptions/my` - My subscription
- `GET /api/v1/subscriptions/{id}` - Subscription details
- `POST /api/v1/subscriptions` - Create subscription
- `POST /api/v1/subscriptions/{id}/cancel` - Cancel
- `POST /api/v1/subscriptions/{id}/renew` - Renew
- `POST /api/v1/subscriptions/{id}/change-plan` - Change plan
- `GET /api/v1/subscriptions/{id}/usage` - Usage tracking

### Features
- `GET /api/v1/features` - List features
- `GET /api/v1/features/{id}` - Feature details
- `POST /api/v1/features` - Create feature
- `PUT /api/v1/features/{id}` - Update feature

### Advertisements
- `GET/POST /api/v1/advertisements/campaigns` - Campaign management
- `GET/PUT /api/v1/advertisements/campaigns/{id}` - Campaign details
- `POST /api/v1/advertisements/campaigns/{id}/submit` - Submit for review
- `POST /api/v1/advertisements/campaigns/{id}/pause` - Pause
- `POST /api/v1/advertisements/campaigns/{id}/resume` - Resume
- `POST /api/v1/advertisements/campaigns/{id}/cancel` - Cancel
- `POST /api/v1/advertisements` - Create advertisement
- `GET/PUT /api/v1/advertisements/{id}` - Advertisement details

### Sell Vehicles
- `GET/POST /api/v1/sell-vehicles/requests` - Request management
- `GET/PUT /api/v1/sell-vehicles/requests/{id}` - Request details
- `POST /api/v1/sell-vehicles/requests/{id}/submit` - Submit
- `POST /api/v1/sell-vehicles/requests/{id}/cancel` - Cancel
- `POST /api/v1/sell-vehicles/requests/{id}/vehicle` - Add vehicle
- `GET /api/v1/sell-vehicles/requests/{id}/vehicle` - Get vehicle
- `GET/POST /api/v1/sell-vehicles/requests/{id}/offers` - Offer management
- `POST /api/v1/sell-vehicles/offers/{offerId}/accept` - Accept offer
- `POST /api/v1/sell-vehicles/offers/{offerId}/reject` - Reject offer

### Payments
- `GET /api/v1/payments` - List payments
- `GET /api/v1/payments/{id}` - Payment details
- `POST /api/v1/payments` - Create payment
- `POST /api/v1/payments/{id}/confirm` - Confirm payment
- `POST /api/v1/payments/{id}/fail` - Fail payment
- `GET /api/v1/payments/{id}/invoice` - Get invoice
- `GET /api/v1/payments/invoices` - List invoices
- `POST /api/v1/payments/{id}/refund` - Create refund

## Database Relationships

```
SubscriptionPlan (1) ← (many) SubscriptionPlanFeature → (1) Feature
Subscription (1) ← (1) SubscriptionPlan
Subscription (1) ← (many) SubscriptionUsage → (1) Feature
Subscription (1) ← (many) SubscriptionChangeHistory
AdvertisementCampaign (1) ← (many) Advertisement → (many) AdvertisementCreative
AdvertisementCampaign (1) ← (many) AdvertisementBudget
Advertisement (1) ← (many) AdvertisementTargeting
SellRequest (1) ← (1) SellVehicle
SellRequest (1) ← (many) SellOffer
SellRequest (1) ← (many) SellRequestStatusHistory
Payment (1) ← (many) PaymentItem
Payment (1) ← (many) PaymentTransaction
Payment (1) ← (many) PaymentRefund
Payment (1) ← (many) Invoice → (many) InvoiceItem
```

## Future Extension Points

- Payment provider integration (Stripe, Khalti, eSewa)
- Webhook handling
- Advanced analytics
- Admin dashboard
- Subscription proration
- Advertisement auction system
